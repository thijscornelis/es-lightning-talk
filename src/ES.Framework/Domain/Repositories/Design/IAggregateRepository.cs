using ES.Framework.Domain.Aggregates;
using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.TypedIdentifiers.Design;

namespace ES.Framework.Domain.Repositories.Design;

public interface IAggregateRepository<TAggregate, TKey, TState, TValue>
	 where TAggregate : Aggregate<TKey, TState>
	 where TKey : ITypedIdentifier<TValue>
	 where TState : class, IAggregateState<TKey>, new()
{
	 public Task<TAggregate> FindAsync(TKey id, CancellationToken cancellationToken);

	 public Task<TAggregate> GetAsync(TKey id, CancellationToken cancellationToken);

	 public IAsyncEnumerable<TAggregate> GetEnumerableAsync(CancellationToken cancellationToken);

	 public Task<TAggregate> SaveAsync(TAggregate aggregate, CancellationToken cancellationToken);
}
