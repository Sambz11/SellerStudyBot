using KeySellerBot.Model.PaymentEntyties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace KeySellerBot.Model
{
    /// <summary>
    /// Partial minimum needed realization Qiwi Api
    /// </summary>
    public static class Payment
    {
        public static async Task<InvoiceDataResponse> CreateInvoiceAsync(decimal amount, string invoiceID, string comment = "Бот-магазин")
        {
            string query = "https://api.qiwi.com/partner/bill/v1/bills/" + invoiceID + "/";

            WebRequest request = WebRequest.CreateHttp(query);

            request.Headers.Add(HttpRequestHeader.Accept, "application/json");
            request.Headers.Add(HttpRequestHeader.Authorization, BotSettings.QiwiApiKey);
            request.ContentType = "application/json";
            request.Method = WebRequestMethods.Http.Put;

            string requestData = $"{{\"amount\":{{\"currency\":\"RUB\",\"value\":{amount}.00}},\"comment\":\"{comment}\",\"expirationDateTime\":\"{DateTime.Now.AddDays(2):O}\",\"customFields\":{{\"themeCode\":\"BHtVRxLgVF\"}}}}";

            Stream dataStream = request.GetRequestStream();
            using (var streamWriter = new StreamWriter(dataStream))
            {
                streamWriter.Write(requestData);
            }
            dataStream.Close();

            var response = await request.GetResponseAsync();

            var someString = await new StreamReader(response.GetResponseStream()).ReadToEndAsync();

            try
            {
                var tl = System.Text.Json.JsonSerializer.Deserialize<InvoiceDataResponse>(someString);
                return tl;
            }
            catch (Exception e)
            {
                throw new Exception("Error of response parsing", e);
            }

        }

        public static async Task<InvoiceDataResponse> CancelInvoiceAsync(string invoiceID)
        {
            string query = "https://api.qiwi.com/partner/bill/v1/bills/" + invoiceID + "/reject";

            WebRequest request = WebRequest.CreateHttp(query);

            request.Headers.Add(HttpRequestHeader.Accept, "application/json");
            request.Headers.Add(HttpRequestHeader.Authorization, BotSettings.QiwiApiKey);
            request.ContentType = "application/json";
            request.Method = WebRequestMethods.Http.Post;

            var response = await request.GetResponseAsync();
            var someString = await new StreamReader(response.GetResponseStream()).ReadToEndAsync();

            try
            {
                var tl = System.Text.Json.JsonSerializer.Deserialize<InvoiceDataResponse>(someString);
                return tl;
            }
            catch (Exception e)
            {
                throw new Exception("Error of response parsing", e);
            }
        }


        public static async Task<bool> CheckInvoiceAsync(string invoiceID)
        {
            string query = "https://api.qiwi.com/partner/bill/v1/bills/" + invoiceID + "/";

            WebRequest request = WebRequest.CreateHttp(query);

            request.Headers.Add(HttpRequestHeader.Accept, "application/json");
            request.Headers.Add(HttpRequestHeader.Authorization, BotSettings.QiwiApiKey);
            request.ContentType = "application/json";
            request.Method = WebRequestMethods.Http.Get;

            WebResponse response = request.GetResponse();

            var someString = await new StreamReader(response.GetResponseStream()).ReadToEndAsync();

            try
            {
                var tl = System.Text.Json.JsonSerializer.Deserialize<InvoiceDataResponse>(someString);
                return tl.Status.Value == "PAID";
            }
            catch (Exception e)
            {
                throw new Exception("Error of response parsing", e);
            }
        }
    }
}
