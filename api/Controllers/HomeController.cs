using InterfazUsuario.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InterfazUsuario.Controllers
{
    public class HomeController : Controller
    {
        private static List<Question> questions = new()
    {
        new Question(1, "¿Qué es un delegado (delegate) en .NET y cómo se utiliza?", AnswerDisplayType.Popup),
        new Question(2, "¿Qué es LINQ y cómo se utiliza en .NET?", AnswerDisplayType.NewWindow),
        new Question(3, "Explique el patrón de diseño Singleton y nombre tres patrones más.", AnswerDisplayType.InlineNote),
        new Question(4, "¿Cuáles son los Principios SOLID?", AnswerDisplayType.Email),
        new Question(5, "¿Cómo se implementa la herencia múltiple en .NET?", AnswerDisplayType.Popup)
    };

        public IActionResult Index()
        {
            return View(questions);
        }
    }
   
}



