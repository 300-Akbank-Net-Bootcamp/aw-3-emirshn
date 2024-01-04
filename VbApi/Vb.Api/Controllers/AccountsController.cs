using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Schema;

namespace VbApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IMediator mediator;

    public AccountsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<AccountResponse>>> Get()
    {
        var operation = new GetAllAccountQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("{accountNumber}")]
    public async Task<ApiResponse<AccountResponse>> Get(int accountNumber)
    {
        var operation = new GetAccountByNumberQuery(accountNumber);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost]
    public async Task<ApiResponse<AccountResponse>> Post([FromBody] AccountRequest account)
    {
        var operation = new CreateAccountCommand(account);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut("{accountNumber}")]
    public async Task<ApiResponse> Put(int accountNumber, [FromBody] AccountRequest account)
    {
        var operation = new UpdateAccountCommand(accountNumber, account);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("{accountNumber}")]
    public async Task<ApiResponse> Delete(int accountNumber)
    {
        var operation = new DeleteAccountCommand(accountNumber);
        var result = await mediator.Send(operation);
        return result;
    }
}