using InterfazUsuario.Interfaces;
using InterfazUsuario.Managers;
using Microsoft.AspNetCore.Mvc;

namespace InterfazUsuario.ApiControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RespuestasController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        private readonly IRespuestasManager _respuestasManager;

        public RespuestasController(IEmailSender emailSender, IRespuestasManager respuestasManager)
        {
            _emailSender = emailSender;
            _respuestasManager = respuestasManager;
        }

        [HttpGet("answer/{id}")]
        public IActionResult GetAnswer(int id)
        {
            var respuesta = _respuestasManager.GetPredefinedRespuesta(id);
            return Ok(new { answer = respuesta });
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveAnswers([FromBody] List<AnswerModel> respuestas)
        {
            try
            {
                await _respuestasManager.SaveRespuestas(respuestas);
                return Ok(new { message = "Respuestas guardadas correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al guardar respuestas.", details = ex.Message });
            }
        }

        [HttpGet("cargararchivo")]
        public async Task<IActionResult> CargarRespuestasTxt()
        {
            try
            {
                var respuestas = await _respuestasManager.LoadRespuestas();
                return Ok(respuestas);
            }
            catch (FileNotFoundException)
            {
                return NotFound(new { error = "No hay respuestas guardadas." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al cargar respuestas.", details = ex.Message });
            }
        }

        [HttpPost("uploadfile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                await _respuestasManager.UploadRespuestasFile(file);
                return Ok(new { message = "Archivo subido y reemplazado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al subir el archivo", details = ex.Message });
            }
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            try
            {
                await _emailSender.SendEmailAsync(
                    request.Email,
                    "Respuesta a tu pregunta",
                    request.Answer
                );
                return Ok(new { message = "Email enviado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al enviar el email", details = ex.Message });
            }
        }
    }
}


public record AnswerModel(int QuestionId, string Response);
public record EmailRequest(string Email, string Answer);