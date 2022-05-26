﻿using ES.Framework.Domain.Repositories;
using FluentAssertions;
using Microsoft.Azure.Cosmos;
using Moq;
using System.Net;

namespace ES.Framework.Tests.Infrastructure.CosmosDb.DocumentRepository;

public class WhenAddingSingleDocument : IClassFixture<WhenAddingSingleDocument.Fixture>
{
	 private readonly Fixture _fixture;

	 public WhenAddingSingleDocument(Fixture fixture) => _fixture = fixture;

	 [Fact]
	 public void ItShouldCallCreateItemAsync() => _fixture.ContainerMock.Verify(
		 x => x.CreateItemAsync(_fixture.EventDocument, new PartitionKey(_fixture.EventDocument.PartitionKey),
			 It.IsAny<ItemRequestOptions>(), _fixture.CancellationTokenSource.Token), Times.Once);

	 [Fact]
	 public void ItShouldReturnTrue() => _fixture.Result.Should().BeTrue();

	 [Fact]
	 public void ItShouldSucceed() => _fixture.HasExecutedSuccessfully.Should().BeTrue();

	 public class Fixture : FixtureBase
	 {
		  public EventDocument EventDocument { get; } = new() {
				Id = DocumentId.CreateNew(Guid.Parse("{A2966192-C1F9-4E2F-899F-84C4FB3E2075}")),
				PartitionKey = "UNIT_TEST_PARTITION_KEY",
				Version = 1,
				Timestamp = DateTime.UtcNow
		  };

		  public bool Result { get; private set; }

		  /// <inheritdoc />
		  protected override async Task ActAsync(CancellationToken cancellationToken) =>
			  Result = await Repository.AddAsync(EventDocument, CancellationTokenSource.Token);

		  /// <inheritdoc />
		  protected override void ArrangeContainer(Mock<Container> mock) => mock
			  .Setup(x => x.CreateItemAsync(EventDocument, new PartitionKey(EventDocument.PartitionKey),
				  It.IsAny<ItemRequestOptions>(), CancellationTokenSource.Token)).ReturnsAsync(ArrangeItemResponse());

		  private static Func<EventDocument, PartitionKey, ItemRequestOptions, CancellationToken, ItemResponse<EventDocument>>
			  ArrangeItemResponse() => (e, _, _, _) => {
					var mock = new Mock<ItemResponse<EventDocument>>();
					mock.Setup(x => x.Resource).Returns(e);
					mock.Setup(x => x.StatusCode).Returns(HttpStatusCode.OK);
					return mock.Object;
			  };
	 }
}
