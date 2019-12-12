using Microsoft.Azure.ServiceBus;

namespace DFC.App.ContentPages.MessageFunctionApp.Services
{
    public interface IMessagePropertiesService
    {
        long GetSequenceNumber(Message message);
    }
}