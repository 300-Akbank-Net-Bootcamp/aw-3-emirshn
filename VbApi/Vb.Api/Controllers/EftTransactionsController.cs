using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Schema;

namespace VbApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EftTransactionsController : ControllerBase
{
    private readonly IMediator mediator;

    public EftTransactionsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<EftTransactionResponse>>> Get()
    {
        var operation = new GetAllEftTransactionQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<EftTransactionResponse>> Get(int id)
    {
        var operation = new GetEftTransactionByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost]
    public async Task<ApiResponse<EftTransactionResponse>> Post([FromBody] EftTransactionRequest transaction)
    {
        var operation = new CreateEftTransactionCommand(transaction);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> Put(int id, [FromBody] EftTransactionRequest transaction)
    {
        var operation = new UpdateEftTransactionCommand(id, transaction);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new DeleteEftTransactionCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }
}