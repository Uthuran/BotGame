using BotGame.World;

namespace BotGame.Core
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
