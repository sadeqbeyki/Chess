using Chessfifi.Domain;
using Chessfifi.EndPoint.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Chessfifi.EndPoint.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGameRepository _gameRepository;
        private readonly IChessRepository _chessRepository;

        public HomeController(IGameRepository gameRepository, IChessRepository chessRepository)
        {
            _gameRepository = gameRepository;
            _chessRepository = chessRepository;
        }

        public IActionResult Index()
        {
            ChessGame chessGame = new(); // ایجاد یک نمونه از مدل ChessGame و پر کردن آن

            // پر کردن اطلاعات تخته شطرنج (ChessBoard)
            chessGame.ChessBoard = new List<ChessSquare>();
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    var square = new ChessSquare { Row = row, Column = col, Piece = null };
                    //square.Piece = new List<string>
                    //{
                    //        "/img/rook.png",
                    //        "/img/knight.png",
                    //        "/img/bishop.png",
                    //        "/img/queen.png",
                    //        "/img/king.png"
                    //};

                    //var pieceIndex = 0;
                    //foreach (var item in chessGame.ChessBoard)
                    //{
                    //    if (pieceIndex < square.Piece.Count)
                    //    {
                    //        square.Piece = square.Piece[pieceIndex];
                    //        pieceIndex++;
                    //    }
                    //}
                    // می‌توانید مهره‌ها را بر اساس شرایط مد نظر خود پر کنید
                    // square.Piece = ...;
                    chessGame.ChessBoard.Add(square);
                }
            }

            // پر کردن اطلاعات بازیکنان (Players)
            chessGame.Players = new List<Player>
    {
        new Player { Name = "Player 1", IsTurn = true },
        new Player { Name = "Player 2", IsTurn = false }
    };

            // پر کردن اطلاعات حرکت‌ها (ChessMoves)
            chessGame.ChessMoves = new List<ChessMove>();
            // می‌توانید حرکت‌ها را بر اساس شرایط مد نظر خود پر کنید
            // chessGame.ChessMoves.Add(...);

            return View(chessGame); // ارسال مدل به ویو
        }


        //public IActionResult Index()
        //{
        //    //var chessGame = _gameRepository.GetChessGame(gameId);
        //    return View(/*chessGame*/);
        //}

        //public IActionResult MakeMove(int gameId)
        //{
        //    var chessGame = _gameRepository.GetChessGame(gameId);
        //    return View(chessGame);
        //}

        //[HttpPost]
        //public IActionResult MakeMove(int gameId, int fromRow, int fromColumn, int toRow, int toColumn, string player)
        //{
        //    if (_chessRepository.IsMoveValid(gameId, fromRow, fromColumn, toRow, toColumn))
        //    {
        //        _chessRepository.MakeMove(gameId, fromRow, fromColumn, toRow, toColumn);
        //        if (_chessRepository.IsCheck(gameId, player))
        //        {
        //            return RedirectToAction("GameOver", new { gameId });
        //        }
        //    }
        //    return RedirectToAction("Index", new { gameId });
        //}

        //public IActionResult GameOver(int gameId)
        //{
        //    var chessGame = _gameRepository.GetChessGame(gameId);
        //    return View(chessGame);
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}