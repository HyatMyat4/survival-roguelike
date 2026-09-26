using Godot;

public partial class ArenaTimeManager : Node
{

	[Signal]
	public delegate void ArenaDifficultyIncreasedEventHandler(int arenaDifficulty);

	[Export]
	private PackedScene endScreenScene;
	private Timer timer;

	private int difficultyInterval = 5;

	private int arenaDifficulty = 0;



	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		timer.Timeout += OnTimerTimeOut;
	}

	public override void _Process(double delta)
	{
		var nextTimeTarget = timer.WaitTime - ((arenaDifficulty + 1) * difficultyInterval);
		if (timer.TimeLeft <= nextTimeTarget)
		{
			arenaDifficulty += 1;
			EmitSignal(SignalName.ArenaDifficultyIncreased, arenaDifficulty);
		}
	}

	public double GetTimeElapsed()
	{
		return timer.WaitTime - timer.TimeLeft;
	}

	private void OnTimerTimeOut()
	{
		var endSceneInstance = endScreenScene.Instantiate();
		AddChild(endSceneInstance);
	}
}