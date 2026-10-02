using Godot;
using Godot.Collections;

public partial class AxeAbilityController : Node
{
	[Export]
	private PackedScene axeAbilityScene;

	private Timer timer;

	private float baseDamage = 10;
	private float additionalDamagePercent = 1;

	private double baseWaitTime;

	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		timer.Timeout += OnTimerTimeOut;

		baseWaitTime = timer.WaitTime;

		GameEvent.Instance.AbilityUpgradeAdded += OnAbilityUpgradeAdded;
	}

	private void OnTimerTimeOut()
	{
		var player =
			GetTree().GetFirstNodeInGroup("player") as Player;

		if (player == null)
			return;

		var foreground =
			GetTree().GetFirstNodeInGroup("foreground_layer") as Node2D;

		if (foreground == null)
			return;

		var axeInstance =
			axeAbilityScene.Instantiate<AxeAbility>();

		foreground.AddChild(axeInstance);

		axeInstance.GlobalPosition =
			player.GlobalPosition;

		axeInstance.hitBoxComponent.Damage =
			baseDamage * additionalDamagePercent;
	}

	private void OnAbilityUpgradeAdded(
		AbilityUpgrade upgrade,
		Dictionary<string, CurrentUpgrade> currentUpgrade)
	{
		if (upgrade.id == "AxeDamage")
		{
			var quantity =
				currentUpgrade["AxeDamage"].Quantity;

			additionalDamagePercent =
				1.0f + quantity * 0.10f;

			GD.Print($"Axe Damage Level: {quantity}");
			GD.Print(
				$"Axe Damage: {baseDamage * additionalDamagePercent}"
			);
		}
	}
}