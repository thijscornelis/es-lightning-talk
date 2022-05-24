using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.TypedIdentifiers;
using ES.Framework.Domain.TypedIdentifiers.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Framework.Domain;
public class Snapshot<TKey, TState>
	 where TState : class, IAggregateState<TKey>, new()
	 where TKey : ITypedIdentifier
{
	public SnapshotId Id { get; init; }
	public TKey AggregateId { get; init; }
	public long Version { get; init; }
	public TState State { get; init; }
}

public record SnapshotId : TypedIdentifier<SnapshotId, Guid>
{
	/// <inheritdoc />
	public SnapshotId(Guid value) : base(value)
	{
	}
}