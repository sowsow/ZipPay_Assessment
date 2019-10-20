using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZipPay.Common.Data.Models;
using ZipPay.Common.Rest.Models;
using ZipPay.Common.Rest.Models.AccountServiceApi;
using ZipPay.Common.Rest.Models.UserServiceApi;
using ZipPay.Services.Api.Services;

namespace ZipPay.Services.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IAccountService _accountService;

        private readonly IMapper _mapper;
        
        public UserController(
            IUserService userService,
            IAccountService accountService,
            IMapper mapper)
        {
            _userService = userService;
            _accountService = accountService;
            _mapper = mapper;
        }

        // GET: User
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<UserResponse>> GetAsync(long id)
        {
            var result = await _userService.FindByIdAsync(id);

            if (result.Resource == null)
            {
                return NotFound(new ErrorResponse(result.Message));
            }

            var response = _mapper.Map<User, UserResponse>(result.Resource);

            return Ok(response);
        }

        // GET: User
        [HttpGet("list")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<IEnumerable<User>>> ListAllAsync()
        {
           var result = await _userService.ListAllAsync();

            if (result.Resource == null)
            {
                return NotFound(new ErrorResponse(result.Message));
            }

            return Ok(result.Resource);
        }

        // GET: User Accounts
        [HttpGet("{id}/accounts")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<IEnumerable<Account>>> ListAccountsByUserId(long id)
        {
            var result = await _accountService.FindByUserIdAsync(id);

            if (result.Resource == null)
            {
                return NotFound(new ErrorResponse(result.Message));
            }

            return Ok(result.Resource);
        }

        // POST: User
        [HttpPost]
        [ProducesResponseType( 201)]
        [ProducesResponseType( 400)]
        public async Task<ActionResult<UserResponse>> PostAsync([FromBody] CreateUserRequest request)
        {
            var user = _mapper.Map<CreateUserRequest, User>(request);

            var result = await _userService.SaveAsync(user);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponse(result.Message));
            }

            var response = _mapper.Map<User, UserResponse>(result.Resource);

            return Ok(response);
        }

        // POST: Account
        [HttpPost("{id}/Account")]
        [ProducesResponseType( 201)]
        [ProducesResponseType( 400)]
        public async Task<ActionResult<AccountResponse>> PostAsync(long id, [FromBody] CreateAccountRequest accountRequest)
        {
            var account = _mapper.Map<CreateAccountRequest, Account>(accountRequest);

            var result = await _userService.CreateAccountAsync(id, account);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponse(result.Message));
            }

            var response = _mapper.Map<Account, AccountResponse>(result.Resource);

            return Ok(response);
        }
    }
}
