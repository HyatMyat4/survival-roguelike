using Godot;
using System;

public partial class RandomStreamPlayer2dComponent : AudioStreamPlayer2D
{
	[Export]
	private AudioStream[] streams;

	[Export]
	private float minPitch = 0.9f;

	[Export]
	private float maxPitch = 1.9f;

	private RandomNumberGenerator random = new();

	public void PlayRandom()
	{
		if (streams == null || streams.Length == 0)
			return;

		Stream = streams[random.RandiRange(0, streams.Length - 1)];

		PitchScale = random.RandfRange(minPitch, maxPitch);

		Play();
	}
}