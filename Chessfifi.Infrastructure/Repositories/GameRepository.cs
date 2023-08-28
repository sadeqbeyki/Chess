using Chessfifi.Domain;
using Chessfifi.Infrastructure;

namespace Chessfifi.Infrastructure.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ChessDbContext _context;

        public GameRepository(ChessDbContext context)
        {
            _context = context;
        }

        public ChessGame GetChessGame(int gameId)
        {
            throw new NotImplementedException();
        }



    }
}