namespace Cars.Data.Entities;

public class Car
{
    public int Id { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Model_Year { get; set; }
    public decimal Price { get; set; }
    public bool Deleted { get; set; }
}