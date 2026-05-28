namespace ColorRegressionApp.Models;

public class ExperimentPoint
{
    public int Id { get; set; }
    public int ModelId { get; set; }
    public int PointNumber { get; set; }

    public double X1 { get; set; }
    public double X2 { get; set; }
    public double X3 { get; set; }

    public double L { get; set; }
    public double A { get; set; }
    public double B { get; set; }
}