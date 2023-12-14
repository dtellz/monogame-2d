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
        public override void LoadContent()
        {
            AddGameObject(new SplashImage(LoadTexture(BackgroundTexture)));
            AddGameObject(new SplashImage(LoadTexture(PlayerFighter)));
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
