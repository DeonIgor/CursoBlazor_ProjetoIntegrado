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
                var sqlCategories = "SELECT c.CategoryId, c.Name FROM [ProductsProject].[dbo].[Categories] c";
                List<Category> categories = connection.Query<Category>(sqlCategories).AsList();
                bool categoryExists = false;

                foreach (Category category in categories)
                {
                    if ((category.Name == product.Category.Name))
                    {
                        product.Category.CategoryId = category.CategoryId;
                        categoryExists = true;
                    }
                    else if (category.CategoryId == product.Category.CategoryId)
                    {
                        product.Category.CategoryId = Guid.NewGuid();
                        categoryExists = false;
                    }
                }

                if (categoryExists)
                {
                    sqlInsert = $"INSERT INTO [ProductsProject].[dbo].[Products] " +
                        $"VALUES('{product.ProductId}', " +
                        $"'{product.Name}', " +
                        $"'{product.Description}', " +
                        $"'{product.Price.ToString().Replace(',', '.')}', " +
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
                        $"'{product.Price.ToString().Replace(',', '.')}', " +
                        $"'{product.Category.CategoryId}');";
                }

                connection.Execute(sqlInsert);
            }
        }

        [HttpPut]
        public void Update(Product product)
        {
            using (var connection = new SqlConnection(ConnString))
            {
                var sqlUpdate = "";
                var sqlCategories = "SELECT c.CategoryId, c.Name FROM [ProductsProject].[dbo].[Categories] c";
                List<Category> categories = connection.Query<Category>(sqlCategories).AsList();
                bool categoryExists = false;

                foreach (Category category in categories)
                {
                    if ((category.Name == product.Category.Name))
                    {
                        product.Category.CategoryId = category.CategoryId;
                        categoryExists = true;
                    }
                    else if (category.CategoryId == product.Category.CategoryId)
                    {
                        product.Category.CategoryId = Guid.NewGuid();
                        categoryExists = false;
                    }
                }

                if (categoryExists)
                {
                    sqlUpdate = $"UPDATE [ProductsProject].[dbo].[Products] SET " +
                        $"Name = '{product.Name}', " +
                        $"Description = '{product.Description}', " +
                        $"Price = '{product.Price.ToString().Replace(',', '.')}', " +
                        $"FK_CategoryId = '{product.Category.CategoryId}'\n" +
                        $"WHERE ProductId = '{product.ProductId}';" +

                        $"UPDATE [ProductsProject].[dbo].[Categories] SET " +
                        $"Name = '{product.Category.Name}'\n" +
                        $"WHERE CategoryId = '{product.Category.CategoryId}';";
                }
                else
                {
                    sqlUpdate = $"INSERT INTO [ProductsProject].[dbo].[Categories] " +
                        $"VALUES('{product.Category.CategoryId}', '{product.Category.Name}');\n" +

                        $"UPDATE [ProductsProject].[dbo].[Products] SET " +
                        $"Name = '{product.Name}', " +
                        $"Description = '{product.Description}', " +
                        $"Price = '{product.Price.ToString().Replace(',', '.')}', " +
                        $"FK_CategoryId = '{product.Category.CategoryId}'\n" +
                        $"WHERE ProductId = '{product.ProductId}';";
                }

                connection.Execute(sqlUpdate);
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
