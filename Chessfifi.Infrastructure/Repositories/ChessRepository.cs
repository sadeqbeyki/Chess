using Chessfifi.Domain;
using Chessfifi.Infrastructure;

namespace Chessfifi.Infrastructure.Repositories
{
    public class ChessRepository : IChessRepository
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


        public void MakeMove(int gameId, int fromRow, int fromColumn, int toRow, int toColumn)
        {
            // دریافت بازی با gameId
            var game = _context.ChessGames.FirstOrDefault(g => g.Id == gameId);

            // دریافت بازیکن کنونی
            var currentPlayer = game.Players.FirstOrDefault(p => p.IsTurn);

            // بررسی وضعیت مهره در موقعیت مبدأ و مالک آن
            var fromSquare = game.ChessBoard.FirstOrDefault(sq => sq.Row == fromRow && sq.Column == fromColumn);
            if (fromSquare == null || fromSquare.Piece == null || fromSquare.Piece != currentPlayer.Name)
            {
                throw new Exception("Invalid move. The piece is not yours.");
            }

            // بررسی معتبر بودن حرکت با استفاده از IsMoveValid
            if (!IsMoveValid(gameId, fromRow, fromColumn, toRow, toColumn))
            {
                throw new Exception("Invalid move. The move is not valid.");
            }

            // انجام حرکت در تخته شطرنج
            var toSquare = game.ChessBoard.FirstOrDefault(sq => sq.Row == toRow && sq.Column == toColumn);
            toSquare.Piece = fromSquare.Piece;
            fromSquare.Piece = null;

            // تغییر نوبت بازیکن
            currentPlayer.IsTurn = false;
            var otherPlayer = game.Players.FirstOrDefault(p => p != currentPlayer);
            otherPlayer.IsTurn = true;

            // ثبت تغییرات در دیتابیس
            _context.SaveChanges();
        }


        public bool IsCheck(int gameId, string player)
        {
            // دریافت بازی با gameId
            var game = _context.ChessGames.FirstOrDefault(g => g.Id == gameId);

            // دریافت بازیکن کنونی و بازیکن دیگر
            var currentPlayer = game.Players.FirstOrDefault(p => p.Name == player);
            var otherPlayer = game.Players.FirstOrDefault(p => p != currentPlayer);

            // پیدا کردن موقعیت شاه هر بازیکن در تخته شطرنج
            var currentPlayerKing = game.ChessBoard.FirstOrDefault(sq => sq.Piece == $"{player}_King");
            var otherPlayerKing = game.ChessBoard.FirstOrDefault(sq => sq.Piece == $"{otherPlayer.Name}_King");

            // بررسی آیا مهره‌ای در موقعیت مهره حریف وجود دارد که به شاه حمله کند؟
            var attackingPieceToCurrentPlayerKing = game.ChessBoard.FirstOrDefault(sq =>
                sq.Piece != null && sq.Piece.StartsWith(otherPlayer.Name) &&
                IsMoveValid(gameId, sq.Row, sq.Column, currentPlayerKing.Row, currentPlayerKing.Column));

            var attackingPieceToOtherPlayerKing = game.ChessBoard.FirstOrDefault(sq =>
                sq.Piece != null && sq.Piece.StartsWith(currentPlayer.Name) &&
                IsMoveValid(gameId, sq.Row, sq.Column, otherPlayerKing.Row, otherPlayerKing.Column));

            // اگر مهره‌ای به شاه حمله کند، وضعیت مات است
            if (attackingPieceToCurrentPlayerKing != null || attackingPieceToOtherPlayerKing != null)
            {
                return true;
            }

            return false;
        }

    }
}