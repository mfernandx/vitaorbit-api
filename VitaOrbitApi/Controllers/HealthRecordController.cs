using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VitaOrbitApi.Data;
using VitaOrbitApi.Models;

namespace VitaOrbitApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthRecordController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HealthRecordController(AppDbContext context)
        {
            _context = context;

        }

        private string ClassifyRisk(HealthRecord record)
        {
            if (record.OxygenSaturation < 90 || record.BodyTemperature >= 39 || record.HeartRate > 130)
            {
                return "Crítico";
            }

            if (record.OxygenSaturation < 94 || record.BodyTemperature >= 38 || record.HeartRate > 110)
            {
                return "Alto";
            }

            if (record.BodyTemperature >= 37.5m || record.HeartRate > 100 || record.SleepHours < 5)
            {
                return "Moderado";
            }

            return "Baixo";
        }

        /// <summary>
        /// Lista todos os registros de saúde cadastrados.
        /// </summary>
        /// <response code="200">Lista de registros de saúde retornada com sucesso.</response>
        /// <returns>Lista de registros de saúde.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HealthRecord>>> GetAllHealthRecords()
        {
            var records = await _context.HealthRecords.Include(h => h.User).ToListAsync();
            return Ok(records);
        }


        /// <summary>
        /// Busca um registro de saúde pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do registro de saúde.</param>
        /// <response code="200">Registro de saúde encontrado com sucesso.</response>
        /// <response code="400">O ID informado é inválido.</response>
        /// <response code="404">Registro de saúde não encontrado.</response>
        /// <returns>Registro de saúde encontrado.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<HealthRecord>> GetHealthRecordById(int id)
        {
            if (id <= 0)
                return BadRequest("O ID do registro de saúde deve ser maior que zero.");

            var record = await _context.HealthRecords.Include(h => h.User).FirstOrDefaultAsync(h => h.HealthRecordId == id);

            if (record == null)
                return NotFound("Registro de saúde não encontrado.");

            return Ok(record);
        }


        /// <summary>
        /// Busca registro(s) de saúde vinculado(s) a um usuário específico.
        /// </summary>
        /// <param name="userId">Identificador do usuário.</param>
        /// <response code="200">Registro(s) de saúde encontrado(s) com sucesso.</response>
        /// <response code="400">O ID informado é inválido.</response>
        /// <response code="404">Registro e/ou usuário não encontrados.</response>
        /// <returns>Registro(s) de saúde vinculado(s) ao usuário informado.</returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<HealthRecord>>> GetHealthRecordsByUserId(int userId)
        {
            if (userId <= 0)
                return BadRequest("O ID do usuário deve ser maior que zero.");

            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);

            if (!userExists)
                return NotFound("Usuário não encontrado.");

            var records = await _context.HealthRecords.Where(h => h.UserId == userId).OrderByDescending(h => h.RegisteredAt).ToListAsync();

            if (records == null)
                return NotFound("Registro de saúde não encontrado para este usuário.");

            return Ok(records);
        }


        /// <summary>
        /// Cadastra um novo registro de saúde de um usuário.
        /// </summary>
        /// <param name="healthRecord">Dados do registro de saúde.</param>
        /// <response code="201">Registro de saúde criado com sucesso.</response>
        /// <response code="400">Dados nulos.</response>
        /// <response code="404">Usuário não encontrado.</response>
        /// <returns>Registro de saúde cadastrado.</returns>
        [HttpPost]
        public async Task<ActionResult<HealthRecord>> CreateHealthRecord([FromBody] HealthRecord healthRecord)
        {
            if (healthRecord == null)
                return BadRequest("Os dados do registro de saúde são obrigatórios.");

            var userExists = await _context.Users.AnyAsync(u => u.UserId == healthRecord.UserId);

            if (!userExists)
                return NotFound("Usuário não encontrado.");

            healthRecord.UpdateRiskClassification(ClassifyRisk(healthRecord));

            _context.HealthRecords.Add(healthRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHealthRecordById),new { id = healthRecord.HealthRecordId },healthRecord);
        }

        /// <summary>
        /// Remove um registro de saúde pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do registro de saúde.</param>
        /// <response code="200">Exclusão bem sucedida.</response>
        /// <response code="400">O ID informado é inválido.</response>
        /// <response code="404">Registro de saúde não encontrado.</response>
        /// <returns>Mensagem de confirmação.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHealthRecord(int id)
        {
            if (id <= 0)
                return BadRequest("O ID do registro de saúde deve ser maior que zero.");

            var record = await _context.HealthRecords.FindAsync(id);

            if (record == null)
                return NotFound("Registro de saúde não encontrado.");

            _context.HealthRecords.Remove(record);
            await _context.SaveChangesAsync();

            return Ok("Registro de saúde deletado com sucesso."); ;
        }
    }
}
