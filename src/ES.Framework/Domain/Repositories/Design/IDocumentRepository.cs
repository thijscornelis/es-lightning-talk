using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ES.Framework.Domain.Repositories.Design;

public interface IDocumentRepository
{
	 public Task AddAsync<TDocument>(IReadOnlyCollection<TDocument> documents, CancellationToken cancellationToken)
		 where TDocument : Document;
	 public Task AddAsync<TDocument>(TDocument document, CancellationToken cancellationToken)
		 where TDocument : Document;


	 Task<(IEnumerable<TDocument>, ContinuationToken)> GetPagedDocumentsAsync<TDocument>(string partitionKey, Expression<Func<TDocument, bool>> whereClause = null, int pageSize = 50, ContinuationToken continuationToken = null, CancellationToken cancellationToken = default)
		 where TDocument : Document;

	 IAsyncEnumerable<TDocument> GetDocumentsAsync<TDocument>(string partitionKey, Expression<Func<TDocument, bool>> whereClause = null, CancellationToken cancellationToken = default)
		 where TDocument : Document;
}