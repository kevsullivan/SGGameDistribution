using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using Christoc.Modules.SGGameDistribution.Data;

namespace Christoc.Modules.SGGameDistribution.Components
{
    public class GameController
    {
        /// <summary>
        /// Returns a game - leverges the use of IHydratable we made use of in Game class for using CBO.FillObject
        /// Fills the Game object with the data from returned DataReader.
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns>A Game</returns>
        public static Game GetGame(int gameId)
        {
            // CBO.FillObject Convert the returned Datareader to a Game object.
            return CBO.FillObject<Game>(DataProvider.Instance().GetGame(gameId));
        }

        /// <summary>
        /// Returns a list of games - leverges the use of IHydratable we made use of in Game class for using CBO.FillObject
        /// Fills the Games in List object with the data from returned DataReader.
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>A list of Games</returns>
        public static List<Game> GetGames(int moduleId)
        {
            return CBO.FillCollection<Game>(DataProvider.Instance().GetGames(moduleId));
        }

        /// <summary>
        /// Deletes All games for a particular module.
        /// </summary>
        /// <param name="moduleId"></param>
        public static void DeleteGames(int moduleId)
        {
            DataProvider.Instance().DeleteGames(moduleId);
        }

        /// <summary>
        /// Delete a specific game.
        /// </summary>
        /// <param name="gameId"></param>
        public static void DeleteGame(int gameId)
        {
            DataProvider.Instance().DeleteGame(gameId);
        }

        /// <summary>
        /// Saves Game details within system.
        /// Used for adding and updating of game.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="tabId"></param>
        /// <returns></returns>
        public static int SaveGame(Game g, int tabId)
        {
            if (g.GameId < 1)
            {
                g.GameId = DataProvider.Instance().AddGame(g);
                
                //add content item integration - create content items, get id, save task.
            }
            else
            {
                DataProvider.Instance().UpdateGame(g);
            }
            return g.GameId;
        }
    }
}