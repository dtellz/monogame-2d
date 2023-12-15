using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using monogame2d.Enum;
using monogame2d.States.Base;
using monogame2d.Objects;

namespace monogame2d.States
{
    public class GameplayState : BaseGameState
    {
        private const string PlayerFighter = "images/fighter";
        private const string BackgroundTexture = "images/Barren";
        private PlayerSprite _playerSprite;
        public override void LoadContent()
        {
            AddGameObject(new TerrainBackground(LoadTexture(BackgroundTexture)));
            _playerSprite = new PlayerSprite(LoadTexture(PlayerFighter));
            
            // position the player in the middle of the screen, at the bottom, leaving a slight gap at the bottom
            var playerXPos = _viewportWidth / 2 - _playerSprite.Width / 2;
            var playerYPos = _viewportHeight - _playerSprite.Height - 30;
            _playerSprite.Position = new Vector2(playerXPos, playerYPos);
            AddGameObject(_playerSprite);
        }

        public override void HandleInput()
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape))
            {
                NotifyEvent(Events.GAME_QUIT);
            }
        }
    }
}
