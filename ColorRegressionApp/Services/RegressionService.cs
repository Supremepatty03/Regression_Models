using ColorRegressionApp.DTO;
using ColorRegressionApp.Models;
using ColorRegressionApp.Data;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.EntityFrameworkCore;

namespace ColorRegressionApp.Services;
public interface IRegressionService
{
    Task<BuildModelResponse> BuildModelAsync(BuildModelRequest request);
    Task<BuildModelResponse?> GetModelAsync(int id);
    Task<BuildModelResponse?> GetLatestModelAsync();
    Task<List<ModelSummaryDto>> GetModelsAsync();
}
public class RegressionService : IRegressionService
{
    private readonly AppDbContext _db;

    public RegressionService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<BuildModelResponse> BuildModelAsync(BuildModelRequest request)
    {
        if (request.Points == null || request.Points.Count < 3)
            throw new ArgumentException("Для построения модели нужно минимум 3 точки.");

        var userId = await ResolveUserIdAsync();

        var model = new RegressionModel
        {
            Name = string.IsNullOrWhiteSpace(request.Name) ? "Модель" : request.Name,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _db.RegressionModels.Add(model);
        await _db.SaveChangesAsync();

        var modelId = model.Id;

        var newPoints = request.Points.Select((p, index) => new ExperimentPoint
        {
            ModelId = modelId,
            PointNumber = index + 1,
            X1 = p.X1,
            X2 = p.X2,
            X3 = p.X3,
            L = p.L,
            A = p.A,
            B = p.B
        }).ToList();

        _db.ExperimentPoints.AddRange(newPoints);
        await _db.SaveChangesAsync();

        var allPoints = await _db.ExperimentPoints
            .OrderBy(p => p.Id)
            .ToListAsync();

        var fitL = Fit(allPoints, p => p.L);
        var fitA = Fit(allPoints, p => p.A);
        var fitB = Fit(allPoints, p => p.B);

        _db.ModelCoefficients.AddRange(
            new ModelCoefficient { ModelId = modelId, TargetName = "L", B1 = fitL.B1, B2 = fitL.B2, B3 = fitL.B3 },
            new ModelCoefficient { ModelId = modelId, TargetName = "A", B1 = fitA.B1, B2 = fitA.B2, B3 = fitA.B3 },
            new ModelCoefficient { ModelId = modelId, TargetName = "B", B1 = fitB.B1, B2 = fitB.B2, B3 = fitB.B3 }
        );

        await _db.SaveChangesAsync();

        return new BuildModelResponse
        {
            ModelId = modelId,
            ModelName = model.Name,
            FormulaL = fitL.Formula,
            FormulaA = fitA.Formula,
            FormulaB = fitB.Formula,
            Coefficients =
            [
                new CoefficientRow { Term = "X1", L = fitL.B1, A = fitA.B1, B = fitB.B1 },
                new CoefficientRow { Term = "X2", L = fitL.B2, A = fitA.B2, B = fitB.B2 },
                new CoefficientRow { Term = "X3", L = fitL.B3, A = fitA.B3, B = fitB.B3 }
            ],
            RealValuesL = allPoints.Select(p => p.L).ToList(),
            RealValuesA = allPoints.Select(p => p.A).ToList(),
            RealValuesB = allPoints.Select(p => p.B).ToList(),
            PredictedValuesL = allPoints.Select(p => fitL.Predict(new ExperimentPoint { X1 = p.X1, X2 = p.X2, X3 = p.X3 })).ToList(),
            PredictedValuesA = allPoints.Select(p => fitA.Predict(new ExperimentPoint { X1 = p.X1, X2 = p.X2, X3 = p.X3 })).ToList(),
            PredictedValuesB = allPoints.Select(p => fitB.Predict(new ExperimentPoint { X1 = p.X1, X2 = p.X2, X3 = p.X3 })).ToList()
        };
    }   

    public async Task<BuildModelResponse?> GetModelAsync(int id)
    {
        var model = await _db.RegressionModels.FirstOrDefaultAsync(m => m.Id == id);
        if (model == null)
            return null;

        return await BuildResponseForModelAsync(model.Id);
    }

    public async Task<BuildModelResponse?> GetLatestModelAsync()
    {
        var model = await _db.RegressionModels
            .OrderByDescending(m => m.CreatedAt)
            .ThenByDescending(m => m.Id)
            .FirstOrDefaultAsync();

        if (model == null)
            return null;

        return await BuildResponseForModelAsync(model.Id);
    }

    public async Task<List<ModelSummaryDto>> GetModelsAsync()
    {
        return await _db.RegressionModels
            .OrderByDescending(m => m.CreatedAt)
            .ThenByDescending(m => m.Id)
            .Select(m => new ModelSummaryDto
            {
                Id = m.Id,
                Name = m.Name,
                CreatedAt = m.CreatedAt
            })
            .ToListAsync();
    }

    private async Task<BuildModelResponse> BuildResponseForModelAsync(int modelId)
    {
        var model = await _db.RegressionModels.FirstAsync(m => m.Id == modelId);

        var points = await _db.ExperimentPoints
            .Where(p => p.ModelId == modelId)
            .OrderBy(p => p.PointNumber)
            .ToListAsync();

        var coeffs = await _db.ModelCoefficients
            .Where(c => c.ModelId == modelId)
            .ToListAsync();

        var fitL = BuildFitFromCoefficients(coeffs, "L");
        var fitA = BuildFitFromCoefficients(coeffs, "A");
        var fitB = BuildFitFromCoefficients(coeffs, "B");

        return new BuildModelResponse
        {
            ModelId = model.Id,
            ModelName = model.Name,
            FormulaL = fitL.Formula,
            FormulaA = fitA.Formula,
            FormulaB = fitB.Formula,
            Coefficients =
            [
                new CoefficientRow { Term = "X1", L = fitL.B1, A = fitA.B1, B = fitB.B1 },
                new CoefficientRow { Term = "X2", L = fitL.B2, A = fitA.B2, B = fitB.B2 },
                new CoefficientRow { Term = "X3", L = fitL.B3, A = fitA.B3, B = fitB.B3 }
            ],
            RealValuesL = points.Select(p => p.L).ToList(),
            RealValuesA = points.Select(p => p.A).ToList(),
            RealValuesB = points.Select(p => p.B).ToList(),
            PredictedValuesL = points.Select(p => fitL.Predict(p)).ToList(),
            PredictedValuesA = points.Select(p => fitA.Predict(p)).ToList(),
            PredictedValuesB = points.Select(p => fitB.Predict(p)).ToList()
        };
    }

    private static RegressionFit Fit(List<ExperimentPoint> points, Func<ExperimentPoint, double> selector)
    {
        int n = points.Count;

        var X = Matrix<double>.Build.Dense(n, 3);
        var y = Vector<double>.Build.Dense(n);

        for (int i = 0; i < n; i++)
        {
            var p = points[i];
            X[i, 0] = p.X1;
            X[i, 1] = p.X2;
            X[i, 2] = p.X3;
            y[i] = selector(p);
        }

        var beta = X.QR().Solve(y);

        return new RegressionFit
        {
            B1 = beta[0],
            B2 = beta[1],
            B3 = beta[2]
        };
    }

    private static RegressionFit BuildFitFromCoefficients(List<ModelCoefficient> coeffs, string target)
    {
        var c = coeffs.First(x => x.TargetName == target);

        return new RegressionFit
        {
            B1 = c.B1,
            B2 = c.B2,
            B3 = c.B3
        };
    }

    private async Task<int> ResolveUserIdAsync()
    {
        var userId = await _db.Users
            .OrderByDescending(u => u.Id)
            .Select(u => (int?)u.Id)
            .FirstOrDefaultAsync();

        if (userId.HasValue)
            return userId.Value;

        var systemUser = new User
        {
            Username = "system",
            Email = "system@local",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("system"),
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(systemUser);
        await _db.SaveChangesAsync();

        return systemUser.Id;
    }

    private sealed class RegressionFit
    {
        public double B1 { get; set; }
        public double B2 { get; set; }
        public double B3 { get; set; }

        public string Formula => $"y = {B1:F3}·X1 + {B2:F3}·X2 + {B3:F3}·X3";

        public double Predict(ExperimentPoint p) => B1 * p.X1 + B2 * p.X2 + B3 * p.X3;
    }
}