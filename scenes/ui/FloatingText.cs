using Godot;

public partial class FloatingText : Node2D
{
	private Label label;

	public override void _Ready()
	{
		label = GetNode<Label>("Label");
	}

	public void Start(string text)
	{
		label.Text = text;

		// Start small
		Scale = Vector2.One * 0.5f;

		Tween tween = CreateTween();

		// Move upward + grow at the same time
		tween.SetParallel();

		tween.TweenProperty(
			this,
			"global_position",
			GlobalPosition + Vector2.Up * 50f,
			0.3f
		)
		.SetEase(Tween.EaseType.Out)
		.SetTrans(Tween.TransitionType.Cubic);

		tween.TweenProperty(
			this,
			"scale",
			Vector2.One * 1.2f,
			0.3f
		)
		.SetEase(Tween.EaseType.Out)
		.SetTrans(Tween.TransitionType.Cubic);

		// Continue after both animations finish
		tween.SetParallel(false);

		// Shrink back to normal
		tween.TweenProperty(
			this,
			"scale",
			Vector2.One,
			0.3f
		)
		.SetEase(Tween.EaseType.InOut)
		.SetTrans(Tween.TransitionType.Cubic);

		// Move further upward + scale out
		tween.SetParallel();

		tween.TweenProperty(
			this,
			"global_position",
			GlobalPosition + Vector2.Up * 80f,
			0.4f
		)
		.SetEase(Tween.EaseType.In)
		.SetTrans(Tween.TransitionType.Cubic);

		tween.TweenProperty(
			this,
			"scale",
			Vector2.One * 0.1f,
			0.4f
		)
		.SetEase(Tween.EaseType.In)
		.SetTrans(Tween.TransitionType.Cubic);

		// Delete after animation
		tween.SetParallel(false);
		tween.TweenCallback(Callable.From(QueueFree));
	}
}