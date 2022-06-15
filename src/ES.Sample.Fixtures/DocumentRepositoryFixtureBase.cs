using ES.Framework.Domain.Documents;
using ES.Framework.Domain.Repositories.Design;
using ES.Framework.Infrastructure.Cosmos;
using Microsoft.Azure.Cosmos;
using Moq;

namespace ES.Sample.Fixtures;

public abstract class DocumentRepositoryFixtureBase : FixtureBase
{
	 /// <summary>Gets the container mock.</summary>
	 /// <value>The container mock.</value>
	 public Mock<Container> ContainerMock { get; } = new() { CallBase = true };

	 public Mock<ICosmosQuery> QueryMock { get; } = new() {CallBase = true};

	 /// <summary>Gets the repository.</summary>
	 /// <value>The repository.</value>
	 public IDocumentRepository Repository { get; private set; }

	 protected abstract Task ActAsync(CancellationToken cancellationToken);

	 /// <inheritdoc />
	 protected override void Arrange() {
		  base.Arrange();
		  ArrangeContainer(ContainerMock);
		  ArrangeQueryMock(QueryMock);
		  Repository = CreateRepository();
	 }

	 protected virtual void ArrangeQueryMock(Mock<ICosmosQuery> mock) => mock
		 .Setup(x => x.GetFeedIterator<EventDocument>(It.IsAny<IQueryable<EventDocument>>())).Returns((IQueryable<EventDocument> q) => GetFeedIteratorMock(q));

	 protected virtual void ArrangeContainer(Mock<Container> mock) {
	 }
	 private FeedIterator<T> GetFeedIteratorMock<T>(IQueryable<T> list) {
		 var feedResponse = new Mock<FeedResponse<T>>();
		 var feedIterator = new Mock<FeedIterator<T>>();

		 feedResponse.Setup(x => x.Resource).Returns(list.ToList());
		 feedIterator.SetupGet(x => x.HasMoreResults).Returns(true);
		 feedIterator.Setup(x => x.ReadNextAsync(CancellationTokenSource.Token))
			 .Callback(() => feedIterator.SetupGet(x => x.HasMoreResults).Returns(false))
			 .ReturnsAsync(feedResponse.Object);
		 return feedIterator.Object;
	 }
	 protected abstract IDocumentRepository CreateRepository();

	 /// <inheritdoc />
	 protected override async Task InternalActAsync() => await ActAsync(CancellationTokenSource.Token);
}
