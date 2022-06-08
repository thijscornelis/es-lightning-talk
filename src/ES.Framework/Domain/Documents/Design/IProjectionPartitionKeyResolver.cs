namespace ES.Framework.Domain.Abstractions;

/// <summary>Resolve PartitionKeys for projections</summary>
public interface IProjectionPartitionKeyResolver
{
	 /// <summary>Creates the projection partition key.</summary>
	 /// <typeparam name="TProjection">The type of the projection.</typeparam>
	 /// <returns>System.String.</returns>
	 public string CreatePartitionKey<TProjection>();
}
