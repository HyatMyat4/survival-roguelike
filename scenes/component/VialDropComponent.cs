using Godot;
using System;

public partial class VialDropComponent : Node
{
	[Export]
	private PackedScene vialScene;

	[Export]
	private HealthComponent healthComponent;

	public override void _Ready()
	{
		if (healthComponent == null)
		{
			GD.PrintErr("HealthComponent is not assigned!");
			return;
		}

		healthComponent.Died += OnHealthDied;
	}

	private void OnHealthDied()
	{
		GD.Print("Enemy died! Dropping vial...");
		CallDeferred(nameof(DropVial));
	}

	private void DropVial()
	{
		if (vialScene == null)
		{
			GD.PrintErr("VialScene is not assigned!");
			return;
		}

		if (Owner is Node2D owner)
		{
			Node2D vial = vialScene.Instantiate<Node2D>();

			var entitiesLayer = GetTree().GetFirstNodeInGroup("entities_layer");
			entitiesLayer.AddChild(vial);

			vial.GlobalPosition = owner.GlobalPosition;
		}
	}
}