using Godot;
using System;

public partial class BasicEnemy : CharacterBody2D
{
	private HealthComponent healthComponent;
	private Node2D visual;
	private VelocityComponent velocityComponent;

	private HurtboxComponent hurtBoxComponent;

	private RandomStreamPlayer2dComponent audioStreamPlayer;

	public override void _Ready()
	{
		hurtBoxComponent = GetNode<HurtboxComponent>("HurtboxComponent");
		audioStreamPlayer = GetNode<RandomStreamPlayer2dComponent>("RandomStreamPlayer2DComponent");
		visual = GetNode<Node2D>(
			"Visuals"
		);

		velocityComponent = GetNode<VelocityComponent>(
			"VelocityComponent"
		);

		hurtBoxComponent.Hit += OnHit;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (velocityComponent == null)
		{
			return;
		}

		Vector2 direction = GetDirectionToPlayer();

		velocityComponent.AccelerateToPlayer(
			direction,
			delta
		);

		velocityComponent.Move(this);

		UpdateVisualDirection(direction);
	}

	private Vector2 GetDirectionToPlayer()
	{
		var playerNode = GetTree()
			.GetFirstNodeInGroup("player") as Node2D;

		if (playerNode != null)
		{
			return (
				playerNode.GlobalPosition -
				GlobalPosition
			).Normalized();
		}

		return Vector2.Zero;
	}

	private void UpdateVisualDirection(Vector2 direction)
	{
		var moveSign = Mathf.Sign(direction.X);

		if (moveSign != 0)
		{
			visual.Scale = new Vector2(
				Mathf.Abs(visual.Scale.X) * moveSign,
				visual.Scale.Y
			);
		}
	}

	private void OnHit()
	{
		audioStreamPlayer.PlayRandom();
	}

}