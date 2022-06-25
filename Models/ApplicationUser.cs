using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Computer_Store.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Avatar { get; set; }

        [RegularExpression(@"^(\b[A-Z][a-z]*\s)*(\b[A-Z][a-z]*)$")]
        [Required]
        public string? FirstName { get; set; }

        [RegularExpression(@"^[A-Z]([a-z]*)$")]
        [Required]
        public string? LastName { get; set; }
        public string Fullname { get => FirstName + " " + LastName; }

        public string? Address { get; set; }

        [RegularExpression(@"^0[0-9]{9,10}$")]
        public string? Phone { get; set; }

        public DateTime? DateofBirth { get; set; }

        public Cart? Cart { get; set; }

        public History? History{ get; set; }

        public double? Expense { get; set; } = 0;

    }

    public enum Roles
    {
        Admin,
        Moderator,
        B_User,
        A_User,
        S_User
    }
}