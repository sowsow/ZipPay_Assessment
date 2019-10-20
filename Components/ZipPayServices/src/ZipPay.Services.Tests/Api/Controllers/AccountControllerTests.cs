using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;
using ZipPay.Common.Data.Models;
using ZipPay.Common.Rest.Models.AccountServiceApi;
using ZipPay.Common.Services.Models;
using ZipPay.Services.Api.Controllers;
using ZipPay.Services.Api.Services;

namespace ZipPay.Services.Tests.Api.Controllers
{
    public class AccountControllerTests
    {
        private readonly AccountController _accountController;

        private readonly IAccountService _accountService;

        private readonly IMapper _mapper;
        
        private readonly IFixture _fixture;

        public AccountControllerTests()
        {
            _fixture = new Fixture();

            _accountService = Substitute.For<IAccountService>();
            _mapper = Substitute.For<IMapper>();

            _accountController = new AccountController(_accountService, _mapper);
        }

        [Fact]
        public async Task GivenValidRequestWhenAccountPostAsyncThenReturnCorrectValue()
        {
            var validRequest = new CreateAccountRequest
            {
                Id = 1,
                Name = "smith",
                CreatedByUserId = _fixture.Create<long>()
            };

            var account = _fixture.Create<Account>();
            var accountWrapper = new AccountWrapper(account);

            _mapper
                .Map<CreateAccountRequest, Account>(validRequest)
                .Returns(account);

            _accountService
                .SaveAsync(account)
                .Returns(accountWrapper);

            var actual = await _accountController.PostAsync(validRequest);

            (actual.Result as StatusCodeResult)?
                .StatusCode.Should()
                .Be((int)HttpStatusCode.OK);
        }
    }
}
