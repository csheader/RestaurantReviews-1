using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMMI.Models
{
    public class Address
    {
        public long AddressId { get; set; }
        [MaxLength(255)]
        public string Address1 { get; set; }
        [MaxLength(255)]
        public string Address2 { get; set; }
        [MaxLength(255)]
        public string Address3 { get; set; }
        [MaxLength(255)]
        public string City { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
        [MaxLength(50)]
        public string ZipCode { get; set; }
        [MaxLength(50)]
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}