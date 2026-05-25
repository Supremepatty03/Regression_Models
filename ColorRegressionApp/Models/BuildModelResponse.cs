public class BuildModelResponse
{
    public string FormulaL { get; set; } = "";
    public string FormulaA { get; set; } = "";
    public string FormulaB { get; set; } = "";

    public List<double> RealValuesL { get; set; } = new();
    public List<double> RealValuesA { get; set; } = new();
    public List<double> RealValuesB { get; set; } = new();

    public List<double> PredictedValuesL { get; set; } = new();
    public List<double> PredictedValuesA { get; set; } = new();
    public List<double> PredictedValuesB { get; set; } = new();
}