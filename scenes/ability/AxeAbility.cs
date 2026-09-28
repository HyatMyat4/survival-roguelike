using Godot;
using System;

public partial class AxeAbility : Node2D
{
	int MaxRadius = 100;

	public HitboxComponent hitBoxComponent;

	Vector2 baseRotation = Vector2.Right;

	public override void _Ready()
	{
		baseRotation = Vector2.Right.Rotated(
			(float)GD.RandRange(0.0, Mathf.Tau)
		);
		hitBoxComponent = GetNode<HitboxComponent>("HitboxComponent");
		var tween = CreateTween();

		tween.TweenMethod(
			Callable.From<float>(TweenMethodFunc),
			0.0f,
			2.0f,
			3.0f
		);
		tween.TweenCallback(Callable.From(QueueFree));
	}

	private void TweenMethodFunc(float rotations)
	{
		GD.Print(rotations);

		var percent = rotations / 2;
		var currentRadius = percent * MaxRadius;
		var currentDirection = baseRotation.Rotated(rotations * Mathf.Tau);


		var player = GetTree().GetFirstNodeInGroup("player") as Player;

		if (player == null) return;

		GlobalPosition = player.GlobalPosition + (currentDirection * currentRadius);
	}
}