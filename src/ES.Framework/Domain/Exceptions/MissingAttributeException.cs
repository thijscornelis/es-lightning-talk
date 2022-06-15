namespace ES.Framework.Domain.Exceptions;

/// <summary>Class MissingAttributeException.</summary>
public class MissingAttributeException : Exception
{
	 /// <inheritdoc />
	 public MissingAttributeException(Type attributeType, Type classType) : base(string.Format("Attribute {0} was expected but not found on class {1}", attributeType.Name, classType.Name)) {
	 }
}

/// <summary>Class EventTypeNotFoundException.</summary>
public class EventTypeNotFoundException : Exception
{
	 /// <inheritdoc />
	 public EventTypeNotFoundException(string message) : base(message) {
	 }
}
