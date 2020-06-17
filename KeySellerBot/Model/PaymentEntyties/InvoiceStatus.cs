using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KeySellerBot.Model.PaymentEntyties
{
    public class InvoiceStatus
    {
        //status {status.value str}{status.changedDateTime Date}
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("changedDateTime")]
        public DateTime ChangedDateTime { get; set; }
    }
}
