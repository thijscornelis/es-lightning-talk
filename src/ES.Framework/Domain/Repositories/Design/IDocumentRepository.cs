using ES.Framework.Domain.Documents;
using ES.Framework.Domain.TypedIdentifiers.Design;
using System.Linq.Expressions;

namespace ES.Framework.Domain.Repositories.Design;

/// <summary>Design document database functionality</summary>
public interface IDocumentRepository
{
	 /// <summary>Add a collection of <see cref="Document" /> to the document database asynchronously.</summary>
	 /// <typeparam name="TDocument">The type of document.</typeparam>
	 /// <param name="documents">The documents.</param>
	 /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	 /// <returns>True when successful, False when failed.</returns>
	 public Task<bool> AddAsync<TDocument>(IReadOnlyCollection<TDocument> documents, CancellationToken cancellationToken)
		 where TDocument : Document;

	 /// <summary>Add a <see cref="Document" /> to the document database asynchronously.</summary>
	 /// <typeparam name="TDocument">The type of document.</typeparam>
	 /// <param name="document">The document.</param>
	 /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	 /// <returns>True when successful, False when failed.</returns>
	 public Task<bool> AddAsync<TDocument>(TDocument document, CancellationToken cancellationToken)
		 where TDocument : Document;

	 /// <summary>Adds the or update the document asynchronous.</summary>
	 /// <typeparam name="TDocument">The type of the document.</typeparam>
	 /// <param name="document">The document.</param>
	 /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	 /// <returns>Task&lt;System.Boolean&gt;.</returns>
	 public Task<bool> AddOrUpdateAsync<TDocument>(TDocument document, CancellationToken cancellationToken)
		 where TDocument : Document;

	 /// <summary>Remove a <see cref="Document" /> from the document database asynchronously.</summary>
	 /// <typeparam name="TDocument">The type of document.</typeparam>
	 /// <typeparam name="TKey">The type of the t key.</typeparam>
	 /// <param name="documentId">The document identifier.</param>
	 /// <param name="partitionKey">The partition key.</param>
	 /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	 /// <returns>True when successful, False when failed.</returns>
	 public Task<bool> DeleteAsync<TDocument, TKey>(TKey documentId, string partitionKey, CancellationToken cancellationToken)
		 where TKey : ITypedIdentifier
		 where TDocument : Document;

	 /// <summary>Gets an <see cref="IAsyncEnumerable{TDocument}" /> that retrieves documents from the document database asynchronously.</summary>
	 /// <typeparam name="TDocument">The type of document.</typeparam>
	 /// <param name="partitionKey">The partition key.</param>
	 /// <param name="whereClause">The where clause.</param>
	 /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	 /// <returns>IAsyncEnumerable&lt;TDocument&gt;.</returns>
	 IAsyncEnumerable<TDocument> GetAsyncEnumerable<TDocument>(string partitionKey, Expression<Func<TDocument, bool>> whereClause = null, CancellationToken cancellationToken = default)
		 where TDocument : Document;

	 /// <summary>Gets a page of events asynchronously.</summary>
	 /// <typeparam name="TDocument">The type of document.</typeparam>
	 /// <param name="partitionKey">The partition key.</param>
	 /// <param name="whereClause">The where clause.</param>
	 /// <param name="pageSize">Size of the page.</param>
	 /// <param name="continuationToken">The continuation token.</param>
	 /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	 /// <returns>Task&lt;System.ValueTuple&lt;IEnumerable&lt;TDocument&gt;, ContinuationToken&gt;&gt;.</returns>
	 Task<(IEnumerable<TDocument>, ContinuationToken)> GetPageAsync<TDocument>(string partitionKey, Expression<Func<TDocument, bool>> whereClause = null, int pageSize = 50, ContinuationToken continuationToken = null, CancellationToken cancellationToken = default)
			  where TDocument : Document;

	 /// <summary>Gets the documents as queryable.</summary>
	 /// <typeparam name="TDocument">The type of the document.</typeparam>
	 /// <param name="partitionKey">The partition key.</param>
	 /// <param name="whereClause">The where clause.</param>
	 /// <param name="pageSize">Size of the page.</param>
	 /// <param name="continuationToken">The continuation token.</param>
	 /// <returns>IQueryable&lt;TDocument&gt;.</returns>
	 IQueryable<TDocument> GetQueryable<TDocument>(string partitionKey,
		 Expression<Func<TDocument, bool>> whereClause = null, int pageSize = 50, ContinuationToken continuationToken = null)
		 where TDocument : Document;
}
