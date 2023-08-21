using System.ComponentModel.DataAnnotations;

namespace IntegratedBlazorProject.Shared.Model
{
    public class Product
    {
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "*Campo Obrigatório")]
        [MaxLength(50, ErrorMessage = "*Máximo de 50 Caracteres")]
        public string? Name { get; set; }

        [MaxLength(150, ErrorMessage = "*Máximo de 150 Caracteres")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "*Campo Obrigatório")]
        [Range(0.0, float.MaxValue, ErrorMessage = "*O Preço Deve Ser Um Valor Positivo")]
        public float? Price { get; set; }

        public Category Category { get; set; }

        public Product() 
        {
            this.ProductId = Guid.NewGuid() ;
            this.Category = new Category();
        }

        public Product(string name, string description, float price, Category category)
        {
            this.ProductId = Guid.NewGuid();
            this.Name = name;
            this.Description = description;
            this.Price = price;
            this.Category = category;
        }

        public Product(string name, float price, Category category)
        {
            this.ProductId = Guid.NewGuid();
            this.Name = name;
            this.Price = price;
            this.Category = category;
        }
    }
}
