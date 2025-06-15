using Microsoft.AspNetCore.Mvc;
using TelaAutenticacao.Models;
using TelaAutenticacao.Data;
using System.Linq;

public class AccountController : Controller
{
    private readonly AppDbContext _context;
    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
        if (user != null)
            return RedirectToAction("Register");

        ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
        return View(model);
    }

    [HttpGet]
    public IActionResult Register(int? editId)
    {
        var users = _context.Users.ToList();
        ViewBag.Users = users;

        if (editId.HasValue)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == editId.Value);
            if (user != null)
            {
                var model = new UserRegisterViewModel
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password
                };
                ViewBag.EditId = user.Id;
                return View(model);
            }
        }

        return View();
    }

    [HttpPost]
    public IActionResult Register(UserRegisterViewModel model, int? editId)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Users = _context.Users.ToList();
            ViewBag.EditId = editId;
            return View(model);
        }

        if (editId.HasValue)
        {
            // Editar usuário existente
            var user = _context.Users.FirstOrDefault(u => u.Id == editId.Value);
            if (user != null)
            {
                // Verifica se o e-mail já existe para outro usuário
                if (_context.Users.Any(u => u.Email == model.Email && u.Id != user.Id))
                {
                    ModelState.AddModelError("Email", "E-mail já cadastrado.");
                    ViewBag.Users = _context.Users.ToList();
                    ViewBag.EditId = editId;
                    return View(model);
                }

                user.Name = model.Name;
                user.Email = model.Email;
                user.Password = model.Password;
                _context.SaveChanges();

                ViewBag.SuccessMessage = "Usuário atualizado com sucesso!";
            }
        }
        else
        {
            // Novo cadastro
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "E-mail já cadastrado.");
                ViewBag.Users = _context.Users.ToList();
                return View(model);
            }

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            ViewBag.SuccessMessage = "Cadastro efetuado com sucesso!";
        }

        ViewBag.Users = _context.Users.ToList();
        ModelState.Clear();
        return View();
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
        return RedirectToAction("Register");
    }
}