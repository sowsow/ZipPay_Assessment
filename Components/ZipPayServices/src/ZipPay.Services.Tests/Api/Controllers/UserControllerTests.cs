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
using ZipPay.Common.Rest.Models.UserServiceApi;
using ZipPay.Common.Services.Models;
using ZipPay.Services.Api.Controllers;
using ZipPay.Services.Api.Services;

namespace ZipPay.Services.Tests.Api.Controllers
{
    public class UserControllerTests
    {
        private readonly UserController _userController;

        private readonly IUserService _userService;

        private readonly IAccountService _accountService;

        private readonly IMapper _mapper;

        private readonly IFixture _fixture;

        private readonly long _userId;

        private readonly User _user;

        private readonly UserWrapper _userWrapper;

        private readonly UserResponse _userResponse;

        public UserControllerTests()
        {
            _fixture = new Fixture();
            _userId = _fixture.Create<long>();
            _user = _fixture.Create<User>();
            _userWrapper = new UserWrapper(_user);
            _userResponse = new UserResponse
            {
                Id = _user.Id,
                Name = _user.Name,
                EmailAddress = _user.EmailAddress,
                MonthlySalary = _user.MonthlySalary,
                MonthlyExpenses = _user.MonthlyExpenses
            };

            _userService = Substitute.For<IUserService>();
            _accountService = Substitute.For<IAccountService>();
            _mapper = Substitute.For<IMapper>();

            _userController = new UserController(_userService, _accountService, _mapper);
        }

        [Fact]
        public async Task GivenUserExistWhenGetAsyncThenReturnCorrectValue()
        {
            _userService
                .FindByIdAsync(_userId)
                .Returns(_userWrapper);

            _mapper.Map<User, UserResponse>(_user).Returns(_userResponse);

            var actual = await _userController.GetAsync(_userId);

            (actual.Result as StatusCodeResult)?
                .StatusCode.Should()
                .Be((int) HttpStatusCode.OK);
        }

        [Fact]
        public async Task GivenUserNotExistWhenGetAsyncThenReturnCorrectValue()
        {
            var nullWrapper = _fixture.Create<UserWrapper>();
            nullWrapper.Resource = null;

            _userService
                .FindByIdAsync(_userId)
                .Returns(nullWrapper);

            var actual = await _userController.GetAsync(_userId);

            (actual.Result as StatusCodeResult)?
                .StatusCode.Should()
                .Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenUsersExistWhenListAllAsyncThenReturnCorrectValue()
        {
            var userEnumerableWrapper = _fixture.Create<UserEnumerableWrapper>();

            _userService
                .ListAllAsync()
                .Returns(userEnumerableWrapper);

            var actual = await _userController.ListAllAsync();

            (actual.Result as StatusCodeResult)?
                .StatusCode.Should()
                .Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GivenUserNotExistWhenListAllAsyncThenReturnCorrectValue()
        {
            var nullWrapper = _fixture.Create<UserEnumerableWrapper>();
            nullWrapper.Resource = null;

            _userService
                .ListAllAsync()
                .Returns(nullWrapper);

            var actual = await _userController.ListAllAsync();

            (actual.Result as StatusCodeResult)?
                .StatusCode.Should()
                .Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenUserExistWhenListAccountsByUserIdAsyncThenReturnCorrectValue()
        {
            var accountEnumerableWrapper = _fixture.Create<AccountEnumerableWrapper>();

            _accountService
                .FindByUserIdAsync(_userId)
                .Returns(accountEnumerableWrapper);

            var actual = await _userController.ListAccountsByUserId(_userId);

            (actual.Result as StatusCodeResult)?
                .StatusCode.Should()
                .Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GivenUsersNotExistWhenListAllAsyncThenReturnCorrectValue()
        {
            var nullWrapper = _fixture.Create<AccountEnumerableWrapper>();
            nullWrapper.Resource = null;

            _accountService
                .FindByUserIdAsync(_userId)
                .Returns(nullWrapper);

            var actual = await _userController.ListAccountsByUserId(_userId);

            (actual.Result as StatusCodeResult)?
                .StatusCode.Should()
                .Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GivenValidRequestWhenUserPostAsyncThenReturnCorrectValue()
        {
            var validRequest = new CreateUserRequest
            {
                Id = 1,
                EmailAddress = "smith@gmail.com",
                Name = "smith",
                MonthlySalary = 1000,
                MonthlyExpenses = 0
            };

            _mapper
                .Map<CreateUserRequest, User>(validRequest)
                .Returns(_user);

            _userService
                .SaveAsync(_user)
                .Returns(_userWrapper);

            var actual = await _userController.PostAsync(validRequest);

            (actual.Result as StatusCodeResult)?
                .StatusCode.Should()
                .Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GivenValidRequestWhenAccountPostAsyncThenReturnCorrectValue()
        {
            var validRequest = new CreateAccountRequest
            {
                Id = _fixture.Create<long>(),
                Name = _fixture.Create<string>(),
                CreatedByUserId = _userId
            };

            var account = _fixture.Create<Account>();
            var accountWrapper = new AccountWrapper(account);

            _mapper
                .Map<CreateAccountRequest, Account>(validRequest)
                .Returns(account);

            _userService
                .CreateAccountAsync(_userId, account)
                .Returns(accountWrapper);

            var actual = await _userController.PostAsync(_userId, validRequest);

            (actual.Result as StatusCodeResult)?
                .StatusCode.Should()
                .Be((int)HttpStatusCode.OK);
        }
    }
}
