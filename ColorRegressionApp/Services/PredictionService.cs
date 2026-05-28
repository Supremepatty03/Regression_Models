using ColorRegressionApp.DTO;
using ColorRegressionApp.Models;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.EntityFrameworkCore;
using ColorRegressionApp.Data;

namespace ColorRegressionApp.Services;

public interface IPredictionService
{
    Task<PredictionResponse> CalculateAsync(PredictionRequest request);
}

public class PredictionService : IPredictionService
{
    private readonly AppDbContext _db;

    public PredictionService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PredictionResponse> CalculateAsync(PredictionRequest request)
    {
        // Получаем все модели регрессии
        var models = await _db.RegressionModels
            .OrderBy(m => m.Id)
            .ToListAsync();

        if (!models.Any())
            throw new InvalidOperationException("Сначала постройте хотя бы одну модель.");

        var validResults = new List<double[]>();

        foreach (var model in models)
        {
            var coeffs = await _db.ModelCoefficients
                .Where(c => c.ModelId == model.Id)
                .ToListAsync();

            var cL = coeffs.FirstOrDefault(c => c.TargetName == "L");
            var cA = coeffs.FirstOrDefault(c => c.TargetName == "A");
            var cB = coeffs.FirstOrDefault(c => c.TargetName == "B");

            if (cL == null || cA == null || cB == null)
                continue;

            // Матрица коэффициентов (3x3)
            var matrix = Matrix<double>.Build.DenseOfArray(new double[,]
            {
                { cL.B1, cL.B2, cL.B3 },
                { cA.B1, cA.B2, cA.B3 },
                { cB.B1, cB.B2, cB.B3 }
            });

            // Вектор целевых значений Lab
            var target = Vector<double>.Build.Dense(new[]
            {
                request.TargetL,
                request.TargetA,
                request.TargetB
            });

            // Решаем систему: matrix * x = target  ->  x = pseudoinverse(matrix) * target
            var resultVector = matrix.PseudoInverse() * target;
            var result = resultVector.ToArray();

            if (result.Any(v => double.IsNaN(v) || double.IsInfinity(v)))
                continue;

            validResults.Add(result);
        }

        if (!validResults.Any())
            throw new InvalidOperationException("Не удалось получить результат ни по одной модели.");

        // Усредняем результаты по всем успешным моделям
        var avgX1 = validResults.Average(r => r[0]);
        var avgX2 = validResults.Average(r => r[1]);
        var avgX3 = validResults.Average(r => r[2]);

        // Сохраняем предсказание в БД, НО без привязки к конкретной модели
        var prediction = new Prediction
        {
            ModelId = null,               // ✅ ключевое изменение
            TargetL = request.TargetL,
            TargetA = request.TargetA,
            TargetB = request.TargetB,
            ResultX1 = avgX1,
            ResultX2 = avgX2,
            ResultX3 = avgX3,
            CreatedAt = DateTime.UtcNow
        };

        _db.Predictions.Add(prediction);
        await _db.SaveChangesAsync();

        return new PredictionResponse
        {
            ResultX1 = avgX1,
            ResultX2 = avgX2,
            ResultX3 = avgX3,
            UsedModelsCount = validResults.Count
        };
    }
}