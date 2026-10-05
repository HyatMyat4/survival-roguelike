using Godot;
using Godot.Collections;

public partial class Player : CharacterBody2D
{
	private int numberCollidingBodies = 0;

	private Area2D collisionArea;
	private Timer damageIntervalTimer;
	private ProgressBar healthBar;
	private Node abilities;
	private AnimationPlayer animationPlayer;
	private Node2D visual;
	private VelocityComponent velocityComponent;

	public HealthComponent healthComponent;

	private float baseSpeed = 0;

	public override void _Ready()
	{

		velocityComponent = GetNode<VelocityComponent>("VelocityComponent");
		baseSpeed = velocityComponent.maxSpeed;

		collisionArea = GetNode<Area2D>("CollisionArea2D");
		healthComponent = GetNode<HealthComponent>("HealthComponent");
		damageIntervalTimer = GetNode<Timer>("Timer");
		healthBar = GetNode<ProgressBar>("HealthBar");
		abilities = GetNode<Node>("Abilites");
		visual = GetNode<Node2D>("Visual");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		collisionArea.BodyEntered += OnBodyEntered;
		collisionArea.BodyExited += OnBodyExited;
		damageIntervalTimer.Timeout += OnDamageIntervalTimerTimeout;
		healthComponent.HealthChanged += OnHealthChanged;
		GameEvent.Instance.AbilityUpgradeAdded += OnAbilityUpgradeAdded;

		UpdateHealthDisplay();
	}

	public override void _PhysicsProcess(double delta)
	{
		var direction = GetMovementVector();

		velocityComponent.AccelerateInDirection(direction, delta);
		velocityComponent.Move(this);

		if (direction != Vector2.Zero)
		{
			animationPlayer.Play("Walk");
		}
		else
		{
			animationPlayer.Play("RESET");
		}

		var moveSign = Mathf.Sign(direction.X);

		if (moveSign != 0)
		{
			visual.Scale = new Vector2(
				Mathf.Abs(visual.Scale.X) * moveSign,
				visual.Scale.Y
			);
		}
	}

	private static Vector2 GetMovementVector()
	{
		float x =
			Input.GetActionStrength("move_right")
			- Input.GetActionStrength("move_left");

		float y =
			Input.GetActionStrength("move_down")
			- Input.GetActionStrength("move_up");

		return new Vector2(x, y);
	}

	private void CheckDealDamage()
	{
		if (
			numberCollidingBodies == 0
			|| !damageIntervalTimer.IsStopped()
		)
		{
			return;
		}

		healthComponent.Damage(1);
		damageIntervalTimer.Start();

		GD.Print("HealthComponent: ", healthComponent.currentHealth);
	}

	private void UpdateHealthDisplay()
	{
		healthBar.Value = healthComponent.GetHealthPercent();
	}

	private void OnBodyEntered(Node2D body)
	{
		numberCollidingBodies++;
		CheckDealDamage();
	}

	private void OnBodyExited(Node2D body)
	{
		numberCollidingBodies--;
	}

	private void OnDamageIntervalTimerTimeout()
	{
		CheckDealDamage();
	}

	private void OnHealthChanged()
	{
		UpdateHealthDisplay();
		GameEvent.Instance.EmitSignal(
			GameEvent.SignalName.PlayerDamage
		);
	}

	private void OnAbilityUpgradeAdded(
		AbilityUpgrade upgrade,
		Dictionary<string, CurrentUpgrade> currentUpgrades
	)
	{
		if (upgrade is Ability abilityUpgrade)
		{
			abilities.AddChild(
					abilityUpgrade.abilityControllerScene.Instantiate()
				);
		}
		else if (upgrade.id == "PlayerSpeed")
		{
			velocityComponent.maxSpeed =
				baseSpeed + (baseSpeed * currentUpgrades["PlayerSpeed"].Quantity * 1f);
		}
	}
}