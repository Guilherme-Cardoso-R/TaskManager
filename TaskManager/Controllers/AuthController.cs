using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.Helpers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    // Endpoint para registrar usuário
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new Usuario
        {
            Username = request.Username,
            Email = request.Email
        };

        var result = await _userService.RegisterUser(user, request.Password);

        if (!result)
        {
            return BadRequest("Usuário já existe.");
        }

        return Ok("Usuário registrado com sucesso.");
    }

    // Endpoint para login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userService.LoginUser(request.Username, request.Password);

        if (user == null)
        {
            return Unauthorized("Credenciais inválidas.");
        }

        // Aqui geramos o token JWT após o login (usando a função JwtTokenHelper criada anteriormente)
        var token = JwtTokenHelper.GenerateToken(user.Username, user.Role);

        return Ok(new { token });
    }
}

public class RegisterRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

