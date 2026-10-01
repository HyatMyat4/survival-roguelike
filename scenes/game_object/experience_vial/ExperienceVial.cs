using Godot;

public partial class ExperienceVial : Node2D
{
	private Area2D area2D;
	private Sprite2D sprite2D;

	[Export]
	private float rotationSpeed = 15f;

	[Export]
	private float collectDuration = 0.5f;

	[Export]
	private float shrinkDuration = 0.15f;

	private bool isCollecting = false;

	public override void _Ready()
	{
		area2D = GetNode<Area2D>("Area2D");
		sprite2D = GetNode<Sprite2D>("Sprite2D");

		area2D.AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Area2D area)
	{
		if (isCollecting)
			return;

		GD.Print("Area entered: " + area.Name);

		var player = GetTree().GetFirstNodeInGroup("player") as Player;

		if (player == null)
		{
			GD.PrintErr("Player not found!");
			return;
		}

		isCollecting = true;

		DisableCollision();
		TweenCollect(player);
	}

	private void DisableCollision()
	{
		area2D.SetDeferred("monitoring", false);
		area2D.SetDeferred("monitorable", false);
	}

	private void TweenCollect(Player player)
	{
		Vector2 startPosition = GlobalPosition;

		Tween tween = CreateTween();

		tween.SetParallel();

		tween.TweenMethod(
			Callable.From<float>((percent) =>
			{
				if (!IsInstanceValid(player))
					return;

				// Get the player's CURRENT position.
				Vector2 targetPosition = player.GlobalPosition;

				// Move toward the player's current position.
				Vector2 globalPosition = startPosition.Lerp(
					targetPosition,
					percent
				);

				Vector2 direction =
					targetPosition - globalPosition;

				if (direction.LengthSquared() > 0)
				{
					float targetRotation =
						direction.Angle() + Mathf.Pi / 2f;

					Rotation = Mathf.LerpAngle(
						Rotation,
						targetRotation,
						(float)(rotationSpeed * GetProcessDeltaTime())
					);
				}

				GlobalPosition = globalPosition;
			}),
			0f,
			1f,
			collectDuration
		)
		.SetEase(Tween.EaseType.In)
		.SetTrans(Tween.TransitionType.Back);

		tween.SetParallel(false);

		tween.TweenProperty(
			sprite2D,
			"scale",
			Vector2.Zero,
			shrinkDuration
		);

		tween.TweenCallback(
			Callable.From(() =>
			{
				GameEvent.Instance.EmitExperienceVialCollected(1f);
				QueueFree();
			})
		);
	}
}