using Godot;
using System;

public partial class Player : Area2D
{

    private const float MARGIN = 16.0f;

    [Export] private float _speed = 250.0f;
    [Export] private float _bulletSpeed = 250.0f;
    [Export] private Vector2 _bulletDirection = Vector2.Up;
    [Export] private int _healthBoost = 20;

    [Export] private Sprite2D _sprite2D;
    [Export] private AnimationPlayer _animationPlayer;

    [Export]private Shield _shield;

    private Vector2 _upperLeft;
    private Vector2 _lowerRight;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        AreaEntered += OnAreaEntered;
        SetLimits();
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Vector2 desirePosition = GlobalPosition + GetInput()*(float)delta * _speed;
        GlobalPosition = desirePosition.Clamp(_upperLeft,_lowerRight);

        if (Input.IsActionJustPressed("shoot"))
        {
            Shoot();
        }

    }

    private Vector2 GetInput()
    {
        Vector2 v = new Vector2(Input.GetAxis("left","right"),Input.GetAxis("up","down"));
        //GD.Print($"v.x: {v.X}, v.y: {v.Y}, v.Length(): {v.Length()} v.Normalized(): {v.Normalized()}");
        

        if(v.X != 0)
        {
            _animationPlayer.Play("turn");
            _sprite2D.FlipH = v.X>0;
        }
        else
        {
            _animationPlayer.Play("fly");
        }

        return v.Normalized();
    }

    private void Shoot()
    {
        SignalManager.EmitOnCreateBullet(GlobalPosition,_bulletDirection,_bulletSpeed,(int)Defs.BulletType.Player);    
    }

    private void SetLimits()
    {
        var vp = GetViewportRect();
        _lowerRight = new Vector2(vp.Size.X - MARGIN, vp.Size.Y - MARGIN);
        _upperLeft = new Vector2(MARGIN, MARGIN);
    }

    private void OnAreaEntered(Area2D area)
    {
        if(area is PowerUp)
        {
            var powerUp = area as PowerUp;
            if(powerUp.GetPowerUpType() == Defs.PowerUpType.Health)
            {
                
            }else if(powerUp.GetPowerUpType() == Defs.PowerUpType.Shield)
            {
                _shield.EnableShield();
            }
        }
    }
}
