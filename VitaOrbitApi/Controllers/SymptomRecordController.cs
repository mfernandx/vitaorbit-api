using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using VitaOrbitApi.Data;
using VitaOrbitApi.Models;

namespace VitaOrbitApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SymptomRecordController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SymptomRecordController(AppDbContext context)
        {
            _context = context;

        }

        private string ClassifyRisk(SymptomRecord record)
        {
            var symptom = record.SymptomName.ToLower();

            if (record.Intensity >= 9 ||
                symptom.Contains("falta de ar") ||
                symptom.Contains("dor no peito") ||
                symptom.Contains("desmaio"))
            {
                return "Crítico";
            }

            if (record.Intensity >= 7 ||
                symptom.Contains("febre") ||
                symptom.Contains("tontura") ||
                symptom.Contains("náusea") ||
                symptom.Contains("dor de cabeça"))
            {
                return "Alto";
            }

            if (record.Intensity >= 4)
            {
                return "Moderado";
            }

            return "Baixo";
        }

        /// <summary>
        /// Lista todos os registros de sintomas cadastrados.
        /// </summary>
        /// <response code="200">Lista de registros de sintomas retornada com sucesso.</response>
        /// <returns>Lista de registros de sintomas.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SymptomRecord>>> GetAllSymptomRecords()
        {
            var records = await _context.SymptomRecords.Include(s => s.User).ToListAsync();
            return Ok(records);
        }


        /// <summary>
        /// Busca um registro de sintoma pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do registro de sintoma.</param>
        /// <response code="200">Registro de sintoma encontrado com sucesso.</response>
        /// <response code="400">O ID informado é inválido.</response>
        /// <response code="404">Registro de sintoma não encontrado.</response>
        /// <returns>Registro de sintoma encontrado.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SymptomRecord>> GetSymptomRecordById(int id)
        {
            if (id <= 0)
                return BadRequest("O ID do registro de sintoma deve ser maior que zero.");

            var record = await _context.SymptomRecords.Include(s => s.User).FirstOrDefaultAsync(s => s.SymptomRecordId == id);

            if (record == null)
                return NotFound("Registro de sintoma não encontrado.");

            return Ok(record);
        }


        /// <summary>
        /// Busca registro(s) de sintoma vinculado(s) a um usuário específico.
        /// </summary>
        /// <param name="userId">Identificador do usuário.</param>
        /// <response code="200">Registro(s) de sintoma encontrado(s) com sucesso.</response>
        /// <response code="400">O ID informado é inválido.</response>
        /// <response code="404">Registro e/ou usuário não encontrados.</response>
        /// <returns>Registro(s) de sintoma vinculado(s) ao usuário informado.</returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<SymptomRecord>>> GetSymptomRecordsByUserId(int userId)
        {
            if (userId <= 0)
                return BadRequest("O ID do usuário deve ser maior que zero.");

            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);

            if (!userExists)
                return NotFound("Usuário não encontrado.");

            var records = await _context.SymptomRecords.Where(s => s.UserId == userId).OrderByDescending(s => s.RegisteredAt).ToListAsync();

            if (records == null)
                return NotFound("Registro de sintoma não encontrado para este usuário.");

            return Ok(records);
        }


        /// <summary>
        /// Cadastra um novo registro de sintoma de um usuário.
        /// </summary>
        /// <param name="healthRecord">Dados do registro de sintoma.</param>
        /// <response code="201">Registro de sintoma criado com sucesso.</response>
        /// <response code="400">Dados nulos.</response>
        /// <response code="404">Usuário não encontrado.</response>
        /// <returns>Registro de sintoma cadastrado.</returns>
        [HttpPost]
        public async Task<ActionResult<SymptomRecord>> CreateSymptomRecord([FromBody] SymptomRecord symptomRecord)
        {
            if (symptomRecord == null)
                return BadRequest("Os dados do registro de sintoma são obrigatórios.");

            var userExists = await _context.Users.AnyAsync(u => u.UserId == symptomRecord.UserId);

            if (!userExists)
                return NotFound("Usuário não encontrado.");

            symptomRecord.UpdateRiskClassification(ClassifyRisk(symptomRecord));

            _context.SymptomRecords.Add(symptomRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSymptomRecordById), new { id = symptomRecord.SymptomRecordId }, symptomRecord);
        }


        /// <summary>
        /// Remove um registro de sintoma pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do registro de sintoma.</param>
        /// <response code="200">Exclusão bem sucedida.</response>
        /// <response code="400">O ID informado é inválido.</response>
        /// <response code="404">Registro de sintoma não encontrado.</response>
        /// <returns>Mensagem de confirmação.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSymptomRecord(int id)
        {
            if (id <= 0)
                return BadRequest("O ID do registro de sintoma deve ser maior que zero.");

            var record = await _context.SymptomRecords.FindAsync(id);

            if (record == null)
                return NotFound("Registro de sintoma não encontrado.");

            _context.SymptomRecords.Remove(record);
            await _context.SaveChangesAsync();

            return Ok("Registro de sintoma deletado com sucesso."); ;
        }
    }



}

