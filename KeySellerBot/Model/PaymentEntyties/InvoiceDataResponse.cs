using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KeySellerBot.Model.PaymentEntyties
{
    public class InvoiceDataResponse
    {
        //billId str
        [JsonPropertyName("billId")]
        public string BillId { get; set; }

        //siteId str
        [JsonPropertyName("siteId")]
        public string SiteId { get; set; }

        //amount {amount.value Number} {amount.currency str("RUB")}
        [JsonPropertyName("amount")]
        public InvoiceAmount Amount { get; set; }

        //status {status.value str}{status.changedDateTime Date}
        [JsonPropertyName("status")]
        public InvoiceStatus Status { get; set; }

        //customFields str
        [JsonPropertyName("customFields")]
        public InvoceCustomFields CustomFields { get; set; }
        
        //customer {email, phone, account}
        [JsonPropertyName("customer")]
        public string Customer { get; set; }
        
        //comment str
        [JsonPropertyName("comment")]
        public string Comment { get; set; }
        
        //creationDateTime date
        [JsonPropertyName("creationDateTime")]
        public DateTime CreationDateTime { get; set; }
        
        //payUrl str
        [JsonPropertyName("payUrl")]
        public string PayUrl { get; set; }
        
        //expirationDateTime date
        [JsonPropertyName("expirationDateTime")]
        public string expirationDateTime { get; set; }

    }
}
