using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using FluentAssertions;

namespace Restaurants.API.Middleware.Tests
{
    public class ErrorHandlingMiddlewareTests
    {
        [Fact()]
        public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegateAsync()
        {
            // arrange
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var context = new DefaultHttpContext();
            var nextDelegateMock = new Mock<RequestDelegate>();

            // act
            await middleware.InvokeAsync(context, nextDelegateMock.Object);


            // assert
            nextDelegateMock.Verify(next => next.Invoke(context), Times.Once);
        }

        [Fact()]
        public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCodeTo404AndWriteExceptionMessage()
        {
            // arrange
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var context = new DefaultHttpContext();
            var exception = new NotFoundException(nameof(Restaurant), "1");

            // act
            await middleware.InvokeAsync(context, _ => throw exception);


            // assert
            context.Response.StatusCode.Should().Be(404);
        }

        [Fact()]
        public async Task InvokeAsync_WhenForbidExceptionThrown_ShouldSetStatusCodeTo403AndWriteExceptionMessage()
        {
            // arrange
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var context = new DefaultHttpContext();
            var exception = new ForbidException();

            // act
            await middleware.InvokeAsync(context, _ => throw exception);


            // assert
            context.Response.StatusCode.Should().Be(403);
        }


        [Fact()]
        public async Task InvokeAsync_WhenGenericExceptionThrown_ShouldSetStatusCodeTo500()
        {
            // arrange
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var context = new DefaultHttpContext();
            var exception = new Exception();

            // act
            await middleware.InvokeAsync(context, _ => throw exception);


            // assert
            context.Response.StatusCode.Should().Be(500);
        }
    }
}