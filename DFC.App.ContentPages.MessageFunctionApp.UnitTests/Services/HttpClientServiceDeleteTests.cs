﻿using DFC.App.ContentPages.MessageFunctionApp.Models;
using DFC.App.ContentPages.MessageFunctionApp.Services;
using DFC.App.ContentPages.MessageFunctionApp.UnitTests.FakeHttpHandlers;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContentPages.MessageFunctionApp.UnitTests.Services
{
    [Trait("Messaging Function", "HttpClientService Delete Tests")]
    public class HttpClientServiceDeleteTests
    {
        private readonly ILogger logger;
        private readonly ContentPageClientOptions contentPageClientOptions;

        public HttpClientServiceDeleteTests()
        {
            logger = A.Fake<ILogger>();
            contentPageClientOptions = new ContentPageClientOptions
            {
                BaseAddress = new Uri("https://somewhere.com", UriKind.Absolute),
            };
        }

        [Fact]
        public async Task DeleteAsyncReturnsOkStatusCodeForExistingId()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            var httpResponse = new HttpResponseMessage { StatusCode = expectedResult };
            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = contentPageClientOptions.BaseAddress };
            var httpClientService = new HttpClientService(contentPageClientOptions, httpClient, logger);

            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            // act
            var result = await httpClientService.DeleteAsync(Guid.NewGuid()).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        [Fact]
        public async Task DeleteAsyncReturnsNotFoundStatusCodeForExistingId()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.NotFound;
            var httpResponse = new HttpResponseMessage { StatusCode = expectedResult };
            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = contentPageClientOptions.BaseAddress };
            var httpClientService = new HttpClientService(contentPageClientOptions, httpClient, logger);

            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            // act
            var result = await httpClientService.DeleteAsync(Guid.NewGuid()).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }

        [Fact]
        public async Task DeleteAsyncReturnsExceptionForBadStatus()
        {
            // arrange
            const HttpStatusCode expectedResult = HttpStatusCode.Forbidden;
            var httpResponse = new HttpResponseMessage { StatusCode = expectedResult, Content = new StringContent("bad delete") };
            var fakeHttpRequestSender = A.Fake<IFakeHttpRequestSender>();
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(fakeHttpRequestSender);
            var httpClient = new HttpClient(fakeHttpMessageHandler) { BaseAddress = contentPageClientOptions.BaseAddress };
            var httpClientService = new HttpClientService(contentPageClientOptions, httpClient, logger);

            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).Returns(httpResponse);

            // act
            var exceptionResult = await Assert.ThrowsAsync<HttpRequestException>(async () => await httpClientService.DeleteAsync(Guid.NewGuid()).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeHttpRequestSender.Send(A<HttpRequestMessage>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal($"Response status code does not indicate success: {(int)expectedResult} ({expectedResult}).", exceptionResult.Message);

            httpResponse.Dispose();
            httpClient.Dispose();
            fakeHttpMessageHandler.Dispose();
        }
    }
}
