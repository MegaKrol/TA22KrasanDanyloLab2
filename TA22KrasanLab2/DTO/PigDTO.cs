namespace TA22KrasanLab2.DTO
{
    // Використовується для читання даних
    public class PigDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public string Description { get; set; }
    }

    // Використовується для створення/оновлення
    public class CreateUpdatePigDto
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public string Description { get; set; }
    }
}
