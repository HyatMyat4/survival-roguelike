using Godot;
using System;

public partial class DeathComponent : Node2D
{

	[Export]
	private HealthComponent healthComponent;

	[Export]
	private Sprite2D sprite2D;

	private AnimationPlayer animationPlayer;
	// Called when the node enters the scene tree for the first time.
	private GpuParticles2D gpuParticles2D;
	public override void _Ready()
	{


		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		gpuParticles2D = GetNode<GpuParticles2D>("GPUParticles2D");

		gpuParticles2D.Texture = sprite2D.Texture;

		healthComponent.Died += OnDying;
	}


	private void OnDying()
	{
		if (Owner is not Node2D owner)
		{
			return;
		}

		Vector2 spawnPosition = owner.GlobalPosition;

		var entities = GetTree().GetFirstNodeInGroup("entities_layer");

		if (entities == null)
		{
			return;
		}

		GetParent().RemoveChild(this);

		entities.AddChild(this);

		GlobalPosition = spawnPosition;

		animationPlayer.Play("default");

	}

}
