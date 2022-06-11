using Godot;

public class PlayerTankController : Node2D
{
	private Tank _tank;

	public Tank Tank
	{
		get => _tank;
		set => _tank = value;
	}

	public override void _Process(float delta)
	{
		if (_tank == null) return;

		if (Input.IsActionPressed("move_forward"))
		{
			_tank.MoveForward();
		}
		if (Input.IsActionPressed("move_backward"))
		{
			_tank.MoveBackward();
		}
		if (Input.IsActionPressed("turn_left"))
		{
			_tank.TurnLeft();
		}
		if (Input.IsActionPressed("turn_right"))
		{
			_tank.TurnRight();
		}
		var towerTargetAngle = _tank.Position.AngleToPoint(GetGlobalMousePosition()) - _tank.Rotation;
		if (towerTargetAngle > Mathf.Pi) towerTargetAngle -= Mathf.Pi * 2f;
		if (towerTargetAngle < -Mathf.Pi) towerTargetAngle += Mathf.Pi * 2f;
		_tank.SetTowerTargetAngle(towerTargetAngle);

		if (Input.IsActionJustReleased("shoot"))
		{
			_tank.Shoot();
		}
	}
}
