namespace ColorRegressionApp.Models;

public class ModelCoefficient
{
    public int Id { get; set; }
    public int ModelId { get; set; }
    public string TargetName { get; set; } = "";

    public double B1 { get; set; }
    public double B2 { get; set; }
    public double B3 { get; set; }
}