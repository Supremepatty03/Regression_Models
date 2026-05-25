using ColorRegressionApp.DTO;
using MathNet.Numerics.LinearAlgebra;

public interface IRegressionService
{
    BuildModelResponse BuildModel(BuildModelRequest request);
}

namespace ColorRegressionApp.Services
{
    public class RegressionService : IRegressionService
    {
        public BuildModelResponse BuildModel(BuildModelRequest request)
        {
            if (request.Points == null || request.Points.Count < 3)
                throw new ArgumentException("Для модели нужно минимум 3 точки.");

            var lModel = Fit(request.Points, p => p.L);
            var aModel = Fit(request.Points, p => p.A);
            var bModel = Fit(request.Points, p => p.B);

            return new BuildModelResponse
            {
                FormulaL = lModel.Formula,
                FormulaA = aModel.Formula,
                FormulaB = bModel.Formula,

                RealValuesL = request.Points.Select(p => p.L).ToList(),
                RealValuesA = request.Points.Select(p => p.A).ToList(),
                RealValuesB = request.Points.Select(p => p.B).ToList(),

                PredictedValuesL = lModel.PredictedValues.ToList(),
                PredictedValuesA = lModel.PredictedValues.ToList(),
                PredictedValuesB = bModel.PredictedValues.ToList()
            };
        }

        private static RegressionResult Fit(
            List<ExperimentPointDto> points,
            Func<ExperimentPointDto, double> selector)
        {
            int n = points.Count;

            // Матрица признаков
            var X = Matrix<double>.Build.Dense(n, 3);

            // Вектор ответов
            var y = Vector<double>.Build.Dense(n);

            for (int i = 0; i < n; i++)
            {
                var p = points[i];

                X[i, 0] = p.X1;
                X[i, 1] = p.X2;
                X[i, 2] = p.X3;

                y[i] = selector(p);
            }

            // Решение системы
            var beta = X.QR().Solve(y);

            // Предсказанные значения
            var predicted = X * beta;

            return new RegressionResult
            {
                B1 = beta[0],
                B2 = beta[1],
                B3 = beta[2],

                PredictedValues = predicted.ToArray()
            };
        }
    }

    public class RegressionResult
    {
        public double B1 { get; set; }
        public double B2 { get; set; }
        public double B3 { get; set; }

        public double[] PredictedValues { get; set; } = Array.Empty<double>();

        public string Formula =>
            $"y = {B1:F3}·X1 + {B2:F3}·X2 + {B3:F3}·X3";
    }
}