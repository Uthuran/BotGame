using System.Collections.Generic;
using OdiGame.World;

namespace OdiGame.Core
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
