using Godot;
using System;

public partial class Explosion : AnimatedSprite2D
{
	const string ANI_BOOM = "boom";
	const string ANI_EXPLOSION = "explosion";

	[Export] AudioStreamPlayer2D _sound;
	Defs.ExplosionType _type;
	// Called w
	// hen the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AnimationFinished += OnAnimationFinished;
		SoundManager.PlayExplosionRandom(_sound);
		if(_type == Defs.ExplosionType.Boom)
		{
			this.Animation = ANI_BOOM;

		}else if(_type == Defs.ExplosionType.Explosion)
		{
			this.Animation = ANI_EXPLOSION;
		}
	}

	public void Setup(Defs.ExplosionType type)
	{
			_type = type;
	}

    private void OnAnimationFinished()
    {
       QueueFree();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
