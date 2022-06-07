using ES.Framework.Domain.Abstractions;
using ES.Framework.Domain.Documents.Design;
using ES.Framework.Domain.Repositories;
using ES.Framework.Domain.Repositories.Design;
using ES.Sample.Domain.Aggregates;
using ES.Sample.Domain.Repositories.Design;

namespace ES.Sample.Infrastructure.Repositories;

/// <inheritdoc cref="IBankAccountRepository" />
public class BankAccountRepository : AggregateRepository<BankAccount, BankAccountId, BankAccountState, Guid>, IBankAccountRepository
{
	 /// <inheritdoc />
	 public BankAccountRepository(IDocumentRepository documentRepository, IEventDocumentConverter eventDocumentConverter, IPartitionKeyResolver partitionKeyResolver) : base(documentRepository, eventDocumentConverter, partitionKeyResolver) {
	 }
}
