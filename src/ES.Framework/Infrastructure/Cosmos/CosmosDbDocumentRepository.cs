﻿using ES.Framework.Domain.Repositories;
using ES.Framework.Domain.Repositories.Design;
using ES.Framework.Infrastructure.Cosmos.Exceptions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;

namespace ES.Framework.Infrastructure.Cosmos;

/// <inheritdoc />
public class CosmosDocumentRepository : IDocumentRepository
{
	 private readonly Container _container;

	 /// <summary>Initializes a new instance of the <see cref="CosmosDocumentRepository" /> class.</summary>
	 /// <param name="container">The container.</param>
	 public CosmosDocumentRepository(Container container) => _container = container;

	 /// <inheritdoc />
	 public async Task<bool> AddAsync<TDocument>(IReadOnlyCollection<TDocument> documents, CancellationToken cancellationToken)
		  where TDocument : Document {
		  ArgumentNullException.ThrowIfNull(documents);

		  if(!documents.Any())
				return true;

		  var partitionKey = DeterminePartitionKey(documents);
		  var transactionalBatch = _container.CreateTransactionalBatch(partitionKey);
		  foreach(var document in documents)
				transactionalBatch.CreateItem(document);
		  var itemResponse = await transactionalBatch.ExecuteAsync(cancellationToken);
		  return itemResponse.StatusCode.Equals(HttpStatusCode.OK);
	 }

	 /// <inheritdoc />
	 public async Task<bool> AddAsync<TDocument>(TDocument document, CancellationToken cancellationToken)
	 where TDocument : Document {
		  ArgumentNullException.ThrowIfNull(document);
		  var partitionKey = DeterminePartitionKey(document);
		  var itemResponse = await _container.CreateItemAsync(document, partitionKey, null, cancellationToken);
		  return itemResponse.StatusCode.Equals(HttpStatusCode.OK);
	 }

	 /// <inheritdoc />
	 public async Task<bool> DeleteAsync<TDocument>(TDocument document, CancellationToken cancellationToken)
	 where TDocument : Document
		 => await DeleteAsync<TDocument>(document.Id, document.PartitionKey, cancellationToken);

	 /// <inheritdoc />
	 public async Task<bool> DeleteAsync<TDocument>(DocumentId documentId, string partitionKey, CancellationToken cancellationToken)
		 where TDocument : Document {
		  var response = await _container.DeleteItemAsync<TDocument>(documentId.Value, GetPartitionKey(partitionKey),
			  cancellationToken: cancellationToken);
		  return response.StatusCode.Equals(HttpStatusCode.OK);
	 }

	 /// <inheritdoc />
	 public async IAsyncEnumerable<TDocument> GetAsyncEnumerable<TDocument>(string partitionKey, Expression<Func<TDocument, bool>> whereClause = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		 where TDocument : Document {
		  var feedIterator = GetDocumentFeedIterator(new(partitionKey), whereClause);
		  while(feedIterator.HasMoreResults) {
				var feedResponse = await feedIterator.ReadNextAsync(cancellationToken);
				foreach(var document in feedResponse.Resource) {
					 yield return document;
				}
		  }
	 }

	 /// <inheritdoc />
	 public async Task<(IEnumerable<TDocument>, ContinuationToken)> GetPageAsync<TDocument>(string partitionKey, Expression<Func<TDocument, bool>> whereClause = null, int pageSize = 50, ContinuationToken continuationToken = null, CancellationToken cancellationToken = default)
		  where TDocument : Document {
		  var feedIterator = GetDocumentFeedIterator(GetPartitionKey(partitionKey), whereClause, pageSize, continuationToken);
		  var feedResponse = await feedIterator.ReadNextAsync(cancellationToken);
		  return (feedResponse.Resource, new(feedResponse.ContinuationToken));
	 }

	 private static PartitionKey DeterminePartitionKey(IReadOnlyCollection<Document> documents) =>
		 DeterminePartitionKey(documents.ToArray());

	 private static PartitionKey DeterminePartitionKey(params Document[] documents) {
		  ThrowIfMultiplePartitionKeysDetected(documents);
		  var value = documents.First().PartitionKey;
		  return GetPartitionKey(value);
	 }

	 private static PartitionKey GetPartitionKey(string value) {
		  ThrowIfPartitionKeyIsNullOrEmpty(value);
		  return new PartitionKey(value);
	 }

	 private static void ThrowIfMultiplePartitionKeysDetected(params Document[] documents) {
		  var partitionKeys = documents.Select(x => x.PartitionKey).Distinct();
		  if(partitionKeys.Count() > 1)
				throw new MultiplePartitionKeysDetected();
	 }

	 private static void ThrowIfPartitionKeyIsNullOrEmpty(string value) {
		  if(string.IsNullOrWhiteSpace(value))
				throw new ArgumentNullException(nameof(value));
	 }

	 private FeedIterator<TDocument> GetDocumentFeedIterator<TDocument>(PartitionKey partitionKey,
									Expression<Func<TDocument, bool>> whereClause = null,
		 int? pageSize = null,
		 ContinuationToken continuationToken = null) {
		  var queryable = _container.GetItemLinqQueryable<TDocument>(continuationToken: continuationToken?.Value,
					requestOptions: new() {
						 PartitionKey = partitionKey,
						 MaxItemCount = pageSize
					},
					linqSerializerOptions: new CosmosLinqSerializerOptions() {
						 PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
					});
		  return whereClause is not null
			  ? queryable.Where(whereClause).ToFeedIterator()
			  : queryable.ToFeedIterator();
	 }
}
