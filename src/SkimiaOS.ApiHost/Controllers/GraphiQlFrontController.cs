using Microsoft.AspNetCore.Mvc;

namespace Skimia.Extensions.GraphQl.Controllers
{
    public class GraphiQlFrontController : Controller
    {
        [Route("api/graph.ql.html")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
