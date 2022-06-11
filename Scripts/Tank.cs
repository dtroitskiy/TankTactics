using Godot;

public class Tank : RigidBody2D, IHittable
{
	private PackedScene _ammoRes;

	private Sprite _tower;
	private Node2D _shootingPoint;
	
	private bool _isMovingForward, _isMovingBackward, _isTurningLeft, _isTurningRight;
	private float _curMoveImpulse;
	private float _curTurnImpulse;
	private float _curTowerAngle, _targetTowerAngle;

	public float MoveImpulseIncrement { get; set; }
	public float MaxMoveImpulse { get; set; }
	public float TurnImpulseIncrement { get; set; }
	public float MaxTurnImpulse { get; set; }
	
	public string AmmoResFilename;
	public float AmmoShotImpulse { get; set; }

	public override void _Ready()
	{
		_ammoRes = GD.Load <PackedScene> (AmmoResFilename);
		_tower = GetNode <Sprite> ("Tower");
		_shootingPoint = _tower.GetNode <Node2D> ("ShootingPoint");
	}

	public override void _PhysicsProcess(float delta)
	{
		ProcessMovement(delta);
		ProcessTowerRotation(delta);
	}

	public void MoveForward()
	{
		_isMovingForward = true;
	}

	public void MoveBackward()
	{
		_isMovingBackward = true;
	}

	public void TurnLeft()
	{
		_isTurningLeft = true;
	}

	public void TurnRight()
	{
		_isTurningRight = true;
	}

	public void SetTowerTargetAngle(float angle)
	{
		_targetTowerAngle = angle;
	}

	// TODO: magic numbers
	public void Shoot()
	{
		var ammo = _ammoRes.Instance <Ammo> ();
		ammo.Position = _shootingPoint.GlobalPosition;
		ammo.Rotation = _tower.GlobalRotation;
		GetParent <Node2D> ().AddChild(ammo);
		ammo.ApplyCentralImpulse(Vector2.Left.Rotated(ammo.Rotation) * AmmoShotImpulse);
	}

	private void ProcessMovement(float delta)
	{
		if (_isMovingForward || _isMovingBackward)
		{
			_curMoveImpulse += MoveImpulseIncrement * delta;
			if (_curMoveImpulse > MaxMoveImpulse) _curMoveImpulse = MaxMoveImpulse;
			ApplyCentralImpulse((_isMovingForward ? 1f : -1f) * Vector2.Left.Rotated(Rotation) * _curMoveImpulse * delta);
		}
		else
		{
			_curMoveImpulse = 0;
		}
		if (_isTurningLeft || _isTurningRight)
		{
			_curTurnImpulse += TurnImpulseIncrement * delta;
			if (_curTurnImpulse > MaxTurnImpulse) _curTurnImpulse = MaxTurnImpulse;
			ApplyTorqueImpulse((_isTurningLeft ? -1f : 1f) * _curTurnImpulse * delta);
		}
		else
		{
			_curTurnImpulse = 0;
		}

		_isMovingForward = _isMovingBackward = _isTurningLeft = _isTurningRight = false;
	}

	private void ProcessTowerRotation(float delta)
	{
		float towerRotationDir = 0;
		if (Mathf.Abs(_curTowerAngle - _targetTowerAngle) > 0.01f)
		{
			if (_curTowerAngle >= 0 && _targetTowerAngle >= 0)
			{
				towerRotationDir = _targetTowerAngle > _curTowerAngle ? 1f : -1f;
			}
			if (_curTowerAngle < 0 && _targetTowerAngle < 0)
			{
				towerRotationDir = _targetTowerAngle < _curTowerAngle ? -1f : 1f;
			}
			if (_curTowerAngle >= 0 && _targetTowerAngle < 0)
			{
				towerRotationDir = -_targetTowerAngle + _curTowerAngle > Mathf.Pi ? 1f : -1f;
			}
			if (_curTowerAngle < 0 && _targetTowerAngle >= 0)
			{
				towerRotationDir = -_curTowerAngle + _targetTowerAngle > Mathf.Pi ? -1f : 1f;
			}
			_curTowerAngle += towerRotationDir * delta;
			if (_curTowerAngle > Mathf.Pi)
			{
				_curTowerAngle = -Mathf.Pi + (_curTowerAngle - Mathf.Pi);
			}
			else if (_curTowerAngle < -Mathf.Pi)
			{
				_curTowerAngle = Mathf.Pi - (-_curTowerAngle - Mathf.Pi);
			}
		}
		
		_tower.Rotation = _curTowerAngle;
	}

	public void ReceiveHit()
	{
		
	}
}
