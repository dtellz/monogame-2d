﻿using System;
using monogame2d.Engine.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace monogame2d.States.Gameplay
{
	public class GameplayInputMapper : BaseInputMapper
	{
		public override IEnumerable<BaseInputCommand> GetKeyboardState(KeyboardState state)
		{
			var commands = new List<GameplayInputCommand>();

			if(state.IsKeyDown(Keys.Escape))
			{
				commands.Add(new GameplayInputCommand.GameExit());
			}
			if(state.IsKeyDown(Keys.Left))
			{
				commands.Add(new GameplayInputCommand.PlayerMoveLeft());
			}
            if (state.IsKeyDown(Keys.Right))
            {
                commands.Add(new GameplayInputCommand.PlayerMoveRight());
            }
            if (state.IsKeyDown(Keys.Space))
            {
                commands.Add(new GameplayInputCommand.PlayerShoots());
            }
			return commands;
        }
	}
}

