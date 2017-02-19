using System;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Skimia.Extensions.GraphQl.Schema;
using Skimia.Extensions.GraphQl.Models;
using Microsoft.AspNetCore.Cors;

namespace Skimia.Extensions.GraphQl.Controllers
{
    public class GraphQlController : Controller
    {
        public GraphQlController(RootSchema schema)
        {
            _schema = schema;
        }

        private readonly RootSchema _schema;

        [Route("api/graph.ql")]
        [HttpPost]
        [EnableCors("AllowAnyOriginHeadersAndMethods")]
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
