using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMI.Models
{
    public class Restaurant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RestaurantId { get; set; }
        [MaxLength(255)]
        [Required]
        public string Name { get; set; }
        public long ContactId { get; set; }
        [Required]
        public Contact ContactInformation { get; set; }
        [MaxLength(255)]
        public string Cuisine { get; set; }
        public long AddressId { get; set; }
    }
}