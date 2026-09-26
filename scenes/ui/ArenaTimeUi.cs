using Godot;

public partial class ArenaTimeUi : CanvasLayer
{
	[Export]
	private ArenaTimeManager arenaTimeManager;

	private Label label;

	public override void _Ready()
	{
		label = GetNode<Label>("MarginContainer/Label");
	}

	public override void _Process(double delta)
	{
		if (arenaTimeManager == null || label == null)
			return;

		double time = arenaTimeManager.GetTimeElapsed();

		label.Text = FormatSecondToString((float)time);
	}

	private string FormatSecondToString(float seconds)
	{
		int minutes = Mathf.FloorToInt(seconds / 60);
		int remainingSeconds = Mathf.FloorToInt(seconds % 60);

		return $"{minutes:00}:{remainingSeconds:00}";
	}
}