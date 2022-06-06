namespace ES.Framework.Domain.Aggregates.Attributes;

/// <summary>Custom attribute that defines the layout of a PartitionKey for an Aggregate.</summary>
public class AggregatePartitionKeyAttribute : Attribute
{
	 /// <inheritdoc />
	 public AggregatePartitionKeyAttribute(string format) => Format = format;

	 /// <summary>Gets the format.</summary>
	 /// <value>The format.</value>
	 public string Format { get; private set; }

	 /// <summary>Gets the partition key.</summary>
	 /// <param name="arguments">The arguments.</param>
	 /// <returns>System.String.</returns>
	 public string GetPartitionKey(params object[] arguments) => string.Format(Format, arguments);
}
