using Godot;
using System.Collections.Generic;
using OdiGame.AI;
using OdiGame.UI;
using OdiGame.World;

namespace OdiGame.Core
{
	public partial class TurnController : Node
	{
		[Export] public int MapWidth = 12;
		[Export] public int MapHeight = 8;

		private OdiGame.World.GridWorld _map = default!;
		private GameContext _ctx = default!;
		private PlayerActor _player = default!;
		private List<Actor> _actors = default!;
		private BoardRenderer _renderer = default!;

		public override void _Ready()
		{
			_renderer = GetParent().GetNode<BoardRenderer>("BoardRenderer");

			_map = new OdiGame.World.GridWorld(MapWidth, MapHeight);
			// Simple wall strip with one gap
			for (int x = 2; x < MapWidth - 2; x++)
			{
				if (x == MapWidth / 2) continue; // gap
				_map.SetBlocked(new OdiGame.World.GridPosition(x, 4), true);
			}

			_player = new PlayerActor(new GridPosition(2, 2));
			var enemy = new ChaseEnemy(new GridPosition(MapWidth - 3, MapHeight - 3));

			_actors = new List<Actor> { _player, enemy };
			_ctx = new GameContext(_map, _actors);

			_renderer.BuildTiles(_map);
			_renderer.RebuildActors(_actors);
			var cam = GetParent().GetNode<Camera2D>("Camera2D");

			// Center camera on the middle of the grid in world pixels
			cam.Position = new Vector2(
				(MapWidth * _renderer.TileSize) * 0.5f,
				(MapHeight * _renderer.TileSize) * 0.5f
			);
		}
		
		public override void _UnhandledInput(InputEvent e)
		{
			if (e is not InputEventKey k || !k.Pressed || k.Echo) return;
			if (k.Keycode == Key.R)
			{
				_renderer.RebuildActors(_actors);
				GD.Print("[TurnController] RebuildActors called");
				return;
			}

			GridPosition? move = k.Keycode switch
			{
				Key.W or Key.Up => new GridPosition(0, -1),
				Key.S or Key.Down => new GridPosition(0,  1),
				Key.A or Key.Left => new GridPosition(-1, 0),
				Key.D or Key.Right => new GridPosition( 1, 0),
				_ => null
			};

			if (move == null) return;

			TryPlayerMove(move.Value);
		}

		private void TryPlayerMove(OdiGame.World.GridPosition delta)
		{
			var target = _player.Position + delta;

			if (_map.IsBlocked(target)) return;

			// cannot step onto another actor (combat comes later)
			foreach (var a in _actors)
				if (a != _player && a.Position.X == target.X && a.Position.Y == target.Y)
					return;

			_player.SetPosition(target);

			// enemies after player
			foreach (var a in _actors)
			{
				if (a == _player) continue;

				var before = a.Position;
				a.TakeTurn(_ctx);

				// if enemy tries to move onto player or another enemy, revert
				var after = a.Position;
				if ((after.X == _player.Position.X && after.Y == _player.Position.Y) || IsEnemyCollision(a))
					a.SetPosition(before);
			}

			_renderer.RebuildActors(_actors);
		}

private bool IsEnemyCollision(OdiGame.Core.Actor self)
{
	foreach (var a in _actors)
	{
		if (a == self) continue;
		if (a.Position.X == self.Position.X && a.Position.Y == self.Position.Y)
			return true;
	}
	return false;
}

	private bool IsOccupied(OdiGame.World.GridPosition pos)
{
	foreach (var a in _actors)
		if (a.Position.X == pos.X && a.Position.Y == pos.Y)
			return true;

	return false;
}

	}
}
