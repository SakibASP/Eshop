using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models.BusinessDomains
{
    [Table("ShippingDetails")]
    public class ShippingDetails
    {
        [Key]
        public int AutoId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter a name")]
        public string? Name { get; set; }

        [Display(Name = "Mobile")]
        [Required(ErrorMessage = "Please enter your phone number")]
        public string? MobileNumber { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter your e-mail address")]
        public string? Email { get; set; }

        [Display(Name = "AddressLine 1")]
        [Required(ErrorMessage = "Please enter the first address line")]
        public string? Line1 { get; set; }
        [Display(Name = "AddressLine 2")]
        public string? Line2 { get; set; }
        [Display(Name = "AddressLine 3")]
        public string? Line3 { get; set; }

        [Display(Name = "District")]
        [Required(ErrorMessage = "Please enter a city name")]
        public string? City { get; set; }

        [Display(Name = "Division")]
        [Required(ErrorMessage = "Please enter a state name")]
        public string? State { get; set; }

        [Display(Name = "Zip Code")]
        public string? Zip { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Please enter a country name")]
        public string? Country { get; set; }

        [Display(Name = "Gift Wrap")]
        public bool Giftwrap { get; set; }
        public bool IsConfirmed { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ConfirmDate { get; set; }

        [Display(Name="Confirm By")]
        public string? ConfirmBy { get; set; }
    }
}
