using Godot;
using Godot.Collections;

public partial class GameEvent : Node
{
	public static GameEvent Instance { get; private set; }

	[Signal]
	public delegate void ExperienceVialCollectedEventHandler(float number);

	[Signal]
	public delegate void AbilityUpgradeAddedEventHandler(
		AbilityUpgrade upgrade,
		Dictionary<string, CurrentUpgrade> currentUpgrades
	);

	public override void _Ready()
	{
		Instance = this;
	}

	public void EmitExperienceVialCollected(float number)
	{
		EmitSignal(
			SignalName.ExperienceVialCollected,
			number
		);
	}

	public void EmitAbilityUpgradeAdded(
		AbilityUpgrade upgrade,
		Dictionary<string, CurrentUpgrade> currentUpgrades
	)
	{
		EmitSignal(
			SignalName.AbilityUpgradeAdded,
			upgrade,
			currentUpgrades
		);
	}
}