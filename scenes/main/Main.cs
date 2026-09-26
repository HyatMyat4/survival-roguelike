using Godot;
using System;

public partial class Main : Node
{
	private Player player;

	[Export]
	private PackedScene endScreenScene;

	public override void _Ready()
	{
		player = GetNode<Player>("Entities/Player");
		player.healthComponent.Died += OnPlayerDied;
	}

	private void OnPlayerDied()
	{
		var endScreenInstance = endScreenScene.Instantiate<EndScreen>();
		AddChild(endScreenInstance);
		endScreenInstance.SetDefeat();
	}
}