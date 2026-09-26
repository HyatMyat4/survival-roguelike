using Godot;
using System;

public partial class BasicEnemy : CharacterBody2D
{
	int MAX_SPEEE = 75;


	private HealthComponent healthComponent;


	public override void _Ready()
	{
		healthComponent = GetNode<HealthComponent>("HealthComponent");
	}


	public override void _Process(double delta)
	{
		var direction = GetDirectionToPlayer();
		Velocity = direction * MAX_SPEEE;
		MoveAndSlide();
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