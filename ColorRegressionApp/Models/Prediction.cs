namespace ColorRegressionApp.Models;

public class Prediction
{
    public int Id { get; set; }
    public int? ModelId { get; set; }

    public double TargetL { get; set; }
    public double TargetA { get; set; }
    public double TargetB { get; set; }

    public double ResultX1 { get; set; }
    public double ResultX2 { get; set; }
    public double ResultX3 { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}