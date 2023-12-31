﻿using Chessfifi.Common.Enums;
using Chessfifi.Contracts;
using Chessfifi.Contracts.Dto;
using Microsoft.Extensions.Logging;

namespace Chessfifi.Services;

public class GameManager : IGameManager
{
    private ILogger _logger;
    private PieceTypesDto _pieceTypes;
    private List<IGameInfo> _games;

    public GameManager(PieceTypesDto pieceTypes, ILoggerFactory loggerFactory)
    {
        _pieceTypes = pieceTypes;
        _logger = loggerFactory.CreateLogger("chess");
    }

    private class Search
    {
        public PlayerDto Player { get; set; }
        public SearchStatus Status { get; set; }
        public string GameId { get; set; }
        public DateTime? GameStart { get; set; }
        public GameMode GameMode { get; set; }
    }

    private object lockSearchList = new object();

    private List<Search> searchList = new List<Search>();

    public bool IsInit { get; private set; }

    public void Init(List<IGameInfo> games)
    {
        _games = games;
        IsInit = true;
    }


    public void StartSearch(PlayerDto player, GameMode gameMode)
    {
        _logger.LogInformation("Search Start [player=" + player.Id + "]");

        lock (lockSearchList)
        {
            var search = searchList.FirstOrDefault(x => x.Player.Id == player.Id && x.Status != SearchStatus.Finish);
            if (search != null)
            {
                _logger.LogInformation("Search in process");
                return;
            }

            var gameInProcess = _games.Any(x => x.IsMyGame(player.Id) && !x.IsFinish);
            if (gameInProcess)
            {
                throw new BusinessException("There is an illegal game, search impossible");
            }

            _logger.LogInformation("Search Start Success");

            search = new Search();
            search.Player = player;
            search.Status = SearchStatus.InProcess;
            search.GameMode = gameMode;
            searchList.Insert(0, search);

            CheckPairPlayer();
        }
    }

    private void CheckPairPlayer()
    {
        var searchs = searchList.Where(x => x.Status == SearchStatus.InProcess).GroupBy(x => x.GameMode).First();
        if (searchs.Count() == 2)
        {
            _logger.LogInformation("Search Finish");
            var gameId = DateTime.Now.ToString("yyyyMMddHHmmss") + Guid.NewGuid().ToString("N");
            searchList[0].GameId = gameId;
            searchList[0].Status = SearchStatus.NeedConfirm;
            searchList[1].GameId = gameId;
            searchList[1].Status = SearchStatus.NeedConfirm;
        }
    }

    public SearchStatus Check(int playerId)
    {
        _logger.LogInformation("Search Check [player=" + playerId + "]");
        var search = searchList.FirstOrDefault(x => x.Player.Id == playerId);
        if (search == null)
        {
            if (_games.Any(x => x.IsFinish == false && x.IsMyGame(playerId)))
            {
                return SearchStatus.Finish;
            }
            return SearchStatus.NotFound;
        }

        return search.Status;
    }

    public SearchStatus Confirm(int playerId)
    {
        _logger.LogInformation("Search Confirm [player=" + playerId + "]");
        var search = searchList.FirstOrDefault(x => x.Player.Id == playerId);
        if (search == null)
        {
            throw new BusinessException("Game search missing");
        }

        if (search.Status == SearchStatus.NeedConfirm)
        {
            var twoSearch = searchList.First(x => x.GameId == search.GameId && x.Player.Id != search.Player.Id);
            if (twoSearch.Status == SearchStatus.NeedConfirmOpponent)
            {
                _logger.LogInformation("Two Player Confirm Game Start");
                var gameStartDate = DateTime.Now;
                search.GameStart = gameStartDate;
                search.Status = SearchStatus.Finish;
                twoSearch.GameStart = gameStartDate;
                twoSearch.Status = SearchStatus.Finish;

                PlayerDto whitePlayer;
                PlayerDto blackPlayer;
                if (DateTime.Now.Millisecond % 2 == 0)
                {
                    whitePlayer = searchList[1].Player;
                    blackPlayer = searchList[0].Player;
                }
                else
                {
                    blackPlayer = searchList[1].Player;
                    whitePlayer = searchList[0].Player;
                }

                IGameInfo game = new GameInfo(_pieceTypes, search.GameId, search.GameMode, whitePlayer, blackPlayer);
                _games.Add(game);
            }
            else
            {
                _logger.LogInformation("Wait Game Opponent Confirm");
                search.Status = SearchStatus.NeedConfirmOpponent;
            }
        }

        return search.Status;
    }

    public void StopSearch(int playerId)
    {
        _logger.LogInformation("Search Stop [player=" + playerId + "]");
        var search = searchList.FirstOrDefault(x => x.Player.Id == playerId);
        if (search == null)
        {
            return;
        }
        if (search.Status == SearchStatus.Finish)
        {
            throw new Exception("this status " + search.Status + " bad");
        }

        lock (lockSearchList)
        {
            var twoSearch = searchList.FirstOrDefault(x => x.GameId == search.GameId && x.Player.Id != search.Player.Id);
            if (twoSearch != null)
            {
                twoSearch.Status = SearchStatus.InProcess;
            }
            searchList.Remove(search);
        }
    }

    public IGameInfo FindMyPlayingGame(int playerId)
    {
        var gameInProcess = _games.LastOrDefault(x => x.IsMyGame(playerId));
        return gameInProcess;
    }
}
