using ES.Framework.Domain.Abstractions;
using ES.Framework.Domain.Documents;
using ES.Framework.Domain.Documents.Design;
using ES.Framework.Domain.Projections;
using ES.Framework.Domain.Repositories.Design;
using System.Linq.Expressions;

namespace ES.Framework.Domain.Repositories;

/// <inheritdoc />
public class ProjectionRepository : IProjectionRepository
{
	 private readonly IProjectionDocumentConverter _documentConverter;
	 private readonly IDocumentRepository _documentRepository;
	 private readonly IProjectionPartitionKeyResolver _partitionKeyResolver;

	 /// <summary>Initializes a new instance of the <see cref="ProjectionRepository{TProjection}" /> class.</summary>
	 /// <param name="partitionKeyResolver">The partition key resolver.</param>
	 /// <param name="documentRepository">The document repository.</param>
	 /// <param name="documentConverter">The document converter.</param>
	 public ProjectionRepository(IProjectionPartitionKeyResolver partitionKeyResolver, IDocumentRepository documentRepository, IProjectionDocumentConverter documentConverter) {
		  _partitionKeyResolver = partitionKeyResolver;
		  _documentRepository = documentRepository;
		  _documentConverter = documentConverter;
	 }

	 /// <inheritdoc />
	 public async Task<bool> DeleteAsync<TProjection>(ProjectionId projectionId, CancellationToken cancellationToken) {
		  var partitionKey = _partitionKeyResolver.CreatePartitionKey<TProjection>();
		  return await _documentRepository.DeleteAsync<ProjectionDocument<TProjection>, ProjectionId>(projectionId, partitionKey, cancellationToken);
	 }

	 /// <inheritdoc />
	 public TProjection Find<TProjection>(ProjectionId projectionId)
	 where TProjection : new() => GetDocumentQueryable<TProjection>(x => x.Id == projectionId).Select(x => x.Payload).AsEnumerable().SingleOrDefault();

	 /// <inheritdoc />
	 public IQueryable<TProjection> Get<TProjection>(Expression<Func<TProjection, bool>> whereClause)
		 where TProjection : new() => GetDocumentQueryable<TProjection>().Select(x => x.Payload).Where(whereClause);

	 /// <inheritdoc />
	 public async Task<bool> SaveAsync<TProjection>(ProjectionId projectionId, TProjection projection, CancellationToken cancellationToken)
	 where TProjection : new() {
		  var document = _documentConverter.ToDocument(projectionId, projection);
		  return await _documentRepository.AddOrUpdateAsync(document, cancellationToken);
	 }

	 private IQueryable<ProjectionDocument<TProjection>> GetDocumentQueryable<TProjection>(Expression<Func<ProjectionDocument<TProjection>, bool>> whereClause = null) {
		  var partitionKey = _partitionKeyResolver.CreatePartitionKey<TProjection>();
		  var queryable = _documentRepository.GetQueryable<ProjectionDocument<TProjection>>(partitionKey);
		  return whereClause == null ? queryable : queryable.Where(whereClause);
	 }
}
