using ES.Framework.Domain.Aggregates;
using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.Collections;
using ES.Framework.Domain.TypedIdentifiers.Design;

namespace ES.Framework.Domain.Repositories.Design;

public interface IAggregateRepository<TAggregate, TKey, TState>
	 where TAggregate : Aggregate<TKey, TState> 
	 where TKey : ITypedIdentifier 
	 where TState : class, IAggregateState<TKey>, new()
{
	public Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken);
	public Task DeleteAsync(TKey id, CancellationToken cancellationToken);
	public Task<TAggregate> SaveAsync(TAggregate aggregate, CancellationToken cancellationToken);
	public Task<TAggregate> FindAsync(TKey id, CancellationToken cancellationToken);
	public Task<TAggregate> GetAsync(TKey id, CancellationToken cancellationToken);
	public IAsyncEnumerable<TAggregate> GetEnumerableAsync(CancellationToken cancellationToken);
}