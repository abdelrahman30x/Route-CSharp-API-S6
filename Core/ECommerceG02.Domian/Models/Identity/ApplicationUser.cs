using ECommerceG02.Presistence.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ECommerceG02.Domian.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string? ProfilePictureUrl { get; set; }
        public string? Bio { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; }

        public string? AlternativeEmail { get; set; }
        public string? MobileNumber { get; set; }

        public string? PreferredLanguage { get; set; }
        public string? TimeZone { get; set; }

        public int FailedLoginAttempts { get; set; }
        public DateTime? LockoutEndDate { get; set; }

        public virtual Address Address { get; set; }

        public ApplicationUser()
        {
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
            FailedLoginAttempts = 0;
        }

        // Methods
        public string GetFullName() => $"{FirstName} {LastName}".Trim();

        public void UpdateLastLogin() => LastLoginAt = DateTime.UtcNow;

        public void IncrementFailedLoginAttempts() => FailedLoginAttempts++;

        public void ResetFailedLoginAttempts() => FailedLoginAttempts = 0;
    }
}
