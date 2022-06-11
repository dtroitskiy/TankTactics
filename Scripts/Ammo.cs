using Godot;

public class Ammo : RigidBody2D
{
	public override void _Ready()
	{
	}

	public void OnBodyEntered(Node2D obj)
	{
		if (obj is IHittable)
		{
			((IHittable) obj).ReceiveHit();
		}
	}
}
