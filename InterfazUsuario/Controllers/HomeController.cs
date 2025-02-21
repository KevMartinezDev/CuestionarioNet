using InterfazUsuario.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace InterfazUsuario.Controllers
{
    public class HomeController : Controller
    {
        private static List<Question> questions = new()
    {
        new Question(1, "�Qu� es un delegado (delegate) en .NET y c�mo se utiliza?", AnswerDisplayType.Popup),
        new Question(2, "�Qu� es LINQ y c�mo se utiliza en .NET?", AnswerDisplayType.NewWindow),
        new Question(3, "Explique el patr�n de dise�o Singleton y nombre tres patrones m�s.", AnswerDisplayType.InlineNote),
        new Question(4, "�Cu�les son los Principios SOLID?", AnswerDisplayType.Email),
        new Question(5, "�C�mo se implementa la herencia m�ltiple en .NET?", AnswerDisplayType.Popup)
    };

        public IActionResult Index()
        {
            return View(questions);
        }
    }
   
}



