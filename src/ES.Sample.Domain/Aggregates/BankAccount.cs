using ES.Framework.Domain.Aggregates;
using ES.Framework.Domain.Aggregates.Attributes;
using ES.Sample.Domain.Events;

namespace ES.Sample.Domain.Aggregates;

/// <summary>The <see cref="BankAccount" /> aggregate root.</summary>
[AggregatePartitionKey("{0}-{1:N}")]
public partial class BankAccount : Aggregate<BankAccountId, BankAccountState>
{
	 /// <summary>Initializes a new instance of the <see cref="BankAccount" /> class.</summary>
	 /// <param name="name">The name.</param>
	 /// <param name="withdrawalLimit">The withdrawal limit.</param>
	 public BankAccount(string name, decimal withdrawalLimit) : this(BankAccountId.CreateNew(), name, withdrawalLimit) {
	 }

	 /// <summary>Initializes a new instance of the <see cref="BankAccount" /> class.</summary>
	 /// <param name="id">The identifier.</param>
	 /// <param name="name">The name.</param>
	 /// <param name="withdrawalLimit">The withdrawal limit.</param>
	 protected BankAccount(BankAccountId id, string name, decimal withdrawalLimit) : this() {
		  ThrowIfInvalidName(name);
		  Apply(new BankAccountCreated(id) {
				Name = name,
				WithdrawalLimit = withdrawalLimit
		  });
	 }

	 /// <summary>Sets the name.</summary>
	 /// <param name="value">The value.</param>
	 /// <exception cref="ArgumentNullException">value</exception>
	 public void SetName(string value) {
		  ThrowIfInvalidName(value);
		  Apply(new BankAccountNameChanged(Id) {
				Name = value
		  });
	 }

	 private static void ThrowIfInvalidName(string value) {
		  if(string.IsNullOrWhiteSpace(value))
				throw new ArgumentNullException(nameof(value));
	 }
}
