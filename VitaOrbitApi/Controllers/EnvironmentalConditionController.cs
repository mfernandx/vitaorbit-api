using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VitaOrbitApi.Data;
using VitaOrbitApi.Models;

namespace VitaOrbitApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnvironmentalConditionController : ControllerBase 
    {
        private readonly AppDbContext _context;

        public EnvironmentalConditionController(AppDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Lista todas as condições ambientais cadastradas.
        /// </summary>
        /// <response code="200">Lista de condições ambientais retornada com sucesso.</response>
        /// <returns>Lista de condições ambientais.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnvironmentalCondition>>> GetAllEnvironmentalConditions()
        {
            var conditions = await _context.EnvironmentalConditions.Include(e => e.User).ToListAsync();
            
            return Ok(conditions);
        }

        /// <summary>
        /// Busca uma condição ambiental pelo identificador.
        /// </summary>
        /// <param name="id">Identificador da condição ambiental.</param>
        /// <response code="200">Condição ambiental encontrada com sucesso.</response>
        /// <response code="400">O ID informado é inválido.</response>
        /// <response code="404">Condição ambiental não encontrada.</response>
        /// <returns>Condição ambiental encontrada.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<EnvironmentalCondition>> GetEnvironmentalConditionById(int id)
        {
            if (id <= 0)
                return BadRequest("O ID da condição ambiental deve ser maior que zero.");

            var condition = await _context.EnvironmentalConditions.Include(e => e.User).FirstOrDefaultAsync(e => e.EnvironmentalConditionId == id);

            if (condition == null)
                return NotFound("Condição ambiental não encontrada.");

            return Ok(condition);
        }

        /// <summary>
        /// Busca a condição ambiental vinculada a um usuário específico.
        /// </summary>
        /// <param name="userId">Identificador do usuário.</param>
        /// <response code="200">Condição ambiental encontrada com sucesso</response>
        /// <response code="400">O ID informado é inválido.</response>
        /// <response code="404">Condição ambiental e/ou usuário não encontrados.</response>
        /// <returns>Condição ambiental vinculada ao usuário informado.</returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<EnvironmentalCondition>> GetEnvironmentalConditionByUserId(int userId)
        {
            if (userId <= 0)
                return BadRequest("O ID do usuário deve ser maior que zero.");

            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);

            if (!userExists)
                return NotFound("Usuário não encontrado.");

            var condition = await _context.EnvironmentalConditions.FirstOrDefaultAsync(e => e.UserId == userId);

            if (condition == null)
                return NotFound("Condição ambiental não encontrada para este usuário.");

            return Ok(condition);
        }

        /// <summary>
        /// Cadastra uma nova condição ambiental de um usuário.
        /// </summary>
        /// <param name="environmentalCondition">Dados da condição ambiental.</param>
        /// <response code="201">Condição ambiental criada com sucesso.</response>
        /// <response code="400">Dados nulos ou usuário com condição ambiental já cadastrada.</response>
        /// <response code="404">Usuário não encontrado.</response>
        /// <returns>Condição ambiental cadastrada.</returns>
        [HttpPost]
        public async Task<ActionResult<EnvironmentalCondition>> CreateEnvironmentalCondition([FromBody] EnvironmentalCondition environmentalCondition)
        {
            if (environmentalCondition == null)
                return BadRequest("Os dados da condição ambiental são obrigatórios.");

            var userExists = await _context.Users.AnyAsync(u => u.UserId == environmentalCondition.UserId);

            if (!userExists)
                return NotFound("Usuário não encontrado.");

            var userAlreadyHasCondition = await _context.EnvironmentalConditions.AnyAsync(e => e.UserId == environmentalCondition.UserId);

            if (userAlreadyHasCondition)
                return BadRequest("Este usuário já possui uma condição ambiental cadastrada.");

            _context.EnvironmentalConditions.Add(environmentalCondition);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEnvironmentalConditionById),new { id = environmentalCondition.EnvironmentalConditionId },environmentalCondition);
        }

        /// <summary>
        /// Atualiza uma condição ambiental existente.
        /// </summary>
        /// <param name="id">Identificador da condição ambiental.</param>
        /// <param name="updatedCondition">Novos dados da condição ambiental.</param>
        /// <response code="200">Atualização bem sucedida</response>
        /// <response code="400">O ID informado é inválido ou os dados informados são nulos.</response>
        /// <response code="404">Condição ambiental não encontrada.</response>
        /// <returns>Mensagem de confirmação.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEnvironmentalCondition(int id,[FromBody] EnvironmentalCondition updatedCondition)
        {
            if (id <= 0)
                return BadRequest("O ID da condição ambiental deve ser maior que zero.");

            if (updatedCondition == null)
                return BadRequest("Os dados da condição ambiental são obrigatórios.");

            var condition = await _context.EnvironmentalConditions.FindAsync(id);

            if (condition == null)
                return NotFound("Condição ambiental não encontrada.");

            condition.UpdateEnvironmentalCondition(updatedCondition.ExternalTemperature, updatedCondition.Humidity, updatedCondition.Altitude, updatedCondition.AtmosphericPressure, updatedCondition.AirQuality, updatedCondition.RadiationLevel, updatedCondition.EnvironmentType);

            await _context.SaveChangesAsync();

            return Ok("Condição ambiental atualizada com sucesso.");
        }

        /// <summary>
        /// Remove uma condição ambiental pelo identificador.
        /// </summary>
        /// <param name="id">Identificador da condição ambiental.</param>
        /// <response code="200">Exclusão bem sucedida.</response>
        /// <response code="400">O ID informado é inválido.</response>
        /// <response code="404">Condição ambiental não encontrada.</response>
        /// <returns>Mensagem de confirmação.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEnvironmentalCondition(int id)
        {
            if (id <= 0)
                return BadRequest("O ID da condição ambiental deve ser maior que zero.");

            var condition = await _context.EnvironmentalConditions.FindAsync(id);

            if (condition == null)
                return NotFound("Condição ambiental não encontrada.");

            _context.EnvironmentalConditions.Remove(condition);
            await _context.SaveChangesAsync();

            return Ok("Condição ambiental deletada com sucesso.");
        }

    }
}
