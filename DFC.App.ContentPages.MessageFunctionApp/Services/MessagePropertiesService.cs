using Microsoft.Azure.ServiceBus;

namespace DFC.App.ContentPages.MessageFunctionApp.Services
{
    public class MessagePropertiesService : IMessagePropertiesService
    {
        public long GetSequenceNumber(Message message)
        {
            return (message?.SystemProperties?.SequenceNumber).GetValueOrDefault();
        }
    }
}