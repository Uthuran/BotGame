using System.Linq;
using OdiGame.Core;
using OdiGame.World;

namespace OdiGame.AI
{
	public class ChaseEnemy : Actor
	{
		public ChaseEnemy(GridPosition start) : base(start) { }

		public override void TakeTurn(GameContext context)
		{
			var player = context.Actors.OfType<PlayerActor>().FirstOrDefault();
			if (player == null) return;

			int dx = player.Position.X - Position.X;
			int dy = player.Position.Y - Position.Y;

			// 4-dir: move dominant axis (tie -> X)
			GridPosition step;
			if (System.Math.Abs(dx) >= System.Math.Abs(dy))
				step = new GridPosition(dx == 0 ? 0 : dx / System.Math.Abs(dx), 0);
			else
				step = new GridPosition(0, dy == 0 ? 0 : dy / System.Math.Abs(dy));

			var target = Position + step;
			if (!context.Map.IsBlocked(target))
				SetPosition(target);
		}
	}
}
