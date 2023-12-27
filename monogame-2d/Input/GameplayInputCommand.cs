using System;
namespace monogame2d.Input.Base;

public class GameplayInputCommand : BaseInputCommand
{
	public class GameExit : GameplayInputCommand { }
	public class PlayerMoveLeft : GameplayInputCommand { }
	public class PlayerMoveRight : GameplayInputCommand { }
	public class PlayerShoots : GameplayInputCommand { }
}

