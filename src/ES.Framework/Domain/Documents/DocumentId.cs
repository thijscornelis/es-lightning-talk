using ES.Framework.Domain.TypedIdentifiers;

namespace ES.Framework.Domain.Documents;

/// <summary>Typed Identifier for a <see cref="Document" />.</summary>
public record DocumentId : TypedIdentifier<DocumentId, Guid>
{
	 /// <inheritdoc />
	 public DocumentId(Guid value) : base(value) { }
}
