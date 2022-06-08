using ES.Framework.Domain.Projections;
using System.Linq.Expressions;

namespace ES.Framework.Domain.Repositories.Design;

/// <summary>Create/Read/Update/Delete actions for projections</summary>
public interface IProjectionRepository
{
	 /// <summary>Deletes the projection asynchronous.</summary>
	 /// <param name="projectionId">The projection identifier.</param>
	 /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	 /// <returns>Task.</returns>
	 Task<bool> DeleteAsync<TProjection>(ProjectionId projectionId, CancellationToken cancellationToken);

	 /// <summary>Finds the specified projection by identifier.</summary>
	 /// <param name="projectionId">The projection identifier.</param>
	 /// <returns>TProjection. Null if not found</returns>
	 TProjection Find<TProjection>(ProjectionId projectionId)
		  where TProjection : new();

	 /// <summary>Gets the specified projections by partition key using a where clause.</summary>
	 /// <param name="whereClause">The where clause.</param>
	 /// <returns>IQueryable&lt;TProjection&gt;.</returns>
	 IQueryable<TProjection> Get<TProjection>(Expression<Func<TProjection, bool>> whereClause)
		 where TProjection : new();

	 /// <summary>Saves the projection asynchronous.</summary>
	 /// <param name="projectionId">The projection identifier.</param>
	 /// <param name="projection">The projection.</param>
	 /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
	 /// <returns>Task&lt;TProjection&gt;.</returns>
	 Task<bool> SaveAsync<TProjection>(ProjectionId projectionId, TProjection projection, CancellationToken cancellationToken)
		 where TProjection : new();
}
