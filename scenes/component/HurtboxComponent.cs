using System;
using Godot;

public partial class HurtboxComponent : Area2D
{

    [Signal]
    public delegate void HitEventHandler();

    [Export]
    private HealthComponent healthComponent;

    private PackedScene floatingTextScene =
        ResourceLoader.Load<PackedScene>("res://scenes/ui/FloatingText.tscn");

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

            var floatingText =
                floatingTextScene.Instantiate<FloatingText>();

            GetTree()
                .GetFirstNodeInGroup("foreground_layer")
                .AddChild(floatingText);

            floatingText.GlobalPosition =
                GlobalPosition + (Vector2.Up * 16);

            var damage = Mathf.RoundToInt(hitbox.Damage);

            floatingText.Start(damage.ToString());

            EmitSignal(SignalName.Hit);
        }
    }
}