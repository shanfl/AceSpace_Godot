using Godot;
using System;


public partial class EnemyBase : PathFollow2D
{
	[ExportGroup("EXPORTS")]
	[Export] private float _speed = 50.0f;
	[Export] private Timer _laserTimer;
	[Export] private AudioStreamPlayer2D _sound;
	[Export] private AnimatedSprite2D _animatedSprite2D;

	/// <summary>
    /// 是否可以进行设计(有的敌人纯移动)
    /// </summary>
	[Export] private bool _shoots {get;set;} = false;
	[Export] private bool _aimsAtPlayer {get;set;} = false;
	[Export] private Defs.BulletType _bulletType {get;set;} = Defs.BulletType.Enemy;
	[Export] private float _bulletSpeed {get;set;} = 120.0f;
	[Export] private Vector2 _bulletDirection {get;set;} = Vector2.Down;
	[Export] private float _bulletWaitTime {get;set;} = 2.0f;
	[Export] private float _bulletWaitTimeVar{get;set;} = 0.05f;  // 防止敌人同时shoot,过于一致
	//private float _speed = 50.0f;
	private Player _playerRef;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_playerRef = GetTree().GetFirstNodeInGroup(Defs.GROUP_PLAYER) as Player;
		if(_playerRef == null)
		{
			QueueFree();
			return;
		}

		_laserTimer.Timeout += OnLaserTimeOut;
		SpaceUtils.PlayRandomAnimation(_animatedSprite2D);
		StartShootTimer();
	}

    private void  StartShootTimer()
    {
        SpaceUtils.SetAndStartTimer(_laserTimer,_bulletWaitTime,_bulletWaitTimeVar);
    }


    private void OnLaserTimeOut()
    {
        //SignalManager.emi
		Shoot();
    }


	private void UpdateBulletDirection()
	{
		if(!_aimsAtPlayer || !IsInstanceValid(_playerRef))
		{
			return;
		}

		_bulletDirection = GlobalPosition.DirectionTo(_playerRef.GlobalPosition);

	}

    private void Shoot()
    {
        if(!_shoots)  return;
		// update bullet direction
		UpdateBulletDirection();
		// signal to create a bullet
		SignalManager.EmitOnCreateBullet(GlobalPosition,_bulletDirection,_bulletSpeed,(int)_bulletType);
		
		// play a random laser sound
		SoundManager.PlayLaserRandom(_sound);
		// start shoot timer
		StartShootTimer();
		GD.Print("enemy shoot");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.

    public override void _Process(double delta)
	{
		Progress += _speed * (float)delta;

		if(ProgressRatio > 0.99f)
		{
			QueueFree();
		}
	}
}
