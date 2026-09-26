using Godot;

public partial class GameCamera : Camera2D
{
	[Export]
	private float smoothSpeed = 20f;

	private Node2D player;

	public override void _Ready()
	{
		MakeCurrent();

		var playerNodes = GetTree().GetNodesInGroup("player");

		if (playerNodes.Count > 0 && playerNodes[0] is Node2D target)
		{
			player = target;
			GlobalPosition = player.GlobalPosition;
		}
	}

	public override void _Process(double delta)
	{
		if (player == null)
			return;

		float damping = 1f - Mathf.Exp(-smoothSpeed * (float)delta);

		GlobalPosition = GlobalPosition.Lerp(
			player.GlobalPosition,
			damping
		);
	}
}