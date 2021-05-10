using System.ComponentModel.DataAnnotations;

namespace NewShop.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatorio.")]
        [MaxLength(20, ErrorMessage = "Este campo deve conter entre 3 a 20 caracteres.")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 a 20 caracteres.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatorio.")]
        [MaxLength(20, ErrorMessage = "Este campo deve conter entre 3 a 20 caracteres.")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 a 20 caracteres.")]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}