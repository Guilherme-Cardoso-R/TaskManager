﻿using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } 

        public string Role { get; set; } = "User";
    }
}
