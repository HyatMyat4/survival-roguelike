using Godot;
using System;

public partial class EndScreen : CanvasLayer
{

	private Button restartBtn;
	private Button quitBtn;

	private Label tittleLabel;

	private Label descriptionLabel;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetTree().Paused = true;

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
