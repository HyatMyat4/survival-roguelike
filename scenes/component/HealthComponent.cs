using Godot;

public partial class HealthComponent : Node
{
	[Signal]
	public delegate void DiedEventHandler();

	[Signal]
	public delegate void HealthChangedEventHandler();

	[Export]
	private float maxHealth = 10f;

	public float currentHealth;

	public override void _Ready()
	{
		currentHealth = maxHealth;
	}

	public void Damage(float damage)
	{
		currentHealth = Mathf.Max(currentHealth - damage, 0f);
		EmitSignal(SignalName.HealthChanged);
		if (currentHealth <= 0f)
		{
			Callable.From(CheckDeath).CallDeferred();
		}
	}

	public float GetHealthPercent()
	{
		if (maxHealth <= 0f)
			return 0f;

		return Mathf.Clamp(currentHealth / maxHealth, 0f, 1f);
	}

	private void CheckDeath()
	{
		if (currentHealth <= 0f)
		{
			EmitSignal(SignalName.Died);
			Owner?.QueueFree();
		}
	}
}