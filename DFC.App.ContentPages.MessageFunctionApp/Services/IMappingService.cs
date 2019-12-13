using DFC.App.ContentPages.Data.Models;

namespace DFC.App.ContentPages.MessageFunctionApp.Services
{
    public interface IMappingService
    {
        ContentPageModel MapToContentPageModel(string message, long sequenceNumber);
    }
}