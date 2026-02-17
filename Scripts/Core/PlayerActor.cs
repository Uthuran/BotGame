using OdiGame.World;

namespace OdiGame.Core
{
	public class PlayerActor : Actor
	{
		public PlayerActor(GridPosition start) : base(start) { }

		public override void TakeTurn(GameContext context)
		{
			// handled by input in TurnController
		}
	}
}
