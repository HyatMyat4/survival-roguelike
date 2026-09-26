using Godot;

public partial class Player : CharacterBody2D
{
	private const float MAX_SPEED = 200f;
	private const float ACCELERATION = 800f;
	private const float DECELERATION = 1000f;

	private int NumberCollidingBodies = 0;

	private Area2D collisionArea;

	public HealthComponent healthComponent;

	private Timer dimageIntervalTimer;

	private ProgressBar healthBar;



	public override void _Ready()
	{
		collisionArea = GetNode<Area2D>("CollisionArea2D");
		healthComponent = GetNode<HealthComponent>("HealthComponent");
		dimageIntervalTimer = GetNode<Timer>("Timer");
		healthBar = GetNode<ProgressBar>("HealthBar");

		collisionArea.BodyEntered += OnBodyEntered;
		collisionArea.BodyExited += OnBodyExited;
		dimageIntervalTimer.Timeout += OnDimageIntervalTimerTimeOut;
		healthComponent.HealthChanged += OnHealthChanged;
		UpdateHealthDisplay();
	}
	public override void _PhysicsProcess(double delta)
	{
		var direction = GetMovementVector().Normalized();
		var targetVelocity = direction * MAX_SPEED;

		float acceleration = direction == Vector2.Zero
			? DECELERATION
			: ACCELERATION;

		Velocity = Velocity.MoveToward(
			targetVelocity,
			acceleration * (float)delta
		);

		MoveAndSlide();
	}

	private static Vector2 GetMovementVector()
	{
		float x = Input.GetActionStrength("move_right")
			- Input.GetActionStrength("move_left");

		float y = Input.GetActionStrength("move_down")
			- Input.GetActionStrength("move_up");

		return new Vector2(x, y);
	}

	private void CheckDealDimage()
	{
		if (NumberCollidingBodies == 0 || !dimageIntervalTimer.IsStopped()) return;
		healthComponent.Damage(1);
		dimageIntervalTimer.Start();
		GD.Print("HealthComponent", healthComponent.currentHealth);
	}

	private void UpdateHealthDisplay()
	{
		healthBar.Value = healthComponent.GetHealthPercent();
	}



	private void OnBodyEntered(Node2D body)
	{

		NumberCollidingBodies += 1;
		CheckDealDimage();
	}

	private void OnBodyExited(Node2D body)
	{
		NumberCollidingBodies -= 1;
	}

	private void OnDimageIntervalTimerTimeOut()
	{
		CheckDealDimage();
	}

	private void OnHealthChanged()
	{
		UpdateHealthDisplay();
	}
}