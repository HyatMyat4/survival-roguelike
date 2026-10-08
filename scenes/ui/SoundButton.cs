using Godot;
using System;

public partial class SoundButton : Button
{
	private RandomAudioStreamPlayer audioStreamPlayer;

	public override void _Ready()
	{
		audioStreamPlayer = GetNode<RandomAudioStreamPlayer>("RandomAudioStreamPlayer");

		Pressed += OnPressed;
	}

	private void OnPressed()
	{
		audioStreamPlayer.PlayRandom();
	}
}