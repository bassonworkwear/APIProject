using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Company.Function;
using System.Globalization;
using System.Linq;

namespace BibleVerseVS
{
    public class BibleVerse
    {
        public DateTime DmtDate { get; set; }
        public string txtVerse { get; set; }
        public string txtRef { get; set; }
        public string txtTrans { get; set; }
    }

    public class UpdateBibleVerse
    {
        public DateTime dmtDate { get; set; }
        public string txtVerse { get; set; }
        public string txtRef { get; set; }
        public string txtTrans { get; set; }
    }
    public static class Function1
    {
        public static readonly List<BibleVerse> bibleverses = new List<BibleVerse>();

        [FunctionName("CreateBibleVerse")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequest req,
            [Sql("[dbo].[BibleVerse]", ConnectionStringSetting = "SqlConnectionString")] IAsyncCollector<BibleVerse> verses,
            ILogger log)
        {
            log.LogInformation("Create a new Bibile Verse");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<BibleVerse>(requestBody);

            var verse = new BibleVerse()
            {
                DmtDate = input.DmtDate,
                txtVerse = input.txtVerse,
                txtRef = input.txtRef,
                txtTrans = input.txtTrans
            };
            await verses.AddAsync(verse);
            return new OkObjectResult(verse);
        }

        [FunctionName("GetBibleVerseByDate")]
        public static IActionResult GetBibleVerseByDate(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "versedate")] HttpRequest req,
            [Sql("SELECT * FROM [dbo].[BibleVerse] WHERE dmtDate=@Date",
            CommandType = System.Data.CommandType.Text,
            Parameters ="@Date={Query.dmtDate}",
            ConnectionStringSetting = "SqlConnectionString")] IAsyncCollector<BibleVerse> result,
            ILogger log)
        {
            log.LogInformation("Getting Bible Verse by Date");

            return new OkObjectResult(result);
        }

    }
}
