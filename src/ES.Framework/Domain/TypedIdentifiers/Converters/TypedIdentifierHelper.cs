using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace ES.Framework.Domain.TypedIdentifiers.Converters;

/// <summary>Helper class for <see cref="TypedIdentifier{TIdentity, TKey}" /></summary>
public static class TypedIdentifierHelper
{
	private static readonly ConcurrentDictionary<Type, Delegate> _typedIdentifierFactories = new();

	/// <summary>Gets the factory.</summary>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	/// <param name="typedIdentifierType">Type of the typed identifier.</param>
	/// <returns></returns>
	public static Func<TValue, object> GetFactory<TValue>(Type typedIdentifierType)
		where TValue : notnull => (Func<TValue, object>) _typedIdentifierFactories.GetOrAdd(
		typedIdentifierType,
		CreateFactory<TValue>);

	/// <summary>Determines whether [is typed identifier] [the specified type].</summary>
	/// <param name="type">The type.</param>
	/// <param name="valueType">Type of the actual value</param>
	/// <returns><c>true</c> if [is typed identifier] [the specified type]; otherwise, <c>false</c>.</returns>
	/// <exception cref="ArgumentNullException">nameof(type)</exception>
	public static bool IsTypedIdentifier(Type type, [NotNullWhen(true)] out Type valueType) {
		if(type is null)
			throw new ArgumentNullException(nameof(type));

		if(type.BaseType is Type baseType &&
		   baseType.IsGenericType &&
		   baseType.GetGenericTypeDefinition() == typeof(TypedIdentifier<,>)) {
			valueType = baseType.GetGenericArguments()[1];
			return true;
		}

		valueType = null;
		return false;
	}

	/// <summary>Determines whether [is typed identifier] [the specified type].</summary>
	/// <param name="type">The type.</param>
	/// <returns><c>true</c> if [is typed identifier] [the specified type]; otherwise, <c>false</c>.</returns>
	public static bool IsTypedIdentifier(Type type) => IsTypedIdentifier(type, out _);

	/// <summary>Creates the factory.</summary>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	/// <param name="typedIdentifierType">Type of the typed identifier.</param>
	/// <returns></returns>
	/// <exception cref="ArgumentException">
	///     $"Type '{typedIdentifierType}' is not a TypedIdentifier type,
	///     nameof(typedIdentifierType) or $"Type '{typedIdentifierType}' is not a TypedIdentifier type,
	///     nameof(typedIdentifierType)
	/// </exception>
	private static Func<TValue, object> CreateFactory<TValue>(Type typedIdentifierType) where TValue : notnull {
		if(!IsTypedIdentifier(typedIdentifierType))
			throw new ArgumentException($"Type '{typedIdentifierType}' is not a TypedIdentifier type",
				nameof(typedIdentifierType));

		var ctor = typedIdentifierType.GetConstructor(new[] {typeof(TValue)});
		if(ctor is null)
			throw new ArgumentException(
				$"Type '{typedIdentifierType}' doesn't have a constructor with one parameter of type '{typeof(TValue)}'",
				nameof(typedIdentifierType));

		var param = Expression.Parameter(typeof(TValue), "value");
		var body = Expression.New(ctor, param);
		var lambda = Expression.Lambda<Func<TValue, object>>(body, param);
		return lambda.Compile();
	}
}