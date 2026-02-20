using System.Collections.Generic;

namespace BotGame.Core
{
	public class GameContext
	{
		public BotGame.World.GridWorld Map { get; }
		public List<Actor> Actors { get; }

		public GameContext(BotGame.World.GridWorld map, List<Actor> actors)
		{
			Map = map;
			Actors = actors;
		}
	}
}
