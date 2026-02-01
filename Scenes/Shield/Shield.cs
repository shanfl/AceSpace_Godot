using Godot;
using System;

public partial class Shield : Area2D
{
	[Export] Timer _timer;
	[Export] private float _startHealth = 5;
	[Export] private AudioStreamPlayer2D _sound;
	[Export] private AnimationPlayer _aniPlayer;
	[Export] CollisionShape2D _colliShape;

	private float _health;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_timer.Timeout += OnTimeOut;
		AreaEntered += OnAreaEntered;
		DisableShield();
	}

	public void EnableShield()
	{
		_aniPlayer.Play("RESET");
		Show();
		_health = _startHealth;
		_colliShape.CallDeferred(CollisionShape2D.MethodName.SetDisabled,false);
		_timer.Start();
		SoundManager.PlayPowerUpSound(_sound,Defs.PowerUpType.Shield);
	}

    private void DisableShield()
    {
		Hide();
        //_colliShape.Disabled = true;
		_colliShape.CallDeferred(CollisionShape2D.MethodName.SetDisabled,true);
		_timer.Stop();
    }

	public void Hit()
	{
		_aniPlayer.Play("hit");
		_health--;
		if(_health <= 0)
		{
			DisableShield();
		}
	}


    private void OnAreaEntered(Area2D area)
    {
        Hit();
    }


    private void OnTimeOut()
    {
        DisableShield();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
