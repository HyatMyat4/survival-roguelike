using Godot;

public partial class ExperienceManager : Node
{
	[Signal]
	public delegate void ExperienceUpdatedEventHandler(
		float currentExperience,
		float targetExperience
	);

	[Signal]
	public delegate void LevelUpEventHandler(int newLevel);

	const float TargetExperienceGrowth = 5;

	public float CurrentExperience { get; private set; } = 0;
	public int CurrentLevel { get; private set; } = 1;
	public float TargetExperience { get; private set; } = 5;

	public override void _Ready()
	{
		GameEvent.Instance.ExperienceVialCollected += OnExperienceVialCollected;
	}

	private void IncrementExperience(float number)
	{
		CurrentExperience = Mathf.Min(
			CurrentExperience + number,
			TargetExperience
		);

		EmitSignal(
			SignalName.ExperienceUpdated,
			CurrentExperience,
			TargetExperience
		);

		if (CurrentExperience == TargetExperience)
		{
			CurrentLevel += 1;
			TargetExperience += TargetExperienceGrowth;
			CurrentExperience = 0;
			EmitSignal(
				SignalName.ExperienceUpdated,
				CurrentExperience,
				TargetExperience
			);
			EmitSignal(SignalName.LevelUp, CurrentLevel);
		}
	}

	private void OnExperienceVialCollected(float number)
	{
		IncrementExperience(number);

		GD.Print("Current Experience: " + CurrentExperience);
	}
}