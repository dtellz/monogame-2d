using Microsoft.Xna.Framework.Input;
using monogame2d.Objects;
using monogame2d.States.Base;
using System;

namespace monogame2d.States
{
    public class SplashState : BaseGameState
    {
        public override void LoadContent()
        {
            // TODO: Add Content Loading

            AddGameObject(new SplashImage(LoadTexture("images/splash")));
        }

        public override void HandleInput()
        {
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Enter))
            {
                SwitchState(new GameplayState());
            }
        }

        protected override void SetInputManager()
        {
            Console.WriteLine("SplashState input manager Not implemented");
        }
    }
}

