using System.ComponentModel.DataAnnotations;

namespace NewShop.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(60, ErrorMessage = "Este campo deve ter o tamanho maximo de 60 caracteres.")]
        [MinLength(3, ErrorMessage = "Este campo deve ter o tamanho minimo de 3 caracteres.")]
        public string Title { get; set; }
    }
}