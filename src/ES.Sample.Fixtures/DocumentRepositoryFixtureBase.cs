using ES.Framework.Domain.Repositories.Design;
using Microsoft.Azure.Cosmos;
using Moq;

namespace ES.Sample.Fixtures;

public abstract class DocumentRepositoryFixtureBase : FixtureBase
{
	 /// <summary>Gets the container mock.</summary>
	 /// <value>The container mock.</value>
	 public Mock<Container> ContainerMock { get; } = new() { CallBase = true };

	 /// <summary>Gets the repository.</summary>
	 /// <value>The repository.</value>
	 public IDocumentRepository Repository { get; private set; }

	 protected abstract Task ActAsync(CancellationToken cancellationToken);

	 /// <inheritdoc />
	 protected override void Arrange() {
		  base.Arrange();
		  ArrangeContainer(ContainerMock);
		  Repository = CreateRepository();
	 }

	 protected virtual void ArrangeContainer(Mock<Container> mock) {
	 }

	 protected abstract IDocumentRepository CreateRepository();

	 /// <inheritdoc />
	 protected override async Task InternalActAsync() => await ActAsync(CancellationTokenSource.Token);
}
