using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace BibleVerseVS
{
    public static class Function1Base
    {

        [FunctionName("GetBibleVerse")]
        public static IActionResult GetBibleVerse(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
            HttpRequest req,
            [Sql("SELECT * FROM [dbo].[BibleVerse]",
            CommandType = System.Data.CommandType.Text,
            ConnectionStringSetting = "SqlConnectionString")] IEnumerable<Object> result,
                ILogger log)
        {
            log.LogInformation("Getting All Bible Verse");

            return new OkObjectResult(result);
        }
    }
}