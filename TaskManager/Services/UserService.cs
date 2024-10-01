using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using System.Security.Cryptography;
using System.Text;
using TaskManager.Data;

namespace TaskManager.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para registrar usuário
        public async Task<bool> RegisterUser(Usuario user, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username || u.Email == user.Email))
            {
                return false; // Usuário já existe
            }

            user.PasswordHash = HashPassword(password); // Salvar o hash da senha
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        // Método para verificar login
        public async Task<Usuario> LoginUser(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

            if (user == null || !VerifyPasswordHash(password, user.PasswordHash))
            {
                return null; // Login inválido
            }

            return user; // Retorna o usuário se as credenciais forem válidas
        }

        // Método para gerar hash da senha
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        // Método para verificar se a senha corresponde ao hash
        private bool VerifyPasswordHash(string password, string passwordHash)
        {
            var hash = HashPassword(password);
            return hash == passwordHash;
        }
    }
}
