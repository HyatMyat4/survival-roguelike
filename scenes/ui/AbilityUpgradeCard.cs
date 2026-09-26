using Godot;
using System;

public partial class AbilityUpgradeCard : PanelContainer
{
	private Label NameLabel;
	private Label DescriptionLabel;

	[Signal]
	public delegate void SelectedEventHandler();

	public override void _Ready()
	{
		NameLabel = GetNode<Label>("%NameLabel");
		DescriptionLabel = GetNode<Label>("%DescriptionLabel");

		GuiInput += OnGuiInput;
	}

	public void SetAbilityUpgrade(AbilityUpgrade upgrade)
	{
		NameLabel.Text = upgrade.name;
		DescriptionLabel.Text = upgrade.description;
	}

	private void OnGuiInput(InputEvent @event)
	{
		if (@event.IsActionPressed("left_click"))
		{
			EmitSignal(SignalName.Selected);
			GD.Print("Upgrade card clicked!");
		}
	}
}