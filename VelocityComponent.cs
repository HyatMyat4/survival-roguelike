using Godot;
using System;

public partial class VelocityComponent : Node
{
	[Export]
	private float maxSpeed = 75f;

	[Export]
	private float acceleration = 500f;

	private Vector2 velocity = Vector2.Zero;

	public Vector2 Velocity => velocity;

	public void AccelerateToPlayer(Vector2 direction, double delta)
	{
		AccelerateInDirection(direction, delta);
	}

	public void AccelerateInDirection(Vector2 direction, double delta)
	{
		if (direction == Vector2.Zero)
		{
			Decelerate(delta);
			return;
		}

		Vector2 desiredVelocity =
			direction.Normalized() * maxSpeed;

		velocity = velocity.MoveToward(
			desiredVelocity,
			acceleration * (float)delta
		);
	}

	public void Decelerate(double delta)
	{
		velocity = velocity.MoveToward(
			Vector2.Zero,
			acceleration * (float)delta
		);
	}

	public void Move(CharacterBody2D body)
	{
		body.Velocity = velocity;
		body.MoveAndSlide();
	}
}