using Godot;

public partial class HurtboxComponent : Area2D
{
    [Export]
    private HealthComponent healthComponent;

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (healthComponent == null)
            return;

        if (area is HitboxComponent hitbox)
        {
            healthComponent.Damage(hitbox.Damage);
        }
    }
}