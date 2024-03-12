using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Exam_Dictionardle
{
    public static class ApiController
    {

        private static string _key = "aa3262ce-7e87-4110-bfc7-b05f09b677e8";

        private static HttpClient _httpClient = new()
        {
            BaseAddress = new Uri(@"https://dictionaryapi.com/api/v3/references/learners/json/"),
        };

        public static async Task<string> GetWord(string word)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(@$"{word}?key={_key}").ConfigureAwait(false);
           
            return await response.Content.ReadAsStringAsync();
        }



    }
}
