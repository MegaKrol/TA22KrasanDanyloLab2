using System.ComponentModel.DataAnnotations;

namespace TA22KrasanLab2.Models
{
    public class Pig
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ім'я є обов'язковим")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Ім'я має бути від 2 до 50 символів")]
        [Display(Name = "Кличка")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        [Range(0.1, 500.0, ErrorMessage = "Вага має бути в межах від 0.1 до 500 кг")]
        public double Weight { get; set; }
    }
}
