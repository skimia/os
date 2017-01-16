using System;
using System.Dynamic;
using System.Net.Http;
using ExtCore.Infrastructure;
using GraphQLCore.Type;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Skimia.Extensions.GraphQl.Schema;
using Skimia.Extensions.GraphQl.Models;
using System.Linq;

namespace Skimia.Extensions.GraphQl.Controllers
{
    public class GraphiQlFrontController : Controller
    {
        [Route("api/graph.ql.html")]
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
