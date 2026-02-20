namespace BotGame.World
{
	public readonly struct GridPosition
	{
		public readonly int X;
		public readonly int Y;

		public GridPosition(int x, int y) { X = x; Y = y; }

		public static GridPosition operator +(GridPosition a, GridPosition b)
			=> new GridPosition(a.X + b.X, a.Y + b.Y);

		public override string ToString() => $"({X},{Y})";
	}
}
