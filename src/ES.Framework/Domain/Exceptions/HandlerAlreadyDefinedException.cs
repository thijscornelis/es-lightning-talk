namespace ES.Framework.Domain.Exceptions;

[Serializable]
public class HandlerAlreadyDefinedException : ApplicationException
{
	 /// <summary>Initializes a new instance of the <see cref="HandlerAlreadyDefinedException" /> class.</summary>
	 /// <param name="type">The type.</param>
	 public HandlerAlreadyDefinedException(Type type) : base($"Handler for type '{type.Name}' has already been defined!") {
	 }
}
