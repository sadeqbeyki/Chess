using System.Collections.Generic;
using System.Numerics;
using Chessfifi.Domain;
using Microsoft.EntityFrameworkCore;
namespace Chessfifi.Infrastructure;

public class ChessDbContext : DbContext
{

    public ChessDbContext(DbContextOptions<ChessDbContext> options) : base(options)
    {
    }

    public DbSet<ChessSquare> ChessSquares { get; set; }
    public DbSet<ChessGame> ChessGames { get; set; }
    public DbSet<ChessMove> ChessMoves { get; set; }
    public DbSet<Player> Players { get; set; }
}

