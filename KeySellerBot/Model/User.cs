using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KeySellerBot.Model
{
    public class User
    {
        [JsonPropertyName("d")]
        public long Id { get; set; }

        [JsonPropertyName("r")]
        public List<long> Referals { get; set; }

        [JsonPropertyName("b")]
        public decimal Balance { get; set; }
    }
}
