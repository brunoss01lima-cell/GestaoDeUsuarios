using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using GestaoDeUsuarios.Models;

namespace GestaoDeUsuarios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosControler : Controller
    {
        private string _connectionString = "Data Source=C:\\Users\\Bruno\\Downloads\\Nova pasta\\sqlite-tools-win-x64-3530400\\Empresa1.db";

        [HttpGet]
        public IActionResult GetAll()
        {
            var usuarios = new List<Usuario>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var sql = "SELECT Id, Nome, Email, Idade FROM usuarios";
            using var command = new SqliteCommand(sql, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                usuarios.Add(new Usuario
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Email = reader.GetString(2),
                    Idade = reader.GetInt32(3)

                });
            }

            return Ok(usuarios);
        }
        [HttpPost]
        public IActionResult Create([FromBody] Usuario usuario)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var sql = $"INSERT INTO usuarios (Nome, Email, Idade) VALUES ('{usuario.Nome}', '{usuario.Email}', {usuario.Idade})";

            using var command = new SqliteCommand(sql, connection);

            command.ExecuteNonQuery();

            return Ok(new { mensagem = "Usuário cadastrado com sucesso!" });
        }
    }
}