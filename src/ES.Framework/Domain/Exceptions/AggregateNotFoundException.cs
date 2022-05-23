using ES.Framework.Domain.TypedIdentifiers.Design;

namespace ES.Framework.Domain.Exceptions;

[Serializable]
public class AggregateNotFoundException : ApplicationException
{
	/// <summary>Initializes a new instance of the <see cref="AggregateNotFoundException" /> class.</summary>
	/// <param name="id">The identifier.</param>
	/// <param name="message">The message.</param>
	public AggregateNotFoundException(ITypedIdentifier id, string message = null) : base($"Aggregate could not be found! {message ?? ""}".Trim()) => AggregateId = id;

	/// <summary>Gets or sets the aggregate identifier.</summary>
	/// <value>The aggregate identifier.</value>
	public ITypedIdentifier AggregateId { get; init; }
}