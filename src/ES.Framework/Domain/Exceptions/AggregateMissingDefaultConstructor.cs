namespace ES.Framework.Domain.Exceptions;

[Serializable]
public class AggregateMissingDefaultConstructor : ApplicationException
{
	public AggregateMissingDefaultConstructor(Type type) : base($"Aggregate {type.Name} needs to declare a default constructor") {
		
	}
}