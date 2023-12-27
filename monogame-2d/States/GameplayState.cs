using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using monogame2d.Enum;
using monogame2d.States.Base;
using monogame2d.Objects;
using monogame2d.Input;
using monogame2d.Input.Base;

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
            var playerXPos = _viewportWidth / 2 + _playerSprite.Width / 2;
            var playerYPos = _viewportHeight - _playerSprite.Height - 80;
            _playerSprite.Position = new Vector2(playerXPos, playerYPos);
            AddGameObject(_playerSprite);
        }

        public override void HandleInput()
        {
            InputManager.GetCommands( cmd =>
            {
                if (cmd is GameplayInputCommand.GameExit)
                {
                    NotifyEvent(Events.GAME_QUIT);
                }
                if (cmd is GameplayInputCommand.PlayerMoveLeft)
                {
                    _playerSprite.MoveLeft();
                    KeepPlayerInbounds();
                }
                if (cmd is GameplayInputCommand.PlayerMoveRight)
                {
                    _playerSprite.MoveRight();
                    KeepPlayerInbounds();
                    Console.WriteLine($"DEBUG -> {_playerSprite.Width}");
                }
            });
        }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new GameplayInputMapper());
        }

        public void KeepPlayerInbounds()
        {
            if (_playerSprite.Position.X < 0)
            {
                _playerSprite.Position = new Vector2(0, _playerSprite.Position.Y);
            }
            if (_playerSprite.Position.X > _viewportWidth + _playerSprite.Width)
            {
                _playerSprite.Position = new Vector2(_viewportWidth + _playerSprite.Width, _playerSprite.Position.Y);
            }
            if (_playerSprite.Position.Y < 0)
            {
                _playerSprite.Position = new Vector2(_playerSprite.Position.X, 0);
            }
            if (_playerSprite.Position.Y > _viewportHeight - _playerSprite.Height)
            {
                _playerSprite.Position = new Vector2(_playerSprite.Position.X, _viewportHeight - _playerSprite.Height);
            }
        }
    }
}
