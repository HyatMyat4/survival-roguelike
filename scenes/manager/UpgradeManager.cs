using Godot;
using Godot.Collections;

public partial class CurrentUpgrade : RefCounted
{
	public AbilityUpgrade Resource { get; set; }
	public int Quantity { get; set; }
}

public partial class UpgradeManager : Node
{
	[Export]
	private Godot.Collections.Array<AbilityUpgrade> upgradePool;

	[Export]
	private ExperienceManager experienceManager;

	[Export]
	private PackedScene upgradeScreenScene;

	private Dictionary<string, CurrentUpgrade> currentUpgrades = new();

	public override void _Ready()
	{
		experienceManager.LevelUp += OnLevelUp;
	}

	private void OnLevelUp(int newLevel)
	{
		if (upgradePool.Count == 0)
			return;

		var chosenUpgrade = upgradePool.PickRandom();

		if (chosenUpgrade == null)
			return;

		var upgradeScreenInstance =
			upgradeScreenScene.Instantiate() as UpgradeScreen;

		if (upgradeScreenInstance == null)
			return;

		AddChild(upgradeScreenInstance);

		var upgrades = new Godot.Collections.Array<AbilityUpgrade>
		{
			chosenUpgrade
		};

		upgradeScreenInstance.SetAbilityUpgrades(upgrades);

		upgradeScreenInstance.UpgradeSelected += OnUpgradeSelected;
	}

	private void OnUpgradeSelected(AbilityUpgrade upgrade)
	{
		ApplyUpgrade(upgrade);
	}

	private void ApplyUpgrade(AbilityUpgrade upgrade)
	{
		if (!currentUpgrades.ContainsKey(upgrade.id))
		{
			currentUpgrades[upgrade.id] = new CurrentUpgrade
			{
				Resource = upgrade,
				Quantity = 1
			};
		}
		else
		{
			currentUpgrades[upgrade.id].Quantity++;
		}

		GameEvent.Instance.EmitAbilityUpgradeAdded(
			upgrade,
			currentUpgrades
		);
	}
}