using ES.Framework.Domain.Repositories.Design;
using ES.Framework.Infrastructure.Cosmos;
using ES.Sample.Fixtures;

namespace ES.Framework.Tests.Infrastructure.CosmosDb.DocumentRepository;

public abstract class FixtureBase : DocumentRepositoryFixtureBase
{
	/// <inheritdoc />
	protected override IDocumentRepository CreateRepository() =>
		new CosmosDocumentRepository(ContainerMock.Object, QueryMock.Object);
}
