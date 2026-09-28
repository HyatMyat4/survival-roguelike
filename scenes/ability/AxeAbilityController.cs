using Godot;

public partial class AxeAbilityController : Node
{
	[Export]
	private PackedScene axeAbilityScene;

	private Timer timer;

	private int dimage = 10;

	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		timer.Timeout += OnTimerTimeOut;
	}

	private void OnTimerTimeOut()
	{
		var player = GetTree().GetFirstNodeInGroup("player") as Player;

		if (player == null) return;

		var foreground = GetTree().GetFirstNodeInGroup("foreground_layer") as Node2D;

		if (foreground == null) return;

		var axeInstance = axeAbilityScene.Instantiate() as AxeAbility;

		foreground.AddChild(axeInstance);

		axeInstance.GlobalPosition = player.GlobalPosition;
		axeInstance.hitBoxComponent.Damage = dimage;
	}

}