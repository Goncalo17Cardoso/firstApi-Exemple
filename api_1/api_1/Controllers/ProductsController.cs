using api_1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace api_1.Controllers
{
    // Aqui é onde as operações (querys) são feitas


    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly string connectionString; // criar esta string para a ligação à db

        // escrever ctor e depois clicar no tab, ele cria esta classe constructor
        public ProductsController(IConfiguration configuration /*precisa deste objeto para funfar*/)
        {
            // inicializar a condiguração da db
            connectionString = configuration["ConnectionStrings:SqlServerDb"] ?? ""; // caso deia erro a conectar o valor default será MERDA NENHUMA 
        }

        [HttpPost] // http para enviar dados
        public IActionResult CreateProduct(ProductDto productDto /* Vai enviar um objeto do tipo ProductDto */)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString /* passamos a string de ligação à db */ )) // vai dar erro e clicamos com o botão direito para instalar a package do sql
                {
                    connection.Open(); // ligar à db

                    // fazer uma query para inserir dados
                    string sql = "INSERT INTO products " +
                                "(name, brand, category, price, description) VALUES " +
                                "(@name, @brand, @category, @price, @description)";

                    //criar os parametros para corrigir o sql injection
                    using (var command = new SqlCommand(sql, connection /* recebe a query feita em cima e a conexão à db */))
                    {

                        // parametros usados na query
                        command.Parameters.AddWithValue("@name", productDto.Name);
                        command.Parameters.AddWithValue("@brand", productDto.Brand);
                        command.Parameters.AddWithValue("@category", productDto.Category);
                        command.Parameters.AddWithValue("@price", productDto.Price);
                        command.Parameters.AddWithValue("@description", productDto.Description);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("Product" /* Ao que se relaciona o problema */, "Sorry, but we have an exception" /* Mensagem de erro */);
                return BadRequest(ModelState); /* Devolve um badRequest com o problema definido em cima */
            }

            return Ok(); // devolve uma resposta
        }

        [HttpGet] // http para obter dados
        public IActionResult GetProducts()
        {
            List<Product> products = new List<Product>(); // cria uma lista (vetor) dos dados devolvidos de cada obejto de products. lista chama-se products do tipo Product criado nos models e inicia-la
        
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM products";
                    using (var command = new SqlCommand(sql, connection /* recebe a query feita em cima e a conexão à db */))
                    {
                        using (var reader = command.ExecuteReader()) // cria um reader (leitor) e executa
                        {
                            while (reader.Read()) // loop para criar varios objetos com os diversos dados enquanto existir dados
                            {
                                Product product = new Product(); // cria um objeto 

                                // o objeto product é preenchido conforme o que o reader lê (os parenteses no final de cada linha é a coluna de cada campo da tabela)
                                product.Id = reader.GetInt32(0);
                                product.Name = reader.GetString(1);
                                product.Brand = reader.GetString(2);
                                product.Category = reader.GetString(3);
                                product.Price = reader.GetDecimal(4);
                                product.Description = reader.GetString(5);
                                product.CreatedAt = reader.GetDateTime(6);

                                // adicionar os dados que foram colocados no obejto product para a lista products
                                products.Add(product);
                            }
                        }
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("Product" /* Ao que se relaciona o problema */, "Sorry, but we have an exception" /* Mensagem de erro */);
                return BadRequest(ModelState); /* Devolve um badRequest com o problema definido em cima */
            }

            return Ok(products); // devolve uma resposta com a lista 
        }

        [HttpGet("{id}")] // http para obter dados pelo id 
        public IActionResult GetProduct(int id)
        {
            Product product = new Product();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM products WHERE id=@id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // se o produto com o id existir ele preenche o objeto, caso contrario e diz que não encontrou merda nenhuma
                            {
                                product.Id = reader.GetInt32(0);
                                product.Name = reader.GetString(1);
                                product.Brand = reader.GetString(2);
                                product.Category = reader.GetString(3);
                                product.Price = reader.GetDecimal(4);
                                product.Description = reader.GetString(5);
                                product.CreatedAt = reader.GetDateTime(6);
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                    }

                }
            }
            catch
            {
                ModelState.AddModelError("Product" /* Ao que se relaciona o problema */, "Sorry, but we have an exception" /* Mensagem de erro */);
                return BadRequest(ModelState); /* Devolve um badRequest com o problema definido em cima */
            }

            return Ok(product);
        }

        [HttpPut("{id}")] // http para atualizar o produto com x id
        public IActionResult UpdateProduct(int id, ProductDto productDto)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "UPDATE products SET name=@name, brand=@brand, category=@category, " +
                                "price=@price, description=@description WHERE id=@id";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", productDto.Name);
                        command.Parameters.AddWithValue("@brand", productDto.Brand);
                        command.Parameters.AddWithValue("@category", productDto.Category);
                        command.Parameters.AddWithValue("@price", productDto.Price);
                        command.Parameters.AddWithValue("@description", productDto.Description);
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("Product" /* Ao que se relaciona o problema */, "Sorry, but we have an exception" /* Mensagem de erro */);
                return BadRequest(ModelState); /* Devolve um badRequest com o problema definido em cima */
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                using ( var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "DELETE FROM products WHERE id=@id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("Product" /* Ao que se relaciona o problema */, "Sorry, but we have an exception" /* Mensagem de erro */);
                return BadRequest(ModelState); /* Devolve um badRequest com o problema definido em cima */
            }

            return Ok();
        }
    }
}
