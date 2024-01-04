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
    public class AddressCommandHandler :
        IRequestHandler<CreateAddressCommand, ApiResponse<AddressResponse>>,
        IRequestHandler<UpdateAddressCommand, ApiResponse>,
        IRequestHandler<DeleteAddressCommand, ApiResponse>
    {
        private readonly VbDbContext dbContext;
        private readonly IMapper mapper;

        public AddressCommandHandler(VbDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<AddressResponse>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            var checkAddress = await dbContext.Set<Address>()
                .Where(x => x.CustomerId == request.Model.CustomerId && x.Address1 == request.Model.Address1)
                .FirstOrDefaultAsync(cancellationToken);

            if (checkAddress != null)
            {
                return new ApiResponse<AddressResponse>("Address already exists for the customer.");
            }

            var entity = mapper.Map<AddressRequest, Address>(request.Model);

            var entityResult = await dbContext.AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var mapped = mapper.Map<Address, AddressResponse>(entityResult.Entity);
            return new ApiResponse<AddressResponse>(mapped);
        }

        public async Task<ApiResponse> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var fromDb = await dbContext.Set<Address>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

            if (fromDb == null)
            {
                return new ApiResponse("Address not found");
            }

            fromDb.Address1 = request.Model.Address1;
            fromDb.Address2 = request.Model.Address2;
            fromDb.Country = request.Model.Country;
            fromDb.City = request.Model.City;
            fromDb.County = request.Model.County;
            fromDb.PostalCode = request.Model.PostalCode;
            fromDb.IsDefault = request.Model.IsDefault

            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }

        public async Task<ApiResponse> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            var fromDb = await dbContext.Set<Address>().Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

            if (fromDb == null)
            {
                return new ApiResponse("Address not found");
            }

            fromDb.IsActive = false;
            await dbContext.SaveChangesAsync(cancellationToken);
            return new ApiResponse();
        }
    }
}
