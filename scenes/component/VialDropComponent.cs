using Godot;

public partial class VialDropComponent : Node
{
	[Export]
	private PackedScene vialScene;

	[Export]
	private HealthComponent healthComponent;

	[Export(PropertyHint.Range, "0,100,1")]
	private float dropPercent = 25f;

	public override void _Ready()
	{
		if (healthComponent == null)
			return;

		healthComponent.Died += OnHealthDied;
	}

	private void OnHealthDied()
	{
		CallDeferred(nameof(DropVial));
	}

	private void DropVial()
	{
		if (vialScene == null)
			return;

		if (GD.Randf() > dropPercent / 100f)
			return;

		if (Owner is not Node2D owner)
			return;

		var entitiesLayer = GetTree().GetFirstNodeInGroup("entities_layer");

		if (entitiesLayer == null)
			return;

		Node2D vial = vialScene.Instantiate<Node2D>();

		entitiesLayer.AddChild(vial);
		vial.GlobalPosition = owner.GlobalPosition;
	}
}