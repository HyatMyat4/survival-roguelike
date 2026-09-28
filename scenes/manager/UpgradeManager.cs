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

	private Godot.Collections.Array<AbilityUpgrade> PickUpgrades(int amount)
	{
		var filteredUpgrades = upgradePool.Duplicate();
		var chosenUpgrades = new Godot.Collections.Array<AbilityUpgrade>();

		for (int i = 0; i < amount && filteredUpgrades.Count > 0; i++)
		{

			if (filteredUpgrades.Count == 0)
				break;
			var chosenUpgrade = filteredUpgrades.PickRandom();


			chosenUpgrades.Add(chosenUpgrade);
			filteredUpgrades.Remove(chosenUpgrade);
		}

		return chosenUpgrades;
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

		if (upgrade.maxQuantity > 0)
		{
			var currentQuantity = currentUpgrades[upgrade.id].Quantity;

			if (currentQuantity >= upgrade.maxQuantity)
			{
				upgradePool.Remove(upgrade);
			}
		}

		GameEvent.Instance.EmitAbilityUpgradeAdded(
			upgrade,
			currentUpgrades
		);
	}

	private void OnLevelUp(int newLevel)
	{
		if (upgradePool.Count == 0)
			return;

		var upgradeScreenInstance =
			upgradeScreenScene.Instantiate() as UpgradeScreen;

		if (upgradeScreenInstance == null)
			return;

		AddChild(upgradeScreenInstance);

		var upgrades = PickUpgrades(2);

		upgradeScreenInstance.SetAbilityUpgrades(upgrades);
		upgradeScreenInstance.UpgradeSelected += OnUpgradeSelected;
	}
}