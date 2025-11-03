using TA22KrasanLab2.Models;

namespace TA22KrasanLab2.ViewModels
{
    public class PigVM
    {
        public CreatePigVM CreatePigVM { get; set; }
        public ICollection<Pig> Pigs { get; set; }
        public Pig Pig { get; set; }
    }
}
