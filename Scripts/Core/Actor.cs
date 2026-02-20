using OdiGame.World;

namespace OdiGame.Core
{
	public abstract class Actor
	{
		public GridPosition Position { get; private set; }

		protected Actor(GridPosition start) { Position = start; }

		public void SetPosition(GridPosition pos) => Position = pos;

		public abstract void TakeTurn(GameContext context);
	}
}
