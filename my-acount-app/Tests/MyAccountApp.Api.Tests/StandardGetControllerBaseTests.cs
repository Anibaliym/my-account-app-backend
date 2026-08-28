using Microsoft.AspNetCore.Mvc;
using MyAccountApp.Api.Controllers;
using MyAccountApp.Application.Responses;
using Xunit;

namespace MyAccountApp.Api.Tests;

public class StandardGetControllerBaseTests
{
    private readonly TestController _controller = new();

    [Fact]
    public async Task Single_with_data_returns_standard_success()
    {
        var item = new TestItem(Guid.NewGuid());

        IActionResult result = await _controller.Single(item.Id, () => Task.FromResult<TestItem?>(item));

        var response = Assert.IsType<GenericResponse<TestItem>>(Assert.IsType<OkObjectResult>(result).Value);
        Assert.True(response.Resolution);
        Assert.Same(item, response.Data);
        Assert.Equal(ResponseMessages.Success, response.Message);
        Assert.Null(response.ErrorCode);
        Assert.Null(response.Errors);
    }

    [Fact]
    public async Task Empty_collection_returns_standard_success_with_empty_list()
    {
        IActionResult result = await _controller.Collection(Guid.NewGuid(),
            () => Task.FromResult<IEnumerable<TestItem>>(Array.Empty<TestItem>()));

        var response = Assert.IsType<GenericResponse<List<TestItem>>>(Assert.IsType<OkObjectResult>(result).Value);
        Assert.True(response.Resolution);
        Assert.Empty(Assert.IsType<List<TestItem>>(response.Data));
        Assert.Null(response.ErrorCode);
    }

    [Fact]
    public async Task Missing_single_returns_resource_specific_not_found()
    {
        IActionResult result = await _controller.Single(Guid.NewGuid(),
            () => Task.FromResult<TestItem?>(null));

        var response = Assert.IsType<GenericResponse<TestItem>>(Assert.IsType<NotFoundObjectResult>(result).Value);
        Assert.False(response.Resolution);
        Assert.Null(response.Data);
        Assert.Equal(ErrorCodes.CardNotFound, response.ErrorCode);
    }

    [Fact]
    public async Task Empty_identifier_returns_validation_failure()
    {
        IActionResult result = await _controller.Single(Guid.Empty,
            () => Task.FromResult<TestItem?>(null));

        var response = Assert.IsType<GenericResponse<TestItem>>(Assert.IsType<BadRequestObjectResult>(result).Value);
        Assert.False(response.Resolution);
        Assert.Null(response.Data);
        Assert.Equal(ErrorCodes.CommonValidationFailed, response.ErrorCode);
        Assert.NotEmpty(response.Errors!);
    }

    [Fact]
    public async Task Unexpected_exception_returns_safe_standard_failure()
    {
        IActionResult result = await _controller.Single(Guid.NewGuid(),
            () => Task.FromException<TestItem?>(new InvalidOperationException("sensitive")));

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        var response = Assert.IsType<GenericResponse<TestItem>>(objectResult.Value);
        Assert.False(response.Resolution);
        Assert.Null(response.Data);
        Assert.Equal(ErrorCodes.CommonUnexpectedError, response.ErrorCode);
        Assert.DoesNotContain("sensitive", response.Message);
    }

    private sealed record TestItem(Guid Id);

    private sealed class TestController : StandardGetControllerBase
    {
        public Task<IActionResult> Single(Guid id, Func<Task<TestItem?>> query) =>
            GetSingle(id, query, ErrorCodes.CardNotFound);

        public Task<IActionResult> Collection(Guid id, Func<Task<IEnumerable<TestItem>>> query) =>
            GetCollection(id, query);
    }
}
