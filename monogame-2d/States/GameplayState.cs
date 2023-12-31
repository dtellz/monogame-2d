using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private const string BulletTexture = "images/bullet";

        private Texture2D _bulletTexture;
        private List<BulletSprite> _bulletList;
        private List<BulletSprite> _newBulletList;
        private PlayerSprite _playerSprite;

        private bool _isShooting;
        private TimeSpan _lastShotAt;

        public override void LoadContent()
        {
            AddGameObject(new TerrainBackground(LoadTexture(BackgroundTexture)));
            _playerSprite = new PlayerSprite(LoadTexture(PlayerFighter));
            _bulletTexture = LoadTexture(BulletTexture);

            _bulletList = new List<BulletSprite>();
            _newBulletList = new List<BulletSprite>();
            
            // position the player in the middle of the screen, at the bottom, leaving a slight gap at the bottom
            var playerXPos = _viewportWidth / 2 + _playerSprite.Width / 2;
            var playerYPos = _viewportHeight - _playerSprite.Height - 80;
            _playerSprite.Position = new Vector2(playerXPos, playerYPos);
            AddGameObject(_playerSprite);
        }

        public override void Update(GameTime gametime)
        {
            foreach (var bullet in _bulletList)
            {
                bullet.MoveUp();
                var bulletStillOnScreen = bullet.Position.Y > -30;
                if (bulletStillOnScreen)
                {
                    _newBulletList.Add(bullet);
                }
                else
                {
                    RemoveGameObject(bullet);
                }
            }

            _bulletList = _newBulletList;
            _newBulletList = new List<BulletSprite>();
            // Lets lock bullet shots at 0.2 seconds - this prevents shoots to be made by keeping space pressed
            // TODO: fix this logic branch. Pressing space keeps shooting
            if (_lastShotAt != null && gametime.TotalGameTime - _lastShotAt > TimeSpan.FromSeconds(0.2))
            {
                _isShooting = false;
            }
        }

        public override void HandleInput(GameTime gameTime)
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
                }
                if (cmd is GameplayInputCommand.PlayerShoots)
                {
                    Shoot(gameTime);
                }
            });
        }

        private void Shoot(GameTime gameTime)
        {
            if (!_isShooting)
            {
                CreateBullets();
                _isShooting = true;
                _lastShotAt = gameTime.TotalGameTime;
            }
        }

        private void CreateBullets()
        {
            var bulletSpriteLeft = new BulletSprite(_bulletTexture);
            var bulletSpriteRight = new BulletSprite(_bulletTexture);

            // Initial position around player's nose when shoot
            var bulletY = _playerSprite.Position.Y + 30;
            var bulletLeftX = _playerSprite.Position.X + _playerSprite.Width / 2 - 40;
            var bulletRightX = _playerSprite.Position.X + _playerSprite.Width / 2 + 10;

            bulletSpriteLeft.Position = new Vector2(bulletLeftX, bulletY);
            bulletSpriteRight.Position = new Vector2(bulletRightX, bulletY);

            _bulletList.Add(bulletSpriteLeft);
            _bulletList.Add(bulletSpriteRight);

            AddGameObject(bulletSpriteLeft);
            AddGameObject(bulletSpriteRight);
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
