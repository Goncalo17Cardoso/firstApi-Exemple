using System.ComponentModel.DataAnnotations;

namespace api_1.Models
{
    public class ProductDto
        // Aqui é as propriedades que é susposto recebermos do utilizador para guardarmos as merdasG
    {
        [Required] // para ser obrigatório todos os campos obterem valores
        public string Name { get; set; } = ""; // caso não seja preenchido será por default ""

        [Required]
        public string Brand { get; set; } = "";

        [Required]
        public string Category { get; set; } = "";

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; } = "";
    }
}
