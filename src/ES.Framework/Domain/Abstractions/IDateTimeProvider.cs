namespace ES.Framework.Domain.Abstractions;

/// <summary>Make an abstraction of how we determine what is "Now" in the application. Force usage of UniversalTime on everyone.</summary>
public interface IDateTimeProvider
{
	 /// <summary>Get a <see cref="DateTime" /> instance representing now.</summary>
	 /// <returns>DateTime.</returns>
	 DateTime Now();

	 /// <summary>Get a <see cref="TimeOnly" /> instance representing now.</summary>
	 /// <returns>TimeOnly.</returns>
	 TimeOnly Time();

	 /// <summary>Get a <see cref="DateOnly" /> instance representing today.</summary>
	 /// <returns>DateOnly.</returns>
	 DateOnly Today();
}
