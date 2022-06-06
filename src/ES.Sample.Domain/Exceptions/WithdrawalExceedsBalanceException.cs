namespace ES.Sample.Domain.Exceptions;

/// <summary>WithdrawalExceedsBalanceException.</summary>
public class WithdrawalExceedsBalanceException : ApplicationException
{
	 /// <inheritdoc />
	 public WithdrawalExceedsBalanceException(decimal amountRequested) : base($"Your withdrawal request for {amountRequested:##.###} was declined because it exceeds the balance in your account") {
	 }
}
