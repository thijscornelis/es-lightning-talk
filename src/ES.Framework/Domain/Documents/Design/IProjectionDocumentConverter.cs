using ES.Framework.Domain.Projections;

namespace ES.Framework.Domain.Documents.Design;

/// <summary>Interface IProjectionDocumentConverter</summary>
public interface IProjectionDocumentConverter
{
	 /// <summary>Converts to document.</summary>
	 /// <typeparam name="TProjection">The type of the projection.</typeparam>
	 /// <param name="id">The identifier.</param>
	 /// <param name="projection">The projection.</param>
	 /// <returns>ProjectionDocument&lt;TProjection&gt;.</returns>
	 public ProjectionDocument<TProjection> ToDocument<TProjection>(ProjectionId id, TProjection projection);
}
