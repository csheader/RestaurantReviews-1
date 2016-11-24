using System;
using System.ComponentModel.DataAnnotations;

namespace CMMI.Models.DTO
{
    public class ReviewRequest
    {
        private decimal _score;
        public decimal Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = (value < 0) ? 0 : (value > 5) ? 5 : value;
            }
        }
        [Required]
        public long UserId { get; set; }
        [MaxLength(1000)]
        [Required]
        public string Comment { get; set; }
        [Required]
        public DateTime RatingDateTime { get; set; }
        [Required]
        public long RestaurantId { get; set; }
    }
}