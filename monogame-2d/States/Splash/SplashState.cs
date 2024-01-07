using monogame2d.Objects;
using monogame2d.Engine.States;
using Microsoft.Xna.Framework;
using monogame2d.States.Gameplay;
using monogame2d.Engine.Input;
using System;

namespace monogame2d.States.Splash
{
    public class SplashState : BaseGameState
    {
        public override void LoadContent()
        {
            // TODO: Add Content Loading

            AddGameObject(new SplashImage(LoadTexture("images/splash")));
        }

        public override void HandleInput(GameTime gameTime)
        {
            InputManager.GetCommands(cmd =>
            {
                if (cmd is SplashInputCommand.GameSelect)
                {
                    SwitchState(new GameplayState());
                }
            });
        }

        public override void UpdateGameState(GameTime _) { }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new SplashInputMapper());
        }
    }
}
