namespace ColorRegressionApp.DTO;

public class BuildModelRequest
{
    public string Name { get; set; } = "Модель";
    public List<ExperimentPointDto> Points { get; set; } = new();
}