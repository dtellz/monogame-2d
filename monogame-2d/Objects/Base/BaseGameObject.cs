using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using monogame2d.Enum;


namespace monogame2d.Objects.Base
{
	public class BaseGameObject
	{
		protected Texture2D _texture;

		protected Vector2 _position = Vector2.One;

		public int zIndex;

        public int Width { get { return _texture.Width; } }
        public int Height { get { return _texture.Height; } }
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public virtual void OnNotify(Events eventType) { }

		public virtual void Render(SpriteBatch spriteBatch)
		{
			// TODO: drawing call here
			spriteBatch.Draw(_texture, _position, Color.White);
		}

	}
}

