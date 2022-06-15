using ES.Framework.Domain.Abstractions;
using ES.Framework.Domain.Documents.Design;
using ES.Framework.Domain.Projections;

namespace ES.Framework.Domain.Documents;

/// <inheritdoc />
public class ProjectionDocumentConverter : IProjectionDocumentConverter
{
	 private readonly IDateTimeProvider _dateTimeProvider;
	 private readonly IProjectionPartitionKeyResolver _partitionKeyResolver;

	 /// <summary>Initializes a new instance of the <see cref="ProjectionDocumentConverter" /> class.</summary>
	 /// <param name="dateTimeProvider">The date time provider.</param>
	 /// <param name="partitionKeyResolver">The partition key resolver.</param>
	 public ProjectionDocumentConverter(IDateTimeProvider dateTimeProvider, IProjectionPartitionKeyResolver partitionKeyResolver) {
		  _dateTimeProvider = dateTimeProvider;
		  _partitionKeyResolver = partitionKeyResolver;
	 }

	 /// <inheritdoc />
	 public ProjectionDocument<TProjection> ToDocument<TProjection>(ProjectionId id, TProjection projection) => new() {
		  Id = id,
		  PartitionKey = _partitionKeyResolver.CreatePartitionKey<TProjection>(),
		  Payload = projection,
		  PayloadType = projection.GetType().AssemblyQualifiedName,
		  Timestamp = _dateTimeProvider.Now()
	 };
}
