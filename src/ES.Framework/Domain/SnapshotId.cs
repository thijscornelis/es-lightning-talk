using ES.Framework.Domain.TypedIdentifiers;

namespace ES.Framework.Domain;

public record SnapshotId : TypedIdentifier<SnapshotId, Guid>
{
	/// <inheritdoc />
	public SnapshotId(Guid value) : base(value) { }
}