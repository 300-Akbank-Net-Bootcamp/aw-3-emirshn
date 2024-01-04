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
    public class EftTransactionCommandHandler :
        IRequestHandler<CreateEftTransactionCommand, ApiResponse<EftTransactionResponse>>,
        IRequestHandler<UpdateEftTransactionCommand, ApiResponse>,
        IRequestHandler<DeleteEftTransactionCommand, ApiResponse>
    {
        private readonly VbDbContext dbContext;
        private readonly IMapper mapper;

        public EftTransactionCommandHandler(VbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<EftTransactionResponse>> Handle(CreateEftTransactionCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<EftTransactionRequest, EftTransaction>(request.Model);

            var entityResult = await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var mapped = mapper.Map<EftTransaction, EftTransactionResponse>(entityResult.Entity);
            return new ApiResponse<EftTransactionResponse>(mapped);
        }

        public async Task<ApiResponse> Handle(UpdateEftTransactionCommand request, CancellationToken cancellationToken)
        {
            var fromDb = await dbContext.Set<EftTransaction>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

            if (fromDb == null)
            {
                return new ApiResponse("Eft transaction not found");
            }

            fromDb.ReferenceNumber = request.Model.ReferenceNumber;
            fromDb.TransactionDate = request.Model.TransactionDate;
            fromDb.Amount = request.Model.Amount;
            fromDb.Description = request.Model.Description;
            fromDb.SenderAccount = request.Model.SenderAccount;
            fromDb.SenderIban = request.Model.SenderIban;
            fromDb.SenderName = request.Model.SenderName;

            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        public async Task<ApiResponse> Handle(DeleteEftTransactionCommand request, CancellationToken cancellationToken)
        {
            var fromDb = await dbContext.Set<EftTransaction>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

            if (fromDb == null)
            {
                return new ApiResponse("Eft transaction not found");
            }

            dbContext.Set<EftTransaction>().Remove(fromDb);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }
    }
}
