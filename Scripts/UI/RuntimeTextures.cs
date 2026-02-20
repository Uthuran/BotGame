using Godot;

namespace OdiGame.UI
{
	public static class RuntimeTextures
	{
		public static Texture2D SolidTexture(int size, Color color)
		{
			var img = Image.Create(size, size, false, Image.Format.Rgba8);
			img.Fill(color);
			return ImageTexture.CreateFromImage(img);
		}
	}
}
