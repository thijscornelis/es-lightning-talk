using Microsoft.Azure.Cosmos;

namespace ES.Framework.Infrastructure.Cosmos.Exceptions;

/// <summary>Thrown when <see cref="CosmosSerializer" /> or derived serializer cannot serialize/deserialize an object Implements the <see cref="System.ApplicationException" /></summary>
/// <seealso cref="System.ApplicationException" />
public class CosmosSerializerException : ApplicationException
{
	 /// <summary>Initializes a new instance of the <see cref="CosmosSerializerException" /> class.</summary>
	 /// <param name="message">A message that describes the error.</param>
	 public CosmosSerializerException(string message) : base(message) {
	 }
}

public class MultiplePartitionKeysDetected : ApplicationException
{
	 /// <inheritdoc />
	 public MultiplePartitionKeysDetected() : base("CosmosDB can only create a transactional batch for a single Partition Key at a time") {
	 }
}
