using System;
using System.Collections.Generic;
using System.Linq;

using monogame2d.Engine.Objects;
using monogame2d.Enum;
using monogame2d.Engine.Input;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace monogame2d.Engine.States
{
	public abstract class BaseGameState
	{
		private readonly List<BaseGameObject> _gameObjects =
			new List<BaseGameObject>();

		public abstract void LoadContent();

		public abstract void HandleInput(GameTime gameTime);

        public abstract void UpdateGameState(GameTime gameTime);

        public event EventHandler<BaseGameState> OnStateSwitched;
		public event EventHandler<Events> OnEventNotification;

		private const string FallBackTexture = "images/Empty";

		protected int _viewportHeight;

		protected int _viewportWidth;

		protected InputManager InputManager { get; set; }

		protected abstract void SetInputManager();

		// Wrapper to avoid Exceptions and load empty textures instead when there are issues with any of them
		protected Texture2D LoadTexture(string textureName)
		{
			var texture = _contentManager.Load<Texture2D>(textureName);

			return texture ?? _contentManager.Load<Texture2D>(FallBackTexture);
		}

        // TODO: refactor contentManager copy hold strategy defined in page 79 aroung mid page to use the new
        //       UnloadAsset(String) -> https://docs.monogame.net/api/Microsoft.Xna.Framework.Content.ContentManager.html#Microsoft_Xna_Framework_Content_ContentManager_UnloadAsset_System_String_
		//       -> At the time this book was written this method did not exist and there was no way to remove specific assets. This was a workaround of that limitation.
        private ContentManager _contentManager;

		public void Initialize(ContentManager contentManager, int viewportWidht, int viewportHeight)
		{
			_contentManager = contentManager;
			_viewportWidth = viewportWidht;
			_viewportHeight = viewportHeight;

			SetInputManager();
		}

        public virtual void Update(GameTime gametime) { }

        public void UnloadContent()
        {
            _contentManager.Unload();
        }
        // TODO: End of refactor ^
        public void NotifyEvent(Events eventType, object argument = null)
		{
			OnEventNotification?.Invoke(this, eventType);
			foreach (var gameObject in _gameObjects)
			{
				gameObject.OnNotify(eventType);
			}
		}

		protected void SwitchState(BaseGameState gameState)
		{
			OnStateSwitched?.Invoke(this, gameState);
		}

		protected void AddGameObject(BaseGameObject gameObject)
		{
			_gameObjects.Add(gameObject);
		}

		protected void RemoveGameObject(BaseGameObject gameObject)
		{
			_gameObjects.Remove(gameObject);
		}

		public void Render(SpriteBatch spriteBatch)
		{
			foreach (var gameObject in _gameObjects.OrderBy
				(a => a.zIndex)) // zIndex ordering is a technique that orders game objects from the closest to the farthest, therefore rendering first closes game objects
			{
				gameObject.Render(spriteBatch);
			}
		}
	}

}

