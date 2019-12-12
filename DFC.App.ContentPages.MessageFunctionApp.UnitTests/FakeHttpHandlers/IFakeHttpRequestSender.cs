using System.Net.Http;

namespace DFC.App.ContentPages.MessageFunctionApp.UnitTests.FakeHttpHandlers
{
    public interface IFakeHttpRequestSender
    {
        HttpResponseMessage Send(HttpRequestMessage request);
    }
}