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
    public class GraphQlController : Controller
    {
        private readonly RootSchema _schema = new RootSchema();

        [Route("api/graph.ql")]
        [HttpPost]
        public JsonResult QlIndex([FromBody] GraphiQLInput input)
        {
            try
            {
                return this.Json(
                    new
                    {
                        data = this._schema.Execute(input.query, GetVariables(input))
                    }
                );
            }
            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        errors = new dynamic[] { new { message = ex.Message } }
                    }
                );
            }
        }

        private static dynamic GetVariables(GraphiQLInput input)
        {
            if (string.IsNullOrWhiteSpace(input.variables))
                return new ExpandoObject();

            return JsonConvert.DeserializeObject<ExpandoObject>(input.variables);
        }
    }
}
