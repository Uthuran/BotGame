using System;

namespace BotGame.World
{
	public class GridWorld
	{
		private readonly int _width;
		private readonly int _height;
		private readonly bool[,] _blocked;

		public int Width => _width;
		public int Height => _height;

		public GridWorld(int width, int height)
		{
			_width = width;
			_height = height;
			_blocked = new bool[width, height];
		}

		public bool IsInside(GridPosition p)
			=> p.X >= 0 && p.Y >= 0 && p.X < _width && p.Y < _height;

		public bool IsBlocked(GridPosition p)
			=> !IsInside(p) || _blocked[p.X, p.Y];

		public void SetBlocked(GridPosition p, bool blocked)
		{
			if (!IsInside(p)) throw new ArgumentException("Position outside grid.");
			_blocked[p.X, p.Y] = blocked;
		}
	}
}
