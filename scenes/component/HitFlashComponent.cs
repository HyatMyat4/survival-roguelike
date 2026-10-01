using Godot;
using System;

public partial class HitFlashComponent : Node
{
	[Export]
	private HealthComponent healthComponent;

	[Export]
	private Sprite2D sprite2D;

	private Tween hitFlashTween;
	private ShaderMaterial hitFlashMaterial;

	private readonly ShaderMaterial hitFlashMaterialResource =
		ResourceLoader.Load<ShaderMaterial>(
			"res://resources/theme/hitflash_shader_material.tres"
		);

	public override void _Ready()
	{
		if (healthComponent == null)
		{
			GD.PrintErr("HitFlashComponent: HealthComponent is not assigned.");
			return;
		}

		if (sprite2D == null)
		{
			GD.PrintErr("HitFlashComponent: Sprite2D is not assigned.");
			return;
		}

		if (hitFlashMaterialResource == null)
		{
			GD.PrintErr("HitFlashComponent: Hit flash material could not be loaded.");
			return;
		}

		hitFlashMaterial =
			(ShaderMaterial)hitFlashMaterialResource.Duplicate();

		sprite2D.Material = hitFlashMaterial;

		hitFlashMaterial.SetShaderParameter("flash_amount", 0.0f);

		healthComponent.HealthChanged += OnHealthChanged;
	}

	private void OnHealthChanged()
	{
		if (hitFlashMaterial == null)
			return;

		if (hitFlashTween != null && hitFlashTween.IsValid())
		{
			hitFlashTween.Kill();
		}

		// Start red
		hitFlashMaterial.SetShaderParameter(
			"flash_amount",
			1.0f
		);

		// Red -> normal
		hitFlashTween = CreateTween();

		hitFlashTween.TweenProperty(
			hitFlashMaterial,
			"shader_parameter/flash_amount",
			0.0f,
			0.25f
		);
	}
}