using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KeySellerBot.Model.PaymentEntyties
{
    public class InvoceCustomFields
    {
        //themeCode
        [JsonPropertyName("themeCode")]
        public string ThemeCode { get; set; }
    }
}
