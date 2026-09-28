using Godot;
using System;

public partial class BasicEnemy : CharacterBody2D
{
	int MAX_SPEEE = 75;


	private HealthComponent healthComponent;

	private Node2D visual;


	public override void _Ready()
	{
		healthComponent = GetNode<HealthComponent>("HealthComponent");
		visual = GetNode<Node2D>("Visuals");
	}


	public override void _Process(double delta)
	{
		var direction = GetDirectionToPlayer();
		Velocity = direction * MAX_SPEEE;

		MoveAndSlide();


		var moveSign = Mathf.Sign(direction.X);

		if (moveSign != 0)
		{
			visual.Scale = new Vector2(
				Mathf.Abs(visual.Scale.X) * moveSign,
				visual.Scale.Y
			);
		}


	}

	private Vector2 GetDirectionToPlayer()
	{
		var playerNode = GetTree().GetFirstNodeInGroup("player") as Node2D;

		if (playerNode != null)
		{
			return (playerNode.GlobalPosition - GlobalPosition).Normalized();
		}



		return Vector2.Zero;
	}

	private void OnAreaEntered(Area2D otherArea)
	{
		healthComponent.Damage(100);
	}
}