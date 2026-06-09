using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VitaOrbitApi.Data;
using VitaOrbitApi.Models;

namespace VitaOrbitApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AlertController(AppDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Lista todos os alertas cadastrados.
        /// </summary>
        /// <response code="200">Lista de alertas retornada com sucesso.</response>
        /// <returns>Lista de alertas.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alert>>> GetAllAlerts()
        {
            var alerts = await _context.Alerts.Include(a => a.User).Include(a => a.HealthRecord).Include(a => a.SymptomRecord).OrderByDescending(a => a.RegisteredAt).ToListAsync();
            
            return Ok(alerts);
        }


        /// <summary>
        /// Busca um alerta pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do alerta.</param>
        /// <response code="200">Alerta encontrado com sucesso.</response>
        /// <response code="400">O ID informado é inválido.</response>
        /// <response code="404">Alerta não encontrado.</response>
        /// <returns>Alerta encontrado.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Alert>> GetAlertById(int id)
        {
            if (id <= 0)
                return BadRequest("O ID do alerta deve ser maior que zero."); 

            var alert = await _context.Alerts.Include(a => a.User).Include(a => a.HealthRecord).Include(a => a.SymptomRecord).FirstOrDefaultAsync(a => a.AlertId == id);

            if (alert == null)
                return NotFound("Alerta não encontrado.");

            return Ok(alert);
        }


        /// <summary>
        /// Busca alerta(s) vinculado(s) a um usuário específico.
        /// </summary>
        /// <param name="userId">Identificador do usuário.</param>
        /// <response code="200">Alerta(s) encontrado(s) com sucesso.</response>
        /// <response code="400">O ID informado é inválido.</response>
        /// <response code="404">Alerta e/ou usuário não encontrados.</response>
        /// <returns>Alerta(s) vinculado(s) ao usuário informado.</returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Alert>>> GetAlertsByUserId(int userId)
        {
            if (userId <= 0)
                return BadRequest("O ID do usuário deve ser maior que zero.");

            var userExists = await _context.Users.AnyAsync(u => u.UserId == userId);

            if (!userExists)
                return NotFound("Usuário não encontrado.");

            var alerts = await _context.Alerts.Where(a => a.UserId == userId).Include(a => a.HealthRecord).Include(a => a.SymptomRecord).OrderByDescending(a => a.RegisteredAt).ToListAsync();

            if (alerts == null)
                return NotFound("Alerta não encontrado para este usuário.");

            return Ok(alerts);
        }


        /// <summary>
        /// Cadastra um alerta vinculado a um registro de saúde ou sintoma.
        /// </summary>
        /// <param name="emergency">Dados do alerta.</param>
        /// <response code="201">Alerta criado com sucesso.</response>
        /// <response code="400">O alerta não pode ter valores nulos, e deve estar vinculado a apenas um tipo de registro, que deve estar vinculado a um usuário.</response>
        /// <response code="404">Usuário não encontrado.</response>
        /// <returns>Alerta cadastrado.</returns>
        [HttpPost]
        public async Task<ActionResult<Alert>> CreateAlert([FromBody] Alert alert)
        {
            if (alert == null)
                return BadRequest("Os dados do alerta são obrigatórios.");

            var userExists = await _context.Users.AnyAsync(u => u.UserId == alert.UserId);

            if (!userExists)
                return NotFound("Usuário não encontrado.");

            if (alert.HealthRecordId == null && alert.SymptomRecordId == null)
                return BadRequest("O alerta deve estar vinculado a um registro de saúde ou a um registro de sintoma.");

            if (alert.HealthRecordId != null && alert.SymptomRecordId != null)
                return BadRequest("O alerta deve estar vinculado a apenas um tipo de registro.");

            if (alert.HealthRecordId != null)
            {
                var healthRecordExists = await _context.HealthRecords.AnyAsync(h => h.HealthRecordId == alert.HealthRecordId && h.UserId == alert.UserId);

                if (!healthRecordExists)
                    return BadRequest("Registro de saúde não encontrado para este usuário.");
            }

            if (alert.SymptomRecordId != null)
            {
                var symptomRecordExists = await _context.SymptomRecords.AnyAsync(s => s.SymptomRecordId == alert.SymptomRecordId && s.UserId == alert.UserId);

                if (!symptomRecordExists)
                    return BadRequest("Registro de sintoma não encontrado para este usuário.");
            }

            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAlertById), new { id = alert.AlertId }, alert);
        }

        /// <summary>
        /// Remove um alerta pelo identificador.
        /// </summary>
        /// <param name="id">Identificador do alerta.</param>
        /// <response code="200">Exclusão bem sucedida.</response>
        /// <response code="400">O ID informado é inválido.</response>
        /// <response code="404">Alerta não encontrado.</response>
        /// <returns>Mensagem de confirmação.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAlert(int id)
        {
            if (id <= 0)
                return BadRequest("O ID do alerta deve ser maior que zero.");

            var alert = await _context.Alerts.FindAsync(id);

            if (alert == null)
                return NotFound("Alerta não encontrado.");

            _context.Alerts.Remove(alert);
            await _context.SaveChangesAsync();

            return Ok("Alerta deletado com sucesso.");
        }



    }
}
