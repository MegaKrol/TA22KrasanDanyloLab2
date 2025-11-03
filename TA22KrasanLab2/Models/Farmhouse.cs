namespace TA22KrasanLab2.Models
{
    public class Farmhouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descripton { get; set; }
        
        public ICollection<Pig> Pigs { get; set; }
    }
}
