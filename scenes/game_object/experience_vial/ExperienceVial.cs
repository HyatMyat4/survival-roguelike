using Godot;

public partial class ExperienceVial : Node2D
{
	private Area2D area2D;

	public override void _Ready()
	{
		area2D = GetNode<Area2D>("Area2D");
		area2D.AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Area2D area)
	{
		GD.Print("Area entered: " + area.Name);

		GameEvent.Instance.EmitExperienceVialCollected(1f);

		QueueFree();
	}
}