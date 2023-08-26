using Chessfifi.Domain;
using Chessfifi.Infrastructure;

namespace Chessfifi.Infrastructure.Repositories
{
    public class ChessRepository
    {
        private readonly ChessDbContext _context;

        public ChessRepository(ChessDbContext context)
        {
            _context = context;
        }

        public bool IsMoveValid(int gameId, int fromRow, int fromColumn, int toRow, int toColumn)
        {
            // بررسی معتبر بودن موقعیت‌ها
            if (fromRow < 0 || fromRow >= 5 || fromColumn < 0 || fromColumn >= 5 ||
                toRow < 0 || toRow >= 5 || toColumn < 0 || toColumn >= 5)
            {
                return false;
            }

            // دریافت بازیکن کنونی با توجه به gameId
            Player currentPlayer = _context.Players.FirstOrDefault(p => p.Id == gameId && p.IsTurn);

            // بررسی وجود مهره در موقعیت مبدأ و مالک آن
            ChessSquare fromSquare = _context.ChessSquares.FirstOrDefault(sq => sq.Row == fromRow && sq.Column == fromColumn);
            if (fromSquare == null || fromSquare.Piece == null || fromSquare.Piece != currentPlayer.Name)
            {
                return false;
            }

            // بررسی قوانین حرکت مهره
            // اینجا باید بر اساس نوع مهره و قوانین حرکت آن، شرایط را بررسی کنید و مقدار true یا false را بازگردانید.

            // بررسی مسیر حرکت
            // اینجا نیز باید بررسی کنید که مسیر حرکت خالی است یا نه.

            return true; // اگر تمام شرایط بالا مطابقت داشتند
        }

    }
}