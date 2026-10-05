using Godot;
using System;

public partial class Vignette : CanvasLayer
{
	private AnimationPlayer animationPlayer;

	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		GameEvent.Instance.PlayerDamage += OnPlayerDamage;
	}

	private async void OnPlayerDamage()
	{
		animationPlayer.Play("hit");
	}
}