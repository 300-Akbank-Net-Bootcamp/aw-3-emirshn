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
    public class AccountTransactionCommandHandler :
        IRequestHandler<CreateAccountTransactionCommand, ApiResponse<AccountTransactionResponse>>,
        IRequestHandler<UpdateAccountTransactionCommand, ApiResponse>,
        IRequestHandler<DeleteAccountTransactionCommand, ApiResponse>
    {
        private readonly VbDbContext dbContext;
        private readonly IMapper mapper;

        public AccountTransactionCommandHandler(VbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<AccountTransactionResponse>> Handle(CreateAccountTransactionCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<AccountTransactionRequest, AccountTransaction>(request.Model);

            var entityResult = await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var mapped = mapper.Map<AccountTransaction, AccountTransactionResponse>(entityResult.Entity);
            return new ApiResponse<AccountTransactionResponse>(mapped);
        }

        public async Task<ApiResponse> Handle(UpdateAccountTransactionCommand request, CancellationToken cancellationToken)
        {
            var fromDb = await dbContext.Set<AccountTransaction>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

            if (fromDb == null)
            {
                return new ApiResponse("Account transaction not found");
            }

            fromDb.ReferenceNumber = request.Model.ReferenceNumber;
            fromDb.TransactionDate = request.Model.TransactionDate;
            fromDb.Amount = request.Model.Amount;
            fromDb.Description = request.Model.Description;
            fromDb.TransferType = request.Model.TransferType;

            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        public async Task<ApiResponse> Handle(DeleteAccountTransactionCommand request, CancellationToken cancellationToken)
        {
            var fromDb = await dbContext.Set<AccountTransaction>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

            if (fromDb == null)
            {
                return new ApiResponse("Account transaction not found");
            }

            dbContext.Set<AccountTransaction>().Remove(fromDb);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }
    }
}
