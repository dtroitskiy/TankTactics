using Godot;

public class Game : Node2D
{
	public override void _Ready()
	{
		var battlefield = GetNode <Battlefield> ("Battlefield");
		battlefield.Camera = GetNode <Camera2D> ("Camera");
		battlefield.SpawnPlayerTank();
		battlefield.SpawnEnemyTank();
	}
}
