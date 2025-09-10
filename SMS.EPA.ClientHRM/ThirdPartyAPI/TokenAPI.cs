namespace SMS.EPA.ClientHrm.ThirdPartyAPI
{
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Model;

    public class TokenAPI : ITokenAPI
    {
        public async Task<TokenAccess> RequestAsync(ApplicationInfo info)
        {
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string> { { "grant_type", info.GrantType }, { "username", info.UserName }, { "password", info.Password } };
                using (var encodedContent = new FormUrlEncodedContent(form))
                {
                    var tokenResponse = await client.PostAsync(new Uri(ApplicationConfig.ServiceAPI + "token"), encodedContent);

                    var jsonContent = await tokenResponse.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<TokenAccess>(jsonContent);
                }
            }
        }
    }
}