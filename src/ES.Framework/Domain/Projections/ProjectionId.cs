using ES.Framework.Domain.TypedIdentifiers;

namespace ES.Framework.Domain.Projections;
public record ProjectionId : TypedIdentifier<ProjectionId, string>
{
	 /// <inheritdoc />
	 public ProjectionId(string value) : base(value) {
	 }
}
