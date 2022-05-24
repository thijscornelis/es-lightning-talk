namespace ES.Framework.Domain.Repositories.Design;

public interface IDocumentRepository
{
	public Task AddAsync<TDocument>(IReadOnlyCollection<TDocument> documents, CancellationToken cancellationToken)
		where TDocument : Document;
	public Task AddAsync<TDocument>(TDocument document, CancellationToken cancellationToken)
		where TDocument : Document;

	
}