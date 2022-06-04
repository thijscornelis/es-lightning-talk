namespace ES.Sample.Domain.Exceptions;

/// <summary>WithdrawalExceedsLimitException.</summary>
public class WithdrawalExceedsLimitException : ApplicationException
{
	 /// <inheritdoc />
	 public WithdrawalExceedsLimitException(decimal amountRequested, decimal limit, TimeSpan cooldown) : base($"Your withdrawal request for {amountRequested:##.###} was declined because the limit of {limit:##.###} during {cooldown:g} was exceeded.") {
	 }
}
