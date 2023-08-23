using System.ComponentModel.DataAnnotations;

namespace IntegratedBlazorProject.Shared.Model
{
    public class Category
    {
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "*Campo Obrigatório")]
        [MaxLength(50, ErrorMessage = "*Máximo de 50 Caracteres")]
        public string Name { get; set; }

        public Category() 
        {
            this.CategoryId = Guid.NewGuid();
            this.Name = "---";
        }

        public Category(string name)
        {
            this.CategoryId = Guid.NewGuid();
            this.Name = name;
        }
    }
}
