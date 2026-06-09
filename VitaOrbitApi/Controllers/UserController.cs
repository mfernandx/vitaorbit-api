using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using VitaOrbitApi.Data;
using VitaOrbitApi.Models;

namespace VitaOrbitApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController: ControllerBase 
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todos os usuários cadastrados.
        /// </summary>
        /// <response code="200">Lista de usuários retornada com sucesso.</response>
        /// <returns>Lista de usuários.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _context.Users.Include(u => u.HealthRecords).ToListAsync();
            return Ok(users);
        }

        /// <summary>
        /// Busca um usuário pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do usuário.</param>
        /// <response code="200">Usuário encontrado com sucesso.</response>
        /// <response code="400">O ID informado é inválido.</response>
        /// <response code="404">Usuário não encontrado.</response>
        /// <returns>Usuário encontrado.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            if (id <= 0)
                return BadRequest("O ID do usuário deve ser maior que zero.");

            var user = await _context.Users.Include(u => u.HealthRecords).FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                return NotFound("Usuário não encontrado.");

            return Ok(user);
        }


        /// <summary>
        /// Cadastra um novo usuário no sistema.
        /// </summary>
        /// <param name="user">Dados do usuário a ser cadastrado.</param>
        /// <returns>Usuário criado.</returns>
        /// <response code="201">Usuário cadastrado com sucesso.</response>
        /// <response code="400">Dados inválidos ou e-mail já cadastrado.</response>
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest("Os dados do usuário são obrigatórios.");

            var emailAlreadyExists = await _context.Users.AnyAsync(u => u.Email == user.Email);

            if (emailAlreadyExists)
                return BadRequest("Já existe um usuário cadastrado com este e-mail.");

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }


        /// <summary>
        /// Realiza o login de um usuário no sistema.
        /// </summary>
        /// <param name="body">Objeto contendo e-mail e senha do usuário.</param>
        /// <returns>Dados básicos do usuário autenticado.</returns>
        /// <response code="200">Login realizado com sucesso.</response>
        /// <response code="400">E-mail ou senha não informados.</response>
        /// <response code="401">E-mail ou senha inválidos.</response>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] JsonElement body)
        {
            var email = body.GetProperty("email").GetString();
            var password = body.GetProperty("password").GetString();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return BadRequest("E-mail e senha são obrigatórios.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
                return Unauthorized("E-mail ou senha inválidos.");

            return Ok(new
            {
                message = "Login realizado com sucesso.",
                userId = user.UserId,
                fullName = user.FullName,
                email = user.Email
            });
        }


        /// <summary>
        /// Atualiza o e-mail de um usuário existente.
        /// </summary>
        /// <param name="id">Identificador do usuário.</param>
        /// <param name="body">Novo e-mail.</param>
        /// <response code="200">Atualização bem sucedida</response>
        /// <response code="400">O ID informado é inválido ou os dados informados são nulos.</response>
        /// <response code="404">Usuário não encontrada.</response>
        /// <returns>Mensagem de confirmação.</returns>
        [HttpPut("{id}/email")]
        public async Task<ActionResult> UpdateEmail(int id, [FromBody] JsonElement body)
        {
            if (id <= 0)
                return BadRequest("O ID do usuário deve ser maior que zero.");

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound("Usuário não encontrado.");

            var email = body.GetProperty("email").GetString();

            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("O e-mail é obrigatório.");

            user.UpdateEmail(email);

            await _context.SaveChangesAsync();

            return Ok("E-mail alterado com sucesso.");
        }


        // <summary>
        /// Atualiza o telefone de um usuário existente.
        /// </summary>
        /// <param name="id">Identificador do usuário.</param>
        /// <param name="body">Novo telefone.</param>
        /// <response code="200">Atualização bem sucedida</response>
        /// <response code="400">O ID informado é inválido ou os dados informados são nulos.</response>
        /// <response code="404">Usuário não encontrada.</response>
        /// <returns>Mensagem de confirmação.</returns>
        [HttpPut("{id}/phone")]
        public async Task<ActionResult> UpdatePhoneNumber(int id, [FromBody] JsonElement body)
        {
            if (id <= 0)
                return BadRequest("O ID do usuário deve ser maior que zero.");

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound("Usuário não encontrado.");

            var phoneNumber = body.GetProperty("phoneNumber").GetString();

            if (string.IsNullOrWhiteSpace(phoneNumber))
                return BadRequest("O telefone é obrigatório.");

            user.UpdatePhoneNumber(phoneNumber);

            await _context.SaveChangesAsync();

            return Ok("Telefone alterado com sucesso.");
        }

        // <summary>
        /// Atualiza a localização de um usuário existente.
        /// </summary>
        /// <param name="id">Identificador do usuário.</param>
        /// <param name="body">Nova localização.</param>
        /// <response code="200">Atualização bem sucedida</response>
        /// <response code="400">O ID informado é inválido ou os dados informados são nulos.</response>
        /// <response code="404">Usuário não encontrada.</response>
        /// <returns>Mensagem de confirmação.</returns>
        [HttpPut("{id}/location")]
        public async Task<ActionResult> UpdateCurrentLocation(int id, [FromBody] JsonElement body)
        {
            if (id <= 0)
                return BadRequest("O ID do usuário deve ser maior que zero.");

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound("Usuário não encontrado.");

            var currentLocation = body.GetProperty("currentLocation").GetString();

            if (string.IsNullOrWhiteSpace(currentLocation))
                return BadRequest("A localização é obrigatória.");

            user.UpdateCurrentLocation(currentLocation);

            await _context.SaveChangesAsync();

            return Ok("Localização alterada com sucesso.");
        }


        // <summary>
        /// Atualiza o contato de emergência de um usuário existente.
        /// </summary>
        /// <param name="id">Identificador do usuário.</param>
        /// <param name="body">Novo contato de emergência.</param>
        /// <response code="200">Atualização bem sucedida</response>
        /// <response code="400">O ID informado é inválido ou os dados informados são nulos.</response>
        /// <response code="404">Usuário não encontrada.</response>
        /// <returns>Mensagem de confirmação.</returns>
        [HttpPut("{id}/emergency-contact")]
        public async Task<ActionResult> UpdateEmergencyContact(int id, [FromBody] JsonElement body)
        {
            if (id <= 0)
                return BadRequest("O ID do usuário deve ser maior que zero.");

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound("Usuário não encontrado.");

            var emergencyContact = body.GetProperty("emergencyContact").GetString();

            if (string.IsNullOrWhiteSpace(emergencyContact))
                return BadRequest("O telefone de emergência é obrigatório.");

            user.UpdateEmergencyContact(emergencyContact);

            await _context.SaveChangesAsync();

            return Ok("Telefone de emergência alterado com sucesso.");
        }


        /// <summary>
        /// Remove um usuário pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do usuário.</param>
        /// <response code="200">Exclusão bem sucedida.</response>
        /// <response code="400">O ID informado é inválido.</response>
        /// <response code="404">Usuário não encontrado.</response>
        /// <returns>Mensagem de confirmação.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            if (id <= 0)
                return BadRequest("O ID do registro de sintoma deve ser maior que zero.");

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound("Usuário não encontrado.");

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return Ok("Usuário deletado com sucesso.");
        }


        

    }
}
