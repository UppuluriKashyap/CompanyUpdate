using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Ganss.Excel;
using Newtonsoft.Json;

namespace CompanyUpdate
{
    public class CompanyUpdate
    {
        private static HttpRequestMessage CreateCompanyTINUpdateRequest(string url, string username, string password, CompanyUpdateModel companyModel)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}")));
            var json = JsonConvert.SerializeObject(companyModel);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return request;
        }

        private static async Task<CompanyModel> CallCompanyUpdate(CompanyUpdateModel companyModel, string url,
            string userName, string password)
        {
            using (var request = CreateCompanyTINUpdateRequest(url, userName, password, companyModel))
            {
                HttpClient client = new HttpClient();
                var response = await client.SendAsync(request).ConfigureAwait(false);
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (result != null && result.Contains("error"))
                {
                    Console.WriteLine(result);
                    return null;
                }
                return JsonConvert.DeserializeObject<CompanyModel>(result);
            }
        }

        public static void Main(string[] args)
        {
            // Reading the environment, username and password from the user
            Console.Write("Enter the environment: ");
            string environment = Console.ReadLine();
            Console.Write("Enter the username: ");
            string userName = Console.ReadLine();
            Console.Write("Enter the password: ");
            string password = Console.ReadLine();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var companies = new ExcelMapper(Environment.CurrentDirectory + "\\CompanyUpdate.xlsx").Fetch<CompanyUpdateModel>().ToList();
            string url = null;
            if (string.Equals("CQA", environment, StringComparison.OrdinalIgnoreCase))
            {
                url = "https://cqa-restv2.avalara.net/api/v2/companies/";
            }
            if (string.Equals("FQA", environment, StringComparison.OrdinalIgnoreCase))
            {
                url = "http://fqa.avalara.net:8020/api/v2/companies/";
            }
            if (string.Equals("SBX", environment, StringComparison.OrdinalIgnoreCase))
            {
                url = "https://sandbox-rest.avatax.com/api/v2/companies/";
            }
            if (string.Equals("PRD", environment, StringComparison.OrdinalIgnoreCase))
            {
                url = "https://rest.avatax.com/api/v2/companies/";
            }
            foreach (var company in companies)
            {
                url += company.companyId;
                var companyUpdate = new CompanyUpdateModel
                {
                    name = company.name,
                    companyCode = company.companyCode,
                    defaultCountry = company.defaultCountry,
                    accountId = company.accountId,
                    taxPayerIdNumber = null
                };
                var result = CallCompanyUpdate(companyUpdate, url, userName, password).Result;
                if (result != null)
                {
                    Console.WriteLine("Update the TIN for Company: " + result.id);
                }
            }
        }
    }
}
