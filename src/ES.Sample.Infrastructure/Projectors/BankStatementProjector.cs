using ES.Framework.Domain.Events.Design;
using ES.Framework.Domain.Projections.Design;
using ES.Sample.Domain.Events;
using ES.Sample.Domain.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Sample.Infrastructure.Projectors;
public class BankStatementProjector : Projector<BankStatementProjection>
{
	 /// <inheritdoc />
	 public BankStatementProjector() {
		  Handle<MoneyWithdrawn>(OnWithdraw);
		  Handle<MoneyDeposited>(OnDeposit);
	 }

	 private BankStatementProjection OnDeposit(MoneyDeposited @event, BankStatementProjection projection) =>
		 projection with {Amount = @event.Amount, On = @event.OccurredOn, BankAccountId = @event.Id};

	 private BankStatementProjection OnWithdraw(MoneyWithdrawn @event, BankStatementProjection projection) => 
		 projection with { Amount = @event.Amount * -1, On = @event.OccurredOn, BankAccountId = @event.Id };
}


public class BankAccountProjector : Projector<BankAccountProjection>
{
	 /// <inheritdoc />
	 public BankAccountProjector() {
		  Handle<BankAccountCreated>(OnCreated);
		  Handle<BankAccountNameChanged>(OnNameChanged);
		  Handle<MoneyWithdrawn>(OnWithdraw);
		  Handle<MoneyDeposited>(OnDeposit);
	 }

	 private BankAccountProjection OnDeposit(MoneyDeposited @event, BankAccountProjection projection) =>
		 projection with {Balance = projection.Balance + @event.Amount, NumberOfDeposits = projection.NumberOfDeposits + 1 };

	 private BankAccountProjection OnWithdraw(MoneyWithdrawn @event, BankAccountProjection projection) =>
		 projection with {Balance = projection.Balance - @event.Amount, NumberOfWithdrawals = projection.NumberOfWithdrawals + 1};

	 private BankAccountProjection OnNameChanged(BankAccountNameChanged @event, BankAccountProjection projection) =>
		 projection with {Name = @event.Name};

	 private BankAccountProjection OnCreated(BankAccountCreated @event, BankAccountProjection projection) => projection
		 with {
			 Balance = 0,
			 Name = @event.Name,
			 NumberOfDeposits = 0,
			 NumberOfWithdrawals = 0,
			 BankAccountId = @event.Id
		 };
}

public abstract class Projector<TProjection> : IProjector
{
	 private readonly Dictionary<Type, EventHandlerFunc<IEvent, TProjection>> _handlers = new();

	 /// <summary>Delegate func for an event handler.</summary>
	 /// <typeparam name="TEvent">The type of the event.</typeparam>
	 /// <typeparam name="TProjection">The type of the projection.</typeparam>
	 /// <param name="event">The event.</param>
	 /// <param name="projection">The projection.</param>
	 protected delegate TProjection EventHandlerFunc<TEvent, TProjection>(TEvent @event, TProjection projection)
		 where TEvent : IEvent;
	 /// <summary>Registration method for the specified handler.</summary>
	 /// <typeparam name="TEvent">The type of the event.</typeparam>
	 /// <param name="handler">The handler.</param>
	 /// <param name="identifierProvider">A method that provides the correct <see cref="ProjectionId" /> based on data in the event</param>
	 protected void Handle<TEvent>(EventHandlerFunc<TEvent, TProjection> handler) where TEvent : IEvent {
		  if(handler == null)
				throw new ArgumentNullException(nameof(handler), @"Projection can not handle event without handler");

		  _handlers.Add(typeof(TEvent), (IEvent @event, TProjection projection) => handler((TEvent) @event, projection));
	 }

	 /// <inheritdoc />
	 public bool CanHandle<TEvent>(TEvent @event) where TEvent : IEvent => throw new NotImplementedException();

	 /// <inheritdoc />
	 public Task Handle<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : IEvent => throw new NotImplementedException();
}