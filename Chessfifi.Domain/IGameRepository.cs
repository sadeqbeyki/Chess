using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chessfifi.Domain
{
    public interface IGameRepository
    {
        ChessGame GetChessGame(int gameId);
    }
}
