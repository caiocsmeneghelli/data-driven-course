using System.ComponentModel.DataAnnotations;

namespace NewShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(60, ErrorMessage = "Este campo deve ter no maximo 60 caracteres.")]
        [MinLength(3, ErrorMessage = "Este campo deve ter no minimo 3 caracteres.")]
        public string Title { get; set; }

        [MaxLength(1024, ErrorMessage = "Este campo deve conter no maximo 1024 caracteres.")]
        public string Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Categoria inválida.")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}