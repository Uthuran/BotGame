using System.Collections.Generic;

namespace OdiGame.Core
{
	public class GameContext
	{
		public OdiGame.World.GridWorld Map { get; }
		public List<Actor> Actors { get; }

		public GameContext(OdiGame.World.GridWorld map, List<Actor> actors)
		{
			Map = map;
			Actors = actors;
		}
	}
}
