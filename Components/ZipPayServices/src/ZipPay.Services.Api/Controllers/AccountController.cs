using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZipPay.Common.Data.Models;
using ZipPay.Common.Rest.Models;
using ZipPay.Common.Rest.Models.AccountServiceApi;
using ZipPay.Services.Api.Services;

namespace ZipPay.Services.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        private readonly IMapper _mapper;

        public AccountController(
            IAccountService accountService,
            IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        // POST: Account
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<AccountResponse>> PostAsync([FromQuery] CreateAccountRequest request)
        {
            var account = _mapper.Map<CreateAccountRequest, Account>(request);

            var result = await _accountService.SaveAsync(account);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponse(result.Message));
            }

            var response = _mapper.Map<Account, AccountResponse>(result.Resource);

            return Ok(response);
        }
    }
}
