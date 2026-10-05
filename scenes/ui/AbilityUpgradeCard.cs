using Godot;
using System.Threading.Tasks;

public partial class AbilityUpgradeCard : PanelContainer
{
	private Label NameLabel;
	private Label DescriptionLabel;

	private AnimationPlayer animationPlayer;
	private AnimationPlayer hoverAnimationPlayer;

	private bool disabled = false;

	[Signal]
	public delegate void SelectedEventHandler();

	public override void _Ready()
	{
		NameLabel = GetNode<Label>("%NameLabel");
		DescriptionLabel = GetNode<Label>("%DescriptionLabel");

		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		hoverAnimationPlayer = GetNode<AnimationPlayer>("HoverAnimationPlayer");

		GuiInput += OnGuiInput;
		MouseEntered += OnMouseEntered;
	}

	public void SetAbilityUpgrade(AbilityUpgrade upgrade)
	{
		NameLabel.Text = upgrade.name;
		DescriptionLabel.Text = upgrade.description;
	}

	private void OnGuiInput(InputEvent @event)
	{
		if (disabled)
			return;

		if (@event.IsActionPressed("left_click"))
		{
			SelectedCard();
		}
	}

	public async void PlayIn(float delay)
	{
		Modulate = new Color(1, 1, 1, 0f);
		Scale = Vector2.Zero;

		await ToSignal(
			GetTree().CreateTimer(delay),
			SceneTreeTimer.SignalName.Timeout
		);

		animationPlayer.Play("in");
	}

	private async void SelectedCard()
	{
		if (disabled)
			return;

		disabled = true;

		animationPlayer.Play("selected");

		foreach (Node node in GetTree().GetNodesInGroup("upgrade_card"))
		{
			if (node == this)
				continue;

			if (node is AbilityUpgradeCard otherCard)
			{
				otherCard.PlayDiscard();
			}
		}

		await ToSignal(
			animationPlayer,
			AnimationPlayer.SignalName.AnimationFinished
		);

		EmitSignal(SignalName.Selected);
	}

	private void PlayDiscard()
	{
		disabled = true;
		animationPlayer.Play("discard");
	}

	private void OnMouseEntered()
	{
		if (disabled)
			return;

		hoverAnimationPlayer.Play("hover");
	}
}