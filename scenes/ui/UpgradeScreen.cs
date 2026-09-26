using Godot;
using System;

public partial class UpgradeScreen : CanvasLayer
{

	[Signal]
	public delegate void UpgradeSelectedEventHandler(AbilityUpgrade upgrade);

	[Export]
	public PackedScene UpgradeCardScene;

	private HBoxContainer CardContainer;

	public override void _Ready()
	{
		CardContainer = GetNode<HBoxContainer>("%CardContainer");

		GetTree().Paused = true;
	}

	public void SetAbilityUpgrades(
		Godot.Collections.Array<AbilityUpgrade> upgrades
	)
	{
		foreach (var upgrade in upgrades)
		{
			var cardInstance =
				UpgradeCardScene.Instantiate<AbilityUpgradeCard>();

			CardContainer.AddChild(cardInstance);

			cardInstance.SetAbilityUpgrade(upgrade);

			cardInstance.Selected += () => OnUpgradeSelected(upgrade);
		}
	}

	private void OnUpgradeSelected(AbilityUpgrade upgrade)
	{
		GD.Print($"Selected upgrade: {upgrade.name}");
		EmitSignal(SignalName.UpgradeSelected, upgrade);
		GetTree().Paused = false;
		QueueFree();
	}
}