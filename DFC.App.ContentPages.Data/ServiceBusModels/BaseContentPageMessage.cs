using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContentPages.Data.ServiceBusModels
{
    public class BaseContentPageMessage
    {
        [Required]
        public Guid ContentPageId { get; set; }
    }
}
