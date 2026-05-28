using ColorRegressionApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ColorRegressionApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<RegressionModel> RegressionModels => Set<RegressionModel>();
    public DbSet<ExperimentPoint> ExperimentPoints => Set<ExperimentPoint>();
    public DbSet<ModelCoefficient> ModelCoefficients => Set<ModelCoefficient>();
    public DbSet<Prediction> Predictions => Set<Prediction>();
    
    
}