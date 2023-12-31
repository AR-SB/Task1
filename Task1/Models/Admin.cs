﻿using System.ComponentModel.DataAnnotations;

namespace Task1.Models
{
    public class Admin
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
