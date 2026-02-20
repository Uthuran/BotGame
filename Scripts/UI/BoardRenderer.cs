using Godot;
using System.Collections.Generic;
using BotGame.Core;
using BotGame.World;

namespace BotGame.UI
{
	public partial class BoardRenderer : Node2D
	{
		[Export] public int TileSize = 48;
		[Export] public Texture2D FloorTexture;
		[Export] public Texture2D WallTexture;

		[Export] public PackedScene PlayerBotScene;
		[Export] public PackedScene EnemyBotScene;

		private Texture2D _tileTex = default!;
		private Texture2D _playerFallbackTex = default!;
		private Texture2D _enemyTex = default!;

		private readonly Dictionary<Actor, Node2D> _actorNodes = new();
		private readonly List<Sprite2D> _tiles = new();

		public override void _Ready()
		{
			_tileTex = RuntimeTextures.SolidTexture(64, new Color(0.12f, 0.12f, 0.14f));
			_playerFallbackTex = RuntimeTextures.SolidTexture(64, new Color(0.20f, 0.80f, 0.35f));
			_enemyTex = RuntimeTextures.SolidTexture(64, new Color(0.85f, 0.25f, 0.25f));

			GD.Print($"[BoardRenderer READY] Path={GetPath()} TileSize={TileSize}");
			GD.Print($"[BoardRenderer READY] PlayerBotScene={(PlayerBotScene?.ResourcePath ?? "NULL")}");
			GD.Print($"[BoardRenderer READY] EnemyBotScene={(EnemyBotScene?.ResourcePath ?? "NULL")}");
		}

		public void BuildTiles(BotGame.World.GridWorld map)
		{
			foreach (var t in _tiles) t.QueueFree();
			_tiles.Clear();

			for (int y = 0; y < map.Height; y++)
			for (int x = 0; x < map.Width; x++)
			{
				var pos = new GridPosition(x, y);

				var tex = map.IsBlocked(pos) && WallTexture != null
					? WallTexture
					: FloorTexture ?? _tileTex;

				var s = new Sprite2D
				{
					Texture = tex,
					Centered = false,
					Position = new Vector2(x * TileSize, y * TileSize),
					Scale = new Vector2((float)TileSize / tex.GetWidth(), (float)TileSize / tex.GetHeight())
				};

				AddChild(s);
				_tiles.Add(s);
			}
		}

		public void SyncActors(IReadOnlyList<Actor> actors)
		{
			foreach (var a in actors)
			{
				if (_actorNodes.ContainsKey(a)) continue;

				var node = CreateActorVisual(a);
				AddChild(node);
				_actorNodes[a] = node;
			}

			foreach (var kvp in _actorNodes)
			{
				var a = kvp.Key;
				var n = kvp.Value;
				n.Position = GridToWorldCenter(a.Position);
			}
		}

		public void RebuildActors(IReadOnlyList<Actor> actors)
		{
			foreach (var n in _actorNodes.Values)
				n.QueueFree();

			_actorNodes.Clear();
			SyncActors(actors);
		}

		private Node2D CreateActorVisual(Actor a)
		{
			var isPlayer = a is PlayerActor;
			var scene = isPlayer ? PlayerBotScene : EnemyBotScene;

			GD.Print($"CreateActorVisual: actor={a.GetType().Name} isPlayer={isPlayer} scene={(scene?.ResourcePath ?? "NULL")}");

			if (scene != null)
			{
				var inst = scene.Instantiate<Node2D>();

				if (inst is PlayerBotView view)
				{
					var baseSprite = view.GetNodeOrNull<Sprite2D>("Base");
					var texWidth = baseSprite?.Texture?.GetWidth() ?? 1024f;

					view.SpriteScale = TileSize / texWidth;
				}
				else
				{
					var sprite = inst.GetNodeOrNull<Sprite2D>("Base");
					var texWidth = sprite?.Texture?.GetWidth() ?? 1024f;

					inst.Scale = Vector2.One * (TileSize / texWidth);
				}
				return inst;
			}

			GD.Print($"[CreateActorVisual FALLBACK] actor={a.GetType().Name} isPlayer={isPlayer} sceneWasNull={(scene == null)}");

			return new Sprite2D
			{
				Texture = isPlayer ? _playerFallbackTex : _enemyTex,
				Centered = true,
				Scale = new Vector2((float)TileSize / 64f, (float)TileSize / 64f)
			};
		}

		private Vector2 GridToWorldCenter(GridPosition p)
		{
			return new Vector2(
				p.X * TileSize + TileSize * 0.5f,
				p.Y * TileSize + TileSize * 0.5f
			);
		}
	}
}
