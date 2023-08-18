using System.ComponentModel.DataAnnotations;

namespace IntegratedBlazorProject.Shared.Model
{
    public class User
    {
        [Required(ErrorMessage = "*Campo Obrigatório")]
        [MaxLength(50, ErrorMessage = "*Tamanho Máximo de 50 Caracteres")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "*Campo Obrigatório")]
        [Range(18,200, ErrorMessage = "*Idade Mínima de 18 Anos")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "*Campo Obrigatório")]
        [MaxLength(50, ErrorMessage = "*Tamanho Máximo de 50 Caracteres")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "*Campo Obrigatório")]
        [MinLength(10, ErrorMessage = "*A Senha Deve Ter Entre 10 e 50 Caracteres")]
        [MaxLength(50, ErrorMessage = "*A Senha Deve Ter Entre 10 e 50 Caracteres")]
        public string? Password { get; set; }

        public User() { }
        public User(string name, int age, string email, string password) 
        {
            this.Name = name;
            this.Age = age;
            this.Email = email;
            this.Password = password;
        }
    }
}
