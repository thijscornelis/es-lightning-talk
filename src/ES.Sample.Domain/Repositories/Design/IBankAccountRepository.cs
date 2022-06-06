using ES.Framework.Domain.Repositories.Design;
using ES.Sample.Domain.Aggregates;

namespace ES.Sample.Domain.Repositories.Design;

/// <inheritdoc />
public interface IBankAccountRepository : IAggregateRepository<BankAccount, BankAccountId, BankAccountState, Guid>
{ }
