using Dapper;
using IntegratedBlazorProject.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection.Metadata;

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

        [HttpGet("count")]
        public async Task<ActionResult<int>> Count()
        {
            using (var connection = new SqlConnection(ConnString))
            {
                var sql = "SELECT COUNT(*) FROM [ProductsProject].[dbo].[Products];";
                int result;

                try
                {
                    result = await connection.ExecuteScalarAsync<int>(sql);
                    return Ok(result);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get([FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            using (var connection = new SqlConnection(ConnString))
            {
                var sqlProducts = "SELECT p.ProductId, p.Name, p.Description, p.Price FROM [ProductsProject].[dbo].[Products] p " +
                    $"ORDER BY p.Name OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
                var sqlCategoriesIds = "SELECT p.FK_CategoryId FROM [ProductsProject].[dbo].[Products] p " +
                    $"ORDER BY p.Name OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
                var sqlCategories = "SELECT * FROM [ProductsProject].[dbo].[Categories] c";
                IEnumerable<Product> products;
                IEnumerable<Category> categories;
                IEnumerable<Guid> categoriesIds;

                try
                {
                    products = await connection.QueryAsync<Product>(sqlProducts);
                    categories = await connection.QueryAsync<Category>(sqlCategories);
                    categoriesIds = await connection.QueryAsync<Guid>(sqlCategoriesIds);

                    for (int i = 0; i < products.Count(); i++)
                    {
                        foreach (Category category in categories)
                        {
                            if (category.CategoryId == categoriesIds.ElementAt(i))
                            {
                                products.ElementAt(i).Category = category;
                            }
                        }
                    }

                    return Ok(products);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add(Product product)
        {
            using (var connection = new SqlConnection(ConnString))
            {
                var sqlInsert = "";
                var sqlCategories = "SELECT c.CategoryId, c.Name FROM [ProductsProject].[dbo].[Categories] c";
                bool categoryExists = false;
                IEnumerable<Category> categories;
                try
                {
                    categories = await connection.QueryAsync<Category>(sqlCategories);

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
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(Product product)
        {
            using (var connection = new SqlConnection(ConnString))
            {
                var sqlUpdate = "";
                var sqlCategories = "SELECT c.CategoryId, c.Name FROM [ProductsProject].[dbo].[Categories] c";
                bool categoryExists = false;
                IEnumerable<Category> categories;

                try
                {
                    categories = await connection.QueryAsync<Category>(sqlCategories);

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
                    return Ok();
                }
                catch (Exception e) 
                { 
                    return BadRequest(e.Message);
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            using (var connection = new SqlConnection(ConnString))
            {
                var sql = $"DELETE FROM [ProductsProject].[dbo].[Products] WHERE ProductId = '{id.ToString()}'";
                
                try
                {
                    await connection.ExecuteAsync(sql);
                    return Ok();
                }
                catch (SqlException e)
                {
                    return BadRequest(e.Message);
                }
            }
        }
    }
}
