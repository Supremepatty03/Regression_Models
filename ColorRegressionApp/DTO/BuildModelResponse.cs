namespace ColorRegressionApp.DTO;

public class BuildModelResponse
{
    public int ModelId { get; set; }

    public string ModelName { get; set; } = "";
    public string FormulaL { get; set; } = "";
    public string FormulaA { get; set; } = "";
    public string FormulaB { get; set; } = "";

    public List<CoefficientRow> Coefficients { get; set; } = new();

    public List<double> RealValuesL { get; set; } = new();
    public List<double> RealValuesA { get; set; } = new();
    public List<double> RealValuesB { get; set; } = new();

    public List<double> PredictedValuesL { get; set; } = new();
    public List<double> PredictedValuesA { get; set; } = new();
    public List<double> PredictedValuesB { get; set; } = new();
}

public class CoefficientRow
{
    public string Term { get; set; } = "";
    public double L { get; set; }
    public double A { get; set; }
    public double B { get; set; }
}