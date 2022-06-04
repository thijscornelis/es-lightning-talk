using ES.Framework.Domain.Aggregates;
using ES.Framework.Domain.Aggregates.Design;
using ES.Framework.Domain.TypedIdentifiers.Design;

namespace ES.Framework.Domain.Repositories.Design;

/// <summary>Repository for <see cref="Aggregate{TKey,TState}" /></summary>
/// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TState">The type of the state.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public interface IAggregateRepository<TAggregate, in TKey, TState, TValue>
	 where TAggregate : Aggregate<TKey, TState>
	 where TKey : ITypedIdentifier<TValue>
	 where TState : class, IAggregateState<TKey>, new()
{
	 /// <summary>Finds an aggregate asynchronously.</summary>
	 /// <param name="id">The identifier.</param>
	 /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	 /// <returns>Task&lt;TAggregate&gt;.</returns>
	 public Task<TAggregate> FindAsync(TKey id, CancellationToken cancellationToken);

	 /// <summary>Gets an aggregate asynchronously.</summary>
	 /// <param name="id">The identifier.</param>
	 /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	 /// <returns>Task&lt;TAggregate&gt;.</returns>
	 public Task<TAggregate> GetAsync(TKey id, CancellationToken cancellationToken);

	 /// <summary>Gets an enumerable of aggregates asynchronously.</summary>
	 /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	 /// <returns>IAsyncEnumerable&lt;TAggregate&gt;.</returns>
	 public IAsyncEnumerable<TAggregate> GetEnumerableAsync(CancellationToken cancellationToken);

	 /// <summary>Saves the aggregate asynchronously.</summary>
	 /// <param name="aggregate">The aggregate.</param>
	 /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	 /// <returns>Task&lt;TAggregate&gt;.</returns>
	 public Task<TAggregate> SaveAsync(TAggregate aggregate, CancellationToken cancellationToken);
}
