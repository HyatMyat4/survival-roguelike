using Godot;
using System;

public partial class EndScreen : CanvasLayer
{

	private Button restartBtn;
	private Button quitBtn;

	private Label tittleLabel;

	private Label descriptionLabel;

	private PanelContainer panelContainer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		panelContainer = GetNode<PanelContainer>("%PanelContainer");
		GetTree().Paused = true;
		var tween = CreateTween();
		panelContainer.PivotOffset = panelContainer.Size / 2;

		tween.TweenProperty(panelContainer, "scale", Vector2.Zero, 0);
		tween.TweenProperty(panelContainer, "scale", Vector2.One, .3).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);


		restartBtn = GetNode<Button>("%RestartButton");
		quitBtn = GetNode<Button>("%QuitButton");
		tittleLabel = GetNode<Label>("%TittleLabel");
		descriptionLabel = GetNode<Label>("%DescriptionLabel");


		restartBtn.Pressed += OnReStartButtonPressed;
		quitBtn.Pressed += OnQuitButtonPressed;
	}

	public void SetDefeat()
	{
		tittleLabel.Text = "Defeat";
		descriptionLabel.Text = "You lost!";
	}

	private void OnReStartButtonPressed()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://scenes/main/main.tscn");
	}

	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
}
