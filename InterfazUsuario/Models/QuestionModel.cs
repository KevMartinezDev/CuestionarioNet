namespace InterfazUsuario.Models
{
    public record Question(int Id, string Text, AnswerDisplayType DisplayType);

    public enum AnswerDisplayType
    {
        Popup,
        NewWindow,
        InlineNote,
        Email
    }
}
