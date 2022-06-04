using ES.Framework.Domain.Abstractions;

namespace ES.Framework.Infrastructure.Dates;

/// <summary>UniversalDateTimeProvider</summary>
public sealed class UniversalDateTimeProvider : IDateTimeProvider
{
	 /// <inheritdoc />
	 public DateTime Now() => DateTime.UtcNow;

	 /// <inheritdoc />
	 public TimeOnly Time() => TimeOnly.FromDateTime(Now());

	 /// <inheritdoc />
	 public DateOnly Today() => DateOnly.FromDateTime(Now());
}
