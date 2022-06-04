namespace ES.Sample.Domain.ValueObjects;

/// <summary>Class Withdrawal.</summary>
public record Withdrawal(decimal Amount, DateTime Timestamp);

/// <summary>Class WithdrawalLimit.</summary>
public record WithdrawalLimit(decimal Amount, TimeSpan TimeFrame);
