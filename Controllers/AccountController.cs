using Microsoft.AspNetCore.Mvc;
using TelaAutenticacao.Models;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Exemplo de autenticação simples (substitua por sua lógica real)
        if (model.Email == "admin@teste.com" && model.Password == "123456")
        {
            // Autenticação bem-sucedida
            return RedirectToAction("Index", "Home");
        }

        // Se falhar, exibe mensagem de erro
        ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
        return View(model);
    }
}
