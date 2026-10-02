using Godot;
using System;
using System.Collections.Generic;

public class WeightedTable<T> where T : class
{
	private class WeightedItem
	{
		public T Item { get; }
		public float Weight { get; }

		public WeightedItem(T item, float weight)
		{
			Item = item;
			Weight = weight;
		}
	}

	private readonly List<WeightedItem> items = new();

	public int Count => items.Count;

	public void AddItem(T item, float weight)
	{
		if (item == null)
		{
			GD.PrintErr("Cannot add a null item.");
			return;
		}

		if (weight <= 0f)
		{
			GD.PrintErr("Weight must be greater than 0.");
			return;
		}

		items.Add(new WeightedItem(item, weight));
	}

	public T PickItem(IEnumerable<T> exclude)
	{
		var excluded = new HashSet<T>(exclude);

		float availableWeight = 0f;

		foreach (var item in items)
		{
			if (!excluded.Contains(item.Item))
			{
				availableWeight += item.Weight;
			}
		}

		if (availableWeight <= 0f)
			return null;

		float chosenWeight =
			(float)GD.RandRange(0.0, availableWeight);

		foreach (var item in items)
		{
			if (excluded.Contains(item.Item))
				continue;

			chosenWeight -= item.Weight;

			if (chosenWeight <= 0f)
				return item.Item;
		}

		return null;
	}

	public T PickItem()
	{
		return PickItem(Array.Empty<T>());
	}

	public void Remove(T value)
	{
		for (int i = items.Count - 1; i >= 0; i--)
		{
			if (EqualityComparer<T>.Default.Equals(
				items[i].Item,
				value))
			{
				items.RemoveAt(i);
			}
		}
	}
}