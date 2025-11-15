using ECommerceG02.Domian.Models.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceG02.Presistence.Identity.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string AddressLine1 { get; set; }

        [StringLength(100)]
        public string AddressLine2 { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(100)]
        public string State { get; set; }

        [Required]
        [StringLength(100)]
        public string Country { get; set; }

        [Required]
        [StringLength(20)]
        public string PostalCode { get; set; }

        
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ApplicationUser? User { get; set; }

        [ForeignKey("User")]
        public string? UserId { get; set; }

        public string GetFullAddress()
        {
            var fullAddress = AddressLine1;

            if (!string.IsNullOrWhiteSpace(AddressLine2))
                fullAddress += $", {AddressLine2}";

            fullAddress += $", {City}, {State} {PostalCode}, {Country}";

            return fullAddress;
        }

        public void MarkAsDefault()
        {
            IsDefault = true;
        }

        public void UnmarkAsDefault()
        {
            IsDefault = false;
        }

        public void UpdateTimestamp()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}