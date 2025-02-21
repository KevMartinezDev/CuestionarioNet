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


//[ApiController]
//[Route("api/[controller]")]
//public class RespuestasController : ControllerBase
//{
//    private readonly IEmailSender _emailSender;

//    public RespuestasController(IEmailSender emailSender)
//    {
//        _emailSender = emailSender;
//    }

//    [HttpGet("answer/{id}")]
//    public IActionResult GetAnswer(int id)
//    {
//        var answer = GetPredefinedAnswer(id);
//        return Ok(new { answer });
//    }

//    [HttpPost("save")]
//    public IActionResult SaveAnswers([FromBody] List<AnswerModel> answers)
//    {
//        System.IO.File.WriteAllLines("respuestas.txt", answers.Select(a => $"{a.QuestionId}|{a.Response}"));
//        return Ok();
//    }

//    [HttpPost("send-email")]
//    public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
//    {
//        try
//        {
//            await _emailSender.SendEmailAsync(
//                request.Email,
//                "Respuesta a tu pregunta",
//                request.Answer
//            );
//            return Ok(new { message = "Email enviado correctamente" });
//        }
//        catch (Exception ex)
//        {
//            return StatusCode(500, new { error = "Error al enviar el email", details = ex.Message });
//        }
//    }

//    [HttpPost("guardararchivo")]
//    public IActionResult GuardarRespuestasTxt([FromBody] List<AnswerModel> answers)
//    {
//        try
//        {
//            string path = "C:\\archivostxt\\respuestas.txt"; // Ruta del archivo
//            string directory = Path.GetDirectoryName(path);

//            // Verificar si el directorio existe, si no, crearlo
//            if (!Directory.Exists(directory))
//                Directory.CreateDirectory(directory);

//            // Guardar respuestas en el archivo
//            System.IO.File.WriteAllLines(path, answers.Select(a => $"{a.QuestionId}|{a.Response}"));

//            return Ok(new { message = "Respuestas guardadas correctamente." });
//        }
//        catch (Exception ex)
//        {
//            return StatusCode(500, new { error = "Error al guardar respuestas.", details = ex.Message });
//        }
//    }

//    [HttpGet("cargararchivo")]
//    public IActionResult CargarRespuestasTxt()
//    {
//        try
//        {
//            string path = "C:\\archivostxt\\respuestas.txt"; // Ruta del archivo

//            if (!System.IO.File.Exists(path))
//                return NotFound(new { error = "No hay respuestas guardadas." });

//            var answers = System.IO.File.ReadAllLines(path)
//                .Select(line => line.Split('|'))
//                .Where(parts => parts.Length == 2)
//                .Select(parts => new AnswerModel(int.Parse(parts[0]), parts[1]))
//                .ToList();

//            return Ok(answers);
//        }
//        catch (Exception ex)
//        {
//            return StatusCode(500, new { error = "Error al cargar respuestas.", details = ex.Message });
//        }
//    }


//    [HttpPost("uploadfile")]
//    public async Task<IActionResult> UploadFile(IFormFile file)
//    {
//        try
//        {
//            string path = "C:\\archivostxt\\respuestas.txt";
//            string directory = Path.GetDirectoryName(path);

//            if (!Directory.Exists(directory))
//                Directory.CreateDirectory(directory);

//            using (var stream = new FileStream(path, FileMode.Create))
//            {
//                await file.CopyToAsync(stream);
//            }

//            return Ok(new { message = "Archivo subido y reemplazado correctamente." });
//        }
//        catch (Exception ex)
//        {
//            return StatusCode(500, new { error = "Error al subir el archivo", details = ex.Message });
//        }
//    }



//    private string GetPredefinedAnswer(int id) => id switch
//    {
//        1 => "Un delegado es un tipo que representa referencias a métodos...",
//        2 => "LINQ (Language Integrated Query) permite realizar consultas...",
//        3 => "Singleton: Garantiza una única instancia. Otros patrones: Factory, Observer, Strategy",
//        4 => "SOLID: 1. SRP... 2. OCP... 3. LSP... 4. ISP... 5. DIP...",
//        5 => "No se permite herencia múltiple. Solución: Usar interfaces",
//        _ => "Respuesta no disponible"
//    };
//}

public record AnswerModel(int QuestionId, string Response);
public record EmailRequest(string Email, string Answer);