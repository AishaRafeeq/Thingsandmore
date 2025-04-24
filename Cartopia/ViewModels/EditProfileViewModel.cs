
using System.ComponentModel.DataAnnotations;
namespace Cartopia.ViewModels
{
    

    public class EditProfileViewModel
    {
        [Required]
        public required string FullName { get; set; }

        [Required]
        public required string Address { get; set; }

        [Required]
        public required string City { get; set; }

        [Required]
        public required string Country { get; set; }
    }
}
