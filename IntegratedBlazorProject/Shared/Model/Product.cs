using System.ComponentModel.DataAnnotations;

namespace IntegratedBlazorProject.Shared.Model
{
    public class Product
    {
        [Required(ErrorMessage = "*Campo Obrigatório")]
        [MaxLength(50, ErrorMessage = "*Máximo de 50 Caracteres")]
        public string? Name { get; set; }

        [MaxLength(150, ErrorMessage = "*Máximo de 150 Caracteres")]
        public string? Description { get; set; }
        public string? ProductId { get; set; }

        [Required(ErrorMessage = "*Campo Obrigatório")]
        [Range(0.0, float.MaxValue, ErrorMessage = "*O Preço Deve Ser Um Valor Positivo")]
        public float? Price { get; set; }

        public Product() 
        {
            ProductId = Guid.NewGuid().ToString();
        }

        public Product(string name, string description, float price)
        {
            Name = name;
            Description = description;
            ProductId = Guid.NewGuid().ToString();
            Price = price;
        }
    }
}
