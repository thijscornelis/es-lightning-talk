using ES.Framework.Domain.Aggregates;
using ES.Framework.Domain.Aggregates.Attributes;

namespace ES.Sample.Domain
{
	 /// <summary>The <see cref="BankAccount" /> aggregate root.</summary>
	 [AggregatePartitionKey("{0}-{1:N}")]
	 public class BankAccount : Aggregate<BankAccountId, BankAccountState>
	 {
		  /// <summary>Initializes a new instance of the <see cref="BankAccount" /> class.</summary>
		  /// <param name="name">The name.</param>
		  public BankAccount(string name) : this(BankAccountId.CreateNew(), name) {
		  }

		  /// <summary>Initializes a new instance of the <see cref="BankAccount" /> class.</summary>
		  protected BankAccount() {
				Handle<BankAccountCreated>(OnCreated);
				Handle<BankAccountNameChanged>(OnNameChanged);
		  }

		  /// <summary>Initializes a new instance of the <see cref="BankAccount" /> class.</summary>
		  /// <param name="id">The identifier.</param>
		  /// <param name="name">The name.</param>
		  protected BankAccount(BankAccountId id, string name) : this() {
				ThrowIfInvalidName(name);
				Apply(new BankAccountCreated() {
					 Id = id,
					 Name = name
				});
		  }

		  /// <summary>Sets the name.</summary>
		  /// <param name="value">The value.</param>
		  /// <exception cref="System.ArgumentNullException">value</exception>
		  public void SetName(string value) {
				ThrowIfInvalidName(value);
				Apply(new BankAccountNameChanged() {
					 Id = Id, Name = value
				});
		  }

		  private static void ThrowIfInvalidName(string value) {
				if(string.IsNullOrWhiteSpace(value))
					 throw new ArgumentNullException(nameof(value));
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
}
