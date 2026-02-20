using Godot;

namespace BotGame.UI
{
	/// <summary>
	/// Layered sprite compositor for a robot.
	/// Child nodes are expected to exist with these exact names:
	/// Base, Legs, ArmL, ArmR, Module
	/// </summary>
	public partial class PlayerBotView : Node2D
	{
		[ExportGroup("Textures")]
		[Export] public Texture2D BaseTexture { get; set; } = default!;
		[Export] public Texture2D LegsWheelsTexture { get; set; } = default!;
		[Export] public Texture2D ArmLWeaponTexture { get; set; } = default!;

		[ExportGroup("Sizing")]
		[Export(PropertyHint.Range, "0.01,2.0,0.01")]
		public float SpriteScale { get; set; } = 0.25f;

		private Sprite2D _base = default!;
		private Sprite2D _legs = default!;
		private Sprite2D _armL = default!;
		private Sprite2D _armR = default!;
		private Sprite2D _module = default!;

		private bool _useWheels;

		public override void _Ready()
		{
			// Grab child sprites by name (no Inspector wiring)
			_base = GetNode<Sprite2D>("Base");
			_legs = GetNode<Sprite2D>("Legs");
			_armL = GetNode<Sprite2D>("ArmL");
			_armR = GetNode<Sprite2D>("ArmR");
			_module = GetNode<Sprite2D>("Module");

			Scale = Vector2.One * SpriteScale;

			// Default loadout: base + weapon, no wheels
			ApplyLoadout(useWheels: false, weaponLeft: true);
		}

		public override void _UnhandledInput(InputEvent e)
		{
			// Temporary test toggle
			if (e is InputEventKey k && k.Pressed && !k.Echo && k.Keycode == Key.L)
			{
				_useWheels = !_useWheels;
				ApplyLoadout(_useWheels, weaponLeft: true);
			}
		}

		public void ApplyLoadout(bool useWheels, bool weaponLeft)
		{
			_base.Texture = BaseTexture;

			_armL.Texture = weaponLeft ? ArmLWeaponTexture : null;
			_legs.Texture = useWheels ? LegsWheelsTexture : null;

			// Not used yet
			_armR.Texture = null;
			_module.Texture = null;
		}
	}
}
