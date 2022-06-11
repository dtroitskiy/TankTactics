using Godot;

public class AITankController : Node2D
{
	private Tank _tank;

	public Tank Tank
	{
		get => _tank;
		set => _tank = value;
	}

	public override void _Process(float delta)
	{
		
	}
}
