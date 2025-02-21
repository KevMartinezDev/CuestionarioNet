
namespace InterfazUsuario.Managers
{
    public class RespuestasManager : IRespuestasManager
    {
        private const string FILE_PATH = "C:\\archivostxt\\respuestas.txt";

        public string GetPredefinedRespuesta(int id) => id switch
        {
            1 => "Un delegado es un tipo que representa referencias a métodos...",
            2 => "LINQ (Language Integrated Query) permite realizar consultas...",
            3 => "Singleton: Garantiza una única instancia. Otros patrones: Factory, Observer, Strategy",
            4 => "SOLID: 1. SRP... 2. OCP... 3. LSP... 4. ISP... 5. DIP...",
            5 => "No se permite herencia múltiple. Solución: Usar interfaces",
            _ => "Respuesta no disponible"
        };

        public async Task SaveRespuestas(List<AnswerModel> respuestas)
        {
            string directory = Path.GetDirectoryName(FILE_PATH);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            await File.WriteAllLinesAsync(FILE_PATH,
                respuestas.Select(a => $"{a.QuestionId}|{a.Response}"));
        }

        public async Task<List<AnswerModel>> LoadRespuestas()
        {
            if (!File.Exists(FILE_PATH))
                throw new FileNotFoundException("No hay respuestas guardadas.");

            var lines = await File.ReadAllLinesAsync(FILE_PATH);
            return lines
                .Select(line => line.Split('|'))
                .Where(parts => parts.Length == 2)
                .Select(parts => new AnswerModel(int.Parse(parts[0]), parts[1]))
                .ToList();
        }

        public async Task UploadRespuestasFile(IFormFile file)
        {
            string directory = Path.GetDirectoryName(FILE_PATH);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using var stream = new FileStream(FILE_PATH, FileMode.Create);
            await file.CopyToAsync(stream);
        }
    }
}


