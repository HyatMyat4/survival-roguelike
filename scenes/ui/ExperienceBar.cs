using Godot;

public partial class ExperienceBar : CanvasLayer
{
	[Export]
	private ExperienceManager experienceManager;

	private ProgressBar progressBar;

	public override void _Ready()
	{
		progressBar = GetNode<ProgressBar>("MarginContainer/ProgressBar");

		// Progress range
		progressBar.MinValue = 0;
		progressBar.MaxValue = 1;
		progressBar.Value = 0;

		// XP bar color
		var fillStyle = new StyleBoxFlat();
		fillStyle.BgColor = new Color("#43e1b3");
		fillStyle.BorderColor = new Color("#3f2631");
		fillStyle.SetBorderWidthAll(2);


		progressBar.AddThemeStyleboxOverride("fill", fillStyle);

		if (experienceManager == null)
		{
			GD.PrintErr("ExperienceManager is not assigned!");
			return;
		}

		experienceManager.ExperienceUpdated += OnExperienceUpdated;
	}

	private void OnExperienceUpdated(float currentExperience, float targetExperience)
	{
		if (targetExperience <= 0)
		{
			progressBar.Value = 0;
			return;
		}

		float percent = currentExperience / targetExperience;

		progressBar.Value = percent;

		GD.Print($"XP Updated: {percent * 100f}%");
	}
}