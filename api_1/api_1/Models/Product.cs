namespace api_1.Models
{
    public class Product
    {
        // Aqui é o que é usado para obter os dados da db
        // tem que ter todas os campos que a db tem
        // as variaveis deste model não precisam de ter o mesmo nome dos da db

        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Brand { get; set; } = "";

        public string Category { get; set; } = "";

        public decimal Price { get; set; } 

        public string Description { get; set; } = "";

        public DateTime CreatedAt { get; set; }
    }
}
