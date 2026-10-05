using Godot;
using System;

public partial class UpgradeScreen : CanvasLayer
{

	[Signal]
	public delegate void UpgradeSelectedEventHandler(AbilityUpgrade upgrade);

	[Export]
	public PackedScene UpgradeCardScene;

	private HBoxContainer CardContainer;

	private AnimationPlayer animationPlayer;

	public override void _Ready()
	{
		CardContainer = GetNode<HBoxContainer>("%CardContainer");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		GetTree().Paused = true;
	}

	public void SetAbilityUpgrades(
		Godot.Collections.Array<AbilityUpgrade> upgrades
	)
	{
		float delay = 0f;

		foreach (var upgrade in upgrades)
		{
			var cardInstance =
				UpgradeCardScene.Instantiate<AbilityUpgradeCard>();

			CardContainer.AddChild(cardInstance);

			cardInstance.SetAbilityUpgrade(upgrade);
			cardInstance.PlayIn(delay);

			cardInstance.Selected += () => OnUpgradeSelected(upgrade);

			delay += 0.2f;
		}
	}

	private async void OnUpgradeSelected(AbilityUpgrade upgrade)
	{
		EmitSignal(SignalName.UpgradeSelected, upgrade);

		animationPlayer.Play("out");

		await ToSignal(
			animationPlayer,
			AnimationPlayer.SignalName.AnimationFinished
		);

		GetTree().Paused = false;
		QueueFree();
	}
}