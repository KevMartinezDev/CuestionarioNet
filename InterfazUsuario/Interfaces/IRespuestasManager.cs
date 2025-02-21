namespace InterfazUsuario.Managers
{
    public interface IRespuestasManager
    {
        string GetPredefinedRespuesta(int id);
        Task SaveRespuestas(List<AnswerModel> respuestas);
        Task<List<AnswerModel>> LoadRespuestas();
        Task UploadRespuestasFile(IFormFile file);
    }
}
