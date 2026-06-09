using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using VitaOrbitApi.Data;
using VitaOrbitApi.Models;

namespace VitaOrbitApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmergencyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmergencyController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todas as emergências cadastradas.
        /// </summary>
        /// <response code="200">Lista de emergências retornada com sucesso.</response>
        /// <returns>Lista de emergências.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Emergency>>> GetAllEmergencies()
        {
            var emergencies = await _context.Emergencies.Include(e => e.User).OrderByDescending(e => e.RequestDate).ToListAsync();

            return Ok(emergencies);
        }


        /// <summary>
        /// Busca uma emergência pelo identificador.
        /// </summary>
        /// <param name="id">Identificador da emergência.</param>
        /// <response code="200">Emergência encontrada com sucesso.</response>
        /// <response code="400">O ID informado é inválido.</response>
        /// <response code="404">Emergência não encontrada.</response>
        /// <returns>Emergência encontrada.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Emergency>> GetEmergencyById(int id)
        {
            if (id <= 0)
                return BadRequest("O ID da emergência deve ser maior que zero.");

            var emergency = await _context.Emergencies.Include(e => e.User).FirstOrDefaultAsync(e => e.EmergencyId == id);

            if (emergency == null)
                return NotFound("Emergência não encontrada.");

            return Ok(emergency);
        }



        /// <summary>
        /// Busca emergência(s) vinculada(s) a um usuário específico.
        /// </summary>
        /// <param name="userId">Identificador do usuário.</param>
        /// <response code="200">Emergência(s) encontrada(s) com sucesso.</response>
        /// <response code="400">O ID informado é inválido.</response>
        /// <response code="404">Emergência e/ou usuário não encontrados.</response>
        /// <returns>Emergência(s) vinculada(s) ao usuário informado.</returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Emergency>>> GetEmergenciesByUserId(int userId)
        {
            if (userId <= 0)
                return BadRequest("O ID do usuário deve ser maior que zero.");

            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);

            if (!userExists)
                return NotFound("Usuário não encontrado.");

            var emergencies = await _context.Emergencies.Where(e => e.UserId == userId).OrderByDescending(e => e.RequestDate).ToListAsync();

            if (emergencies == null)
                return NotFound("Emergência não encontrada para este usuário.");

            return Ok(emergencies);
        }

        /// <summary>
        /// Cadastra uma emergência de um usuário.
        /// </summary>
        /// <param name="emergency">Dados da emergência.</param>
        /// <response code="201">Emergência criada com sucesso.</response>
        /// <response code="400">Dados nulos.</response>
        /// <response code="404">Usuário não encontrado.</response>
        /// <returns>Emergência cadastrada.</returns>
        [HttpPost]
        public async Task<ActionResult<Emergency>> CreateEmergency([FromBody] Emergency emergency)
        {
            if (emergency == null)
                return BadRequest("Os dados da emergência são obrigatórios.");

            var userExists = await _context.Users
                .AnyAsync(u => u.UserId == emergency.UserId);

            if (!userExists)
                return NotFound("Usuário não encontrado.");

            _context.Emergencies.Add(emergency);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmergencyById),new { id = emergency.EmergencyId },emergency);
        }


        /// <summary>
        /// Atualiza o status de uma emergência existente.
        /// </summary>
        /// <param name="id">Identificador da emergência.</param>
        /// <param name="body">Novo status.</param>
        /// <response code="200">Atualização bem sucedida</response>
        /// <response code="400">O ID informado é inválido ou os dados informados são nulos.</response>
        /// <response code="404">Emergência não encontrada.</response>
        /// <returns>Mensagem de confirmação.</returns>
        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateEmergencyStatus(int id, [FromBody] JsonElement body)
        {
            if (id <= 0)
                return BadRequest("O ID da emergência deve ser maior que zero.");

            var emergency = await _context.Emergencies.FindAsync(id);

            if (emergency == null)
                return NotFound("Emergência não encontrada.");

            var status = body.GetProperty("status").GetString();

            if (string.IsNullOrWhiteSpace(status))
                return BadRequest("O status é obrigatório.");

            emergency.UpdateStatus(status);
            await _context.SaveChangesAsync();

            return Ok("Status da emergência atualizado com sucesso.");
        }


        /// <summary>
        /// Remove uma emergência pelo identificador.
        /// </summary>
        /// <param name="id">Identificador da emergência.</param>
        /// <response code="200">Exclusão bem sucedida.</response>
        /// <response code="400">O ID informado é inválido.</response>
        /// <response code="404">Emergência não encontrada.</response>
        /// <returns>Mensagem de confirmação.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmergency(int id)
        {
            if (id <= 0)
                return BadRequest("O ID da emergência deve ser maior que zero.");

            var emergency = await _context.Emergencies.FindAsync(id);

            if (emergency == null)
                return NotFound("Emergência não encontrada.");

            _context.Emergencies.Remove(emergency);
            await _context.SaveChangesAsync();

            return Ok("Emergência deletada com sucesso.");
        }

    }
}
