using System.Collections.Generic;
using BotGame.World;

namespace BotGame.Core
{
	public class GameContext
	{
		public GridWorld Map { get; }
		public List<Actor> Actors { get; }
		public int Seed { get; }

		public GameContext(GridWorld map, List<Actor> actors, int seed = 0)
		{
			Map = map;
			Actors = actors;
			Seed = seed;
		}
	}
}
