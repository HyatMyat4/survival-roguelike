using Godot;
using Godot.Collections;

public partial class WeightedTable : Node
{
	private Array<Dictionary> items = new();

	private float weightSum = 0f;

	public void AddItem(PackedScene item, float weight)
	{
		if (item == null)
		{
			GD.PrintErr("Cannot add a null PackedScene.");
			return;
		}

		if (weight <= 0f)
		{
			GD.PrintErr("Weight must be greater than 0.");
			return;
		}

		items.Add(new Dictionary
		{
			{ "item", item },
			{ "weight", weight }
		});

		weightSum += weight;
	}

	public PackedScene PickItem()
	{
		if (items.Count == 0 || weightSum <= 0f)
		{
			GD.PrintErr("WeightedTable is empty.");
			return null;
		}

		float chosenWeight =
			(float)GD.RandRange(0.0, weightSum);

		foreach (Dictionary item in items)
		{
			float weight = (float)item["weight"];

			if (chosenWeight <= weight)
			{
				return (PackedScene)item["item"];
			}

			chosenWeight -= weight;
		}

		return null;
	}
}