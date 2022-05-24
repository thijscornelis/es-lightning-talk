using ES.Framework.Domain.TypedIdentifiers.Converters;
using ES.Framework.Domain.TypedIdentifiers.Design;
using ES.Framework.Infrastructure.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace ES.Framework.Domain.TypedIdentifiers;

[TypeConverter(typeof(TypedIdentifierTypeConverter))]
[DebuggerDisplay("{Value}")]
public abstract record TypedIdentifier<TTypedIdentifier> : ITypedIdentifier
	where TTypedIdentifier : ITypedIdentifier
{
	 /// <summary>Initializes a new instance of the <see cref="TypedIdentifier{TTypedIdentifier}" /> class.</summary>
	 protected TypedIdentifier() { }

	 /// <summary>Initializes a new instance of the <see cref="TypedIdentifier{TTypedIdentifier}" /> class.</summary>
	 protected TypedIdentifier(string value) => Value = value;

	 /// <inheritdoc />
	 public string Value { get; }

	 /// <inheritdoc />
	 public override int GetHashCode() => Value.GetHashCode();

	 /// <inheritdoc />
	 public override string ToString() => Value;

	 /// <summary>
	 ///     Compares one <typeparamref name="TTypedIdentifier" /> with another <typeparamref name="TTypedIdentifier" />
	 /// </summary>
	 /// <param name="other">The other <typeparamref name="TTypedIdentifier" /></param>
	 /// <returns>Integer with the comparison result</returns>
	 public int CompareTo(TTypedIdentifier other) => string.Compare(Value, other.Value, StringComparison.OrdinalIgnoreCase);

	 /// <summary>Equality checks the specified other.</summary>
	 /// <param name="other">The other.</param>
	 /// <returns></returns>
	 public bool Equals(TTypedIdentifier other) => Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
}

/// <summary>Base record for typed identifiers</summary>
[DebuggerDisplay("{Value}")]
public abstract record TypedIdentifier<TTypedIdentifier, TValue> : TypedIdentifier<TTypedIdentifier>, ITypedIdentifier<TValue>
	where TTypedIdentifier : ITypedIdentifier<TValue>
	where TValue : notnull

{
	 /// <summary>Initializes a new instance of the <see cref="TypedIdentifier{TTypedIdentifier, TValue}" /> class.</summary>
	 /// <param name="value">The value.</param>
	 protected TypedIdentifier(TValue value) : base(value.ToString()) => TypedValue = value;

	 /// <inheritdoc />
	 public TValue TypedValue { get; private set; }

	 /// <summary>Initializes a new instance of the <see cref="TypedIdentifier{TTypedIdentifier, TValue}" /> class with a specified <typeparamref name="TValue" /> value</summary>
	 public static TTypedIdentifier CreateNew<TValue>(TValue value) {
		  try {
				return (TTypedIdentifier) Activator.CreateInstance(typeof(TTypedIdentifier), value);
		  }
		  catch(TargetInvocationException e) {
				if(e.InnerException != null) {
					 throw e.InnerException;
				}
				throw;
		  }
	 }

	 /// <summary>Initializes a new instance of the <see cref="TypedIdentifier{TTypedIdentifier, TValue}" /> class.</summary>
	 public static TTypedIdentifier CreateNew() => typeof(Guid) == typeof(TValue)
		 ? CreateNew<Guid>(Guid.NewGuid())
		 : CreateNew(Activator.CreateInstance<TValue>())
	 ;
	 
}