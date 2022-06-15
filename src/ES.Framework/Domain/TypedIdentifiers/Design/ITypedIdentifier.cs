namespace ES.Framework.Domain.TypedIdentifiers.Design;

/// <summary>Contract for typed identifiers</summary>
public interface ITypedIdentifier
{
	 /// <summary>Gets a string representation of the actual value</summary>
	 public string Value { get; }
}

/// <summary>Contract for typed identifiers</summary>
/// <typeparam name="TValue">Actual identifier type</typeparam>
public interface ITypedIdentifier<out TValue> : ITypedIdentifier
{
	 /// <summary>Gets the actual value</summary>
	 public TValue TypedValue { get; }
}
