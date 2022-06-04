using ES.Sample.Domain.Events;

namespace ES.Sample.Domain.Aggregates;

public partial class BankAccount
{
	 /// <summary>Initializes a new instance of the <see cref="BankAccount" /> class.</summary>
	 protected BankAccount() {
		  Handle<BankAccountCreated>(OnCreated);
		  Handle<BankAccountNameChanged>(OnNameChanged);
	 }

	 private BankAccountState OnCreated(BankAccountCreated @event) => State with {
		  Id = @event.Id,
		  Name = @event.Name
	 };

	 private BankAccountState OnNameChanged(BankAccountNameChanged @event) => State with {
		  Id = @event.Id,
		  Name = @event.Name
	 };
}
