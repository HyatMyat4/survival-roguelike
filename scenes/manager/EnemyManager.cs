using Godot;

public partial class EnemyManager : Node
{
	private const float SPAWN_RADIUS = 350f;

	[Export]
	private PackedScene BasicEnemy;

	[Export]
	private PackedScene WizardEnemy;

	[Export]
	private ArenaTimeManager arenaTimeManager;

	private Timer timer;

	double baseSpawnTime = 0;

	private WeightedTable enemyTable = new();

	public override void _Ready()
	{
		enemyTable.AddItem(BasicEnemy, 10);

		timer = GetNode<Timer>("Timer");
		baseSpawnTime = timer.WaitTime;
		timer.Timeout += OnTimerTimeout;
		arenaTimeManager.ArenaDifficultyIncreased += OnArenaDifficultyIncrease;

	}

	private Vector2 GetSpawnPosition()
	{
		var player = GetTree().GetFirstNodeInGroup("player") as Node2D;

		if (player == null)
			return Vector2.Zero;

		var spawnPosition = Vector2.Zero;

		var randomDirection = Vector2.Right.Rotated(
			(float)GD.RandRange(0.0, Mathf.Tau)
		);

		for (int i = 0; i < 4; i++)
		{
			spawnPosition =
				player.GlobalPosition + randomDirection * SPAWN_RADIUS;

			var queryParameters = PhysicsRayQueryParameters2D.Create(
				player.GlobalPosition,
				spawnPosition,
				1
			);

			var result = GetTree().Root.World2D.DirectSpaceState
				.IntersectRay(queryParameters);

			if (result.Count == 0)
			{
				break;
			}

			randomDirection = randomDirection.Rotated(
				Mathf.DegToRad(90)
			);
		}

		return spawnPosition;
	}
	private void OnTimerTimeout()
	{
		var enemyScene = enemyTable.PickItem();
		var enemy = enemyScene.Instantiate<Node2D>();
		var entitiesLayer = GetTree().GetFirstNodeInGroup("entities_layer");
		entitiesLayer.AddChild(enemy);
		enemy.GlobalPosition = GetSpawnPosition();
	}


	private void OnArenaDifficultyIncrease(int arenaDifficulty)
	{
		double timeOff = arenaDifficulty * (0.1 / 12.0);
		timeOff = Mathf.Min((float)timeOff, 0.7f);
		GD.Print($"Time off: {timeOff}");

		timer.WaitTime = baseSpawnTime - timeOff;

		if (arenaDifficulty == 1)
		{
			enemyTable.AddItem(WizardEnemy, 20);
		}
	}
}