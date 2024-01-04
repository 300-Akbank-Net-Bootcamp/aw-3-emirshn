using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Command
{
    public class AccountCommandHandler :
        IRequestHandler<CreateAccountCommand, ApiResponse<AccountResponse>>,
        IRequestHandler<UpdateAccountCommand, ApiResponse>,
        IRequestHandler<DeleteAccountCommand, ApiResponse>
    {
        private readonly VbDbContext dbContext;
        private readonly IMapper mapper;

        public AccountCommandHandler(VbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<AccountResponse>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var checkAccount = await dbContext.Set<Account>()
                .Where(x => x.AccountNumber == request.Model.AccountNumber)
                .FirstOrDefaultAsync(cancellationToken);

            if (checkAccount != null)
            {
                return new ApiResponse<AccountResponse>($"Account number {request.Model.AccountNumber} already exists.");
            }

            var entity = mapper.Map<AccountRequest, Account>(request.Model);
            entity.AccountNumber = new Random().Next(1000000, 9999999);

            var entityResult = await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var mapped = mapper.Map<Account, AccountResponse>(entityResult.Entity);
            return new ApiResponse<AccountResponse>(mapped);
        }

        public async Task<ApiResponse> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var fromDb = await dbContext.Set<Account>().Where(x => x.AccountNumber == request.Model.AccountNumber).FirstOrDefaultAsync(cancellationToken);

            if (fromDb == null)
            {
                return new ApiResponse("Account not found");
            }

            fromDb.IBAN = request.Model.IBAN;
            fromDb.Balance = request.Model.Balance;
            fromDb.CurrencyType = request.Model.CurrencyType;
            fromDb.Name = request.Model.Name;
            fromDb.OpenDate = request.Model.OpenDate;

            /*
            fromDb.AccountTransactions.Clear();
            fromDb.AccountTransactions.AddRange(request.Model.AccountTransactions.Select(transaction =>
                new AccountTransaction
                {
                    ReferenceNumber = transaction.ReferenceNumber,
                    TransactionDate = transaction.TransactionDate,
                    Amount = transaction.Amount,
                    Description = transaction.Description,
                    TransferType = transaction.TransferType
                }));

            fromDb.EftTransactions.Clear();
            fromDb.EftTransactions.AddRange(request.Model.EftTransactions.Select(transaction =>
                new EftTransaction
                {
                    ReferenceNumber = transaction.ReferenceNumber,
                    TransactionDate = transaction.TransactionDate,
                    Amount = transaction.Amount,
                    Description = transaction.Description,
                    SenderAccount = transaction.SenderAccount,
                    SenderIban = transaction.SenderIban,
                    SenderName = transaction.SenderName
                }));
            */
            
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        public async Task<ApiResponse> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            var fromDb = await dbContext.Set<Account>().Where(x => x.AccountNumber == request.Model.AccountNumber).FirstOrDefaultAsync(cancellationToken);

            if (fromDb == null)
            {
                return new ApiResponse("Account not found");
            }

            fromDb.IsActive = false;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }
    }
}
