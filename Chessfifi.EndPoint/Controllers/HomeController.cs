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
            return View();
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