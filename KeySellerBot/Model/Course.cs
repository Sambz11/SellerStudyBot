using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KeySellerBot.Model
{
    
    public class Course
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string Descriprion { get; set; }

        public string FileId { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal Price { get; set; }

        public List<long> Buyers { get; set; }

        [JsonPropertyName("key")]
        public string InvoiceString { get; set; }

    }
}
