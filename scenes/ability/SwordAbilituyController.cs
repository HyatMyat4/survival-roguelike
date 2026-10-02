using Godot;
using System.Linq;
using Godot.Collections;
using System;

public partial class SwordAbilituyController : Node
{
	private const int MAX_RANGE = 150;

	[Export]
	private PackedScene swordAbility;

	private float baseDamage = 5;
	private float additionalDamagePercent = 1;

	private double baseWaitTime;
	private Timer timer;

	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		timer.Timeout += OnTimerTimeout;

		baseWaitTime = timer.WaitTime;

		GameEvent.Instance.AbilityUpgradeAdded += OnAbilityUpgradeAdded;
	}

	private void OnTimerTimeout()
	{
		var player =
			GetTree().GetFirstNodeInGroup("player") as Node2D;

		if (player == null)
			return;

		int maxRangeSquared = MAX_RANGE * MAX_RANGE;

		var enemies = GetTree()
			.GetNodesInGroup("enemy")
			.Cast<Node2D>()
			.Where(e =>
				e.GlobalPosition.DistanceSquaredTo(player.GlobalPosition)
				<= maxRangeSquared
			)
			.ToList();

		if (enemies.Count == 0)
			return;

		enemies.Sort((a, b) =>
			a.GlobalPosition
				.DistanceSquaredTo(player.GlobalPosition)
				.CompareTo(
					b.GlobalPosition.DistanceSquaredTo(player.GlobalPosition)
				)
		);

		var swordInstance =
			swordAbility.Instantiate<SwordAbility>();

		var foregroundLayer =
			GetTree().GetFirstNodeInGroup("foreground_layer");

		if (foregroundLayer == null)
			return;

		foregroundLayer.AddChild(swordInstance);

		swordInstance.hitBoxComponent.Damage =
			baseDamage * additionalDamagePercent;

		swordInstance.GlobalPosition =
			enemies[0].GlobalPosition +
			Vector2.Right.Rotated(
				(float)GD.RandRange(0, Mathf.Tau)
			) * 4f;

		var enemyDirection =
			enemies[0].GlobalPosition -
			swordInstance.GlobalPosition;

		swordInstance.Rotation =
			enemyDirection.Angle();
	}

	private void OnAbilityUpgradeAdded(
		AbilityUpgrade upgrade,
		Dictionary<string, CurrentUpgrade> currentUpgrade)
	{
		if (upgrade.id == "SwordRate")
		{
			var quantity =
				currentUpgrade["SwordRate"].Quantity;

			var percentReduction =
				Mathf.Min(quantity * 0.1f, 0.9f);

			timer.WaitTime =
				baseWaitTime * (1.0f - percentReduction);

			timer.Start();

			GD.Print($"Sword Rate: {quantity}");
			GD.Print($"Sword Wait Time: {timer.WaitTime}");
		}
		else if (upgrade.id == "SwordDamage")
		{
			var quantity =
				currentUpgrade["SwordDamage"].Quantity;

			additionalDamagePercent =
				1.0f + quantity * 0.15f;

			GD.Print($"Sword Damage Level: {quantity}");
			GD.Print(
				$"Sword Damage: {baseDamage * additionalDamagePercent}"
			);
		}
	}
}