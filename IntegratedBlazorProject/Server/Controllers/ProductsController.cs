using Dapper;
using IntegratedBlazorProject.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IntegratedBlazorProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly string ConnString;

        public ProductsController()
        {
            ConnString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            using (var connection = new SqlConnection(ConnString))
            {
                var sql = "";
                // var command = connection.Query<Product>(sql).AsList();
                // Console.WriteLine(command);

                return null;
            }
        }
    }
}
