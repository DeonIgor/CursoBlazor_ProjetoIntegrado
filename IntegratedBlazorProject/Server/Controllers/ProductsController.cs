using Dapper;
using IntegratedBlazorProject.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

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
                var sqlProducts = "SELECT p.ProductId, p.Name, p.Description, p.Price FROM [ProductsProject].[dbo].[Products] p";
                IEnumerable<Product> products = connection.Query<Product>(sqlProducts).AsList();

                var sqlCategories = "SELECT * FROM [ProductsProject].[dbo].[Categories] c";
                IEnumerable<Category> categories = connection.Query<Category>(sqlCategories).AsList();
                
                var sqlCategoriesIds = "SELECT p.FK_CategoryId FROM [ProductsProject].[dbo].[Products] p";
                IEnumerable<Guid> categoriesIds = connection.Query<Guid>(sqlCategoriesIds).AsList();

                for(int i = 0; i < products.Count(); i++)
                {
                    foreach (Category category in categories)
                    {
                        if (category.CategoryId == categoriesIds.ElementAt(i))
                        {
                            products.ElementAt(i).Category = category;
                        }
                    }
                }

                return products;
            }
        }

        [HttpPost]
        public void Add(Product product)
        {
            using (var connection = new SqlConnection(ConnString))
            {
                var sqlInsert = "";
                var sqlCategories = "SELECT c.CategoryId FROM [ProductsProject].[dbo].[Categories] c";
                List<Guid> categories = connection.Query<Guid>(sqlCategories).AsList();

                if (categories.Contains(product.Category.CategoryId))
                {
                    sqlInsert = $"INSERT INTO [ProductsProject].[dbo].[Products] " +
                        $"VALUES('{product.ProductId}', " +
                        $"'{product.Name}', " +
                        $"'{product.Description}', " +
                        $"{product.Price}, " +
                        $"'{product.Category.CategoryId}')";
                }
                else
                {
                    sqlInsert = $"INSERT INTO [ProductsProject].[dbo].[Categories] " +
                        $"VALUES('{product.Category.CategoryId}', '{product.Category.Name}');\n" +
                        $"INSERT INTO [ProductsProject].[dbo].[Products] " +
                        $"VALUES('{product.ProductId}', " +
                        $"'{product.Name}', " +
                        $"'{product.Description}', " +
                        $"{product.Price}, " +
                        $"'{product.Category.CategoryId}');";
                }

                connection.Execute(sqlInsert);
            }
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            using (var connection = new SqlConnection(ConnString))
            {
                var sql = $"DELETE FROM [ProductsProject].[dbo].[Products] WHERE ProductId = '{id.ToString()}'";
                connection.Execute(sql);
            }
        }
    }
}
