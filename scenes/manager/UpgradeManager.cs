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
	private ExperienceManager experienceManager;

	[Export]
	private PackedScene upgradeScreenScene;

	private Dictionary<string, CurrentUpgrade> currentUpgrades = new();

	private WeightedTable<AbilityUpgrade> upgradePool = new();

	private AbilityUpgrade axeUpgrade =
		ResourceLoader.Load<AbilityUpgrade>(
			"res://resources/upgrades/axe.tres"
		);

	private AbilityUpgrade axeDamage =
		ResourceLoader.Load<AbilityUpgrade>(
			"res://resources/upgrades/axe_damage.tres"
		);

	private AbilityUpgrade swordRate =
		ResourceLoader.Load<AbilityUpgrade>(
			"res://resources/upgrades/sword_rate.tres"
		);

	private AbilityUpgrade swordDamage =
		ResourceLoader.Load<AbilityUpgrade>(
			"res://resources/upgrades/sword_damage.tres"
		);

	private AbilityUpgrade playerSpeed = ResourceLoader.Load<AbilityUpgrade>("res://resources/upgrades/player_speed.tres");

	public override void _Ready()
	{
		if (experienceManager == null)
		{
			GD.PrintErr(
				"UpgradeManager: ExperienceManager is not assigned."
			);
			return;
		}

		if (upgradeScreenScene == null)
		{
			GD.PrintErr(
				"UpgradeManager: Upgrade Screen Scene is not assigned."
			);
			return;
		}

		if (axeUpgrade == null)
		{
			GD.PrintErr(
				"UpgradeManager: Failed to load axe.tres"
			);
			return;
		}

		if (swordRate == null)
		{
			GD.PrintErr(
				"UpgradeManager: Failed to load sword_rate.tres"
			);
			return;
		}

		if (swordDamage == null)
		{
			GD.PrintErr(
				"UpgradeManager: Failed to load sword_damage.tres"
			);
			return;
		}

		upgradePool.AddItem(axeUpgrade, 10);
		upgradePool.AddItem(swordRate, 10);
		upgradePool.AddItem(swordDamage, 10);
		upgradePool.AddItem(playerSpeed, 10000);

		experienceManager.LevelUp += OnLevelUp;
	}

	private Array<AbilityUpgrade> PickUpgrades(int amount)
	{
		var chosenUpgrades =
			new Array<AbilityUpgrade>();

		for (int i = 0; i < amount; i++)
		{
			var chosenUpgrade =
				upgradePool.PickItem(chosenUpgrades);

			if (chosenUpgrade == null)
				break;

			chosenUpgrades.Add(chosenUpgrade);
		}

		return chosenUpgrades;
	}

	private void OnUpgradeSelected(AbilityUpgrade upgrade)
	{
		ApplyUpgrade(upgrade);
		UpdateUpgradePool(upgrade);
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
			var currentQuantity =
				currentUpgrades[upgrade.id].Quantity;

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
		{
			GD.PrintErr(
				"UpgradeManager: Failed to instantiate UpgradeScreen."
			);
			return;
		}

		AddChild(upgradeScreenInstance);

		var upgrades = PickUpgrades(2);

		upgradeScreenInstance.SetAbilityUpgrades(upgrades);

		upgradeScreenInstance.UpgradeSelected +=
			OnUpgradeSelected;
	}

	private void UpdateUpgradePool(AbilityUpgrade chosenUpgrade)
	{
		if (chosenUpgrade.id == axeUpgrade.id)
		{
			if (axeDamage != null)
			{
				upgradePool.AddItem(axeDamage, 10);
			}
		}
	}
}