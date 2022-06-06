using ES.Framework.Domain.Documents;
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

	 /// <summary>Remove a <see cref="Document" /> from the document database asynchronously.</summary>
	 /// <typeparam name="TDocument">The type of document.</typeparam>
	 /// <param name="document">The document.</param>
	 /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	 /// <returns>True when successful, False when failed.</returns>
	 public Task<bool> DeleteAsync<TDocument>(TDocument document, CancellationToken cancellationToken)
		 where TDocument : Document;

	 /// <summary>Remove a <see cref="Document" /> from the document database asynchronously.</summary>
	 /// <typeparam name="TDocument">The type of document.</typeparam>
	 /// <param name="documentId">The document identifier.</param>
	 /// <param name="partitionKey">The partition key.</param>
	 /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	 /// <returns>True when successful, False when failed.</returns>
	 public Task<bool> DeleteAsync<TDocument>(DocumentId documentId, string partitionKey, CancellationToken cancellationToken)
		 where TDocument : Document;

	 /// <summary>
	 /// Gets an <see cref="IAsyncEnumerable{TDocument}"/> that retrieves documents from the document database asynchronously.
	 /// </summary>
	 /// <typeparam name="TDocument">The type of document.</typeparam>
	 /// <param name="partitionKey">The partition key.</param>
	 /// <param name="whereClause">The where clause.</param>
	 /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	 /// <returns>IAsyncEnumerable&lt;TDocument&gt;.</returns>
	 IAsyncEnumerable<TDocument> GetAsyncEnumerable<TDocument>(string partitionKey, Expression<Func<TDocument, bool>> whereClause = null, CancellationToken cancellationToken = default)
		 where TDocument : Document;

	 Task<(IEnumerable<TDocument>, ContinuationToken)> GetPageAsync<TDocument>(string partitionKey, Expression<Func<TDocument, bool>> whereClause = null, int pageSize = 50, ContinuationToken continuationToken = null, CancellationToken cancellationToken = default)
			  where TDocument : Document;
}
