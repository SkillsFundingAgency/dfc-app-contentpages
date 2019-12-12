using DFC.App.ContentPages.Data.Enums;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContentPages.MessageFunctionApp.Services
{
    public interface IMessageProcessor
    {
        Task<HttpStatusCode> ProcessAsync(string message, long sequenceNumber, MessageContentType messageContentType, MessageAction messageAction);
    }
}