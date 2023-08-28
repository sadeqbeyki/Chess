using System.Collections.Generic;
using System.Numerics;
using Chessfifi.Domain;
using Microsoft.EntityFrameworkCore;
namespace Chessfifi.Infrastructure;

public class ChessDbContext : DbContext
{

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    var assembly = typeof(GameConfig).Assembly;
    //    modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    //    base.OnModelCreating(modelBuilder);
    //}
    public ChessDbContext(DbContextOptions<ChessDbContext> options) : base(options)
    {
    }

    public DbSet<ChessSquare> ChessSquares { get; set; }
    public DbSet<ChessGame> ChessGames { get; set; }
    public DbSet<ChessMove> ChessMoves { get; set; }
    public DbSet<Player> Players { get; set; }
}

