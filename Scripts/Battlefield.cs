using Godot;

public class Battlefield : Node2D
{
	public Camera2D Camera { get; set; }

	private ConfigFile _config;
	private PackedScene _tankRes;
	private CSharpScript _playerTankControllerScript;
	private CSharpScript _aiTankControllerScript;

	private Tank _playerTank;

	public override void _Ready()
	{
		_config = new ConfigFile();
		_config.Load("res://Configs/Battlefield.cfg");
		_tankRes = GD.Load <PackedScene> ("res://Objects/Tank.res");
		_playerTankControllerScript = GD.Load <CSharpScript> ("res://Scripts/PlayerTankController.cs");
		_aiTankControllerScript = GD.Load <CSharpScript> ("res://Scripts/AITankController.cs");
	}

	public override void _Process(float delta)
	{
		if (_playerTank != null)
		{
			UpdateCameraPosition();
		}
	}

	public void UpdateCameraPosition()
	{
		Camera.Position = _playerTank.Position;
	}

	public void SpawnPlayerTank()
	{
		var contollerNode = new Node2D();
		contollerNode.Name = "PlayerTankController";
		var objId = contollerNode.GetInstanceId();
		contollerNode.SetScript(_playerTankControllerScript);
		var playerTankContoller = (PlayerTankController) GD.InstanceFromId(objId);
		AddChild(playerTankContoller);

		_playerTank = _tankRes.Instance <Tank> ();
		_playerTank.Modulate = new Color((string) _config.GetValue("Battlefield", "playerTankColor"));
		ConfigureTank(_playerTank);
		playerTankContoller.AddChild(_playerTank);
		playerTankContoller.Tank = _playerTank;
	}

	public void SpawnEnemyTank()
	{
		var contollerNode = new Node2D();
		contollerNode.Name = "AITankController";
		var objId = contollerNode.GetInstanceId();
		contollerNode.SetScript(_aiTankControllerScript);
		var aiTankContoller = (AITankController) GD.InstanceFromId(objId);
		AddChild(aiTankContoller);

		var tank = _tankRes.Instance <Tank> ();
		// TODO: position should be randomized
		tank.Position = new Vector2(1000, -1000);
		tank.Modulate = new Color((string) _config.GetValue("Battlefield", "enemyTankColor"));
		ConfigureTank(tank);
		aiTankContoller.AddChild(tank);
		aiTankContoller.Tank = tank;
	}

	public void ConfigureTank(Tank tank)
	{
		tank.MoveImpulseIncrement = (float) _config.GetValue("Tank", "moveImpulseIncrement");
		tank.MaxMoveImpulse = (float) _config.GetValue("Tank", "maxMoveImpulse");
		tank.TurnImpulseIncrement = (float) _config.GetValue("Tank", "turnImpulseIncrement");
		tank.MaxTurnImpulse = (float) _config.GetValue("Tank", "maxTurnImpulse");

		tank.AmmoResFilename = (string) _config.GetValue("Ammo", "resFilename");
		tank.AmmoShotImpulse = (float) _config.GetValue("Ammo", "shotImpulse");
	}
}
