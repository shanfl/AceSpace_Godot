using Godot;
using System;
using System.Collections.Generic;

public partial class PowerUp : HitBox
{
	[Export] float _speed = 100;
	[Export] private Sprite2D _sprite;
	[Export] private AudioStreamPlayer2D _sound;
	private static readonly Dictionary<Defs.PowerUpType,Texture2D> PowerUpTextures = new Dictionary<Defs.PowerUpType, Texture2D>
	{
		{Defs.PowerUpType.Health,GD.Load<Texture2D>("res://assets/misc/powerupGreen_bolt.png")},
		{Defs.PowerUpType.Shield,GD.Load<Texture2D>("res://assets/misc/shield_gold.png")},
	} ;


	private Defs.PowerUpType _powerType = Defs.PowerUpType.Health;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_sprite.Texture = PowerUpTextures[_powerType];
		SoundManager.PlayPowerUpDeploySound(_sound);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += new Vector2(0,_speed*(float)delta);
	}


    protected override void OnAreaEnter(Area2D area)
    {
        //base.OnAreaEnter(area);
		QueueFree();
    }


	public void SetupPowerUpType(Defs.PowerUpType type)
	{
		_powerType = type;
	}

	public Defs.PowerUpType GetPowerUpType()
	{
		return _powerType;
	}

}
