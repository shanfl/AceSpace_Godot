using Godot;
using System;

public partial class Saucer : PathFollow2D
{
	const string PLAYBACK_PARAM = "parameters/playback";

	[Export] private AnimationTree _animationTree;
    [Export] private HealthBar _healthBar;
    [Export] private Node2D _booms;
    [Export] private HitBox _hitBox;
    [Export] private Timer _shootTimer;
    [Export] private AudioStreamPlayer2D _sound;

    [Export] private float _speed = 20.0f;
    [Export] private float _boomDelay = 0.1f;
    [Export] private int _score = 150;
    [Export] private float _waitTime = 16.0f;
    [Export] private float _waitTimeVar = 4.0f;


	private bool _shooting = false;

	/// <summary>
	/// PLAYBACK_PARAM 通过AnimationNodeStateMachinePlayback来变换状态
	/// </summary>
	[Export] private AnimationNodeStateMachinePlayback _stateMachinePlayBack;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_stateMachinePlayBack 	= (AnimationNodeStateMachinePlayback)_animationTree.Get(PLAYBACK_PARAM);
		_healthBar.OnDied 		+= OnHealthBarDied;
		_hitBox.AreaEntered 	+= OnHitBoxAreaEnter;
		_shootTimer.Timeout 	+= Shoot;
		ResetTimer();
	}

    private void ResetTimer()
    {
		GD.Print($"---- resettimer  waittime:{_waitTime}");
        SpaceUtils.SetAndStartTimer(_shootTimer,_waitTime,_waitTimeVar);
    }

    private void Shoot()
    {
		GD.Print("shoot---------------> timerout");
        _shooting = true;
		_stateMachinePlayBack.Travel("shoot");
		_sound.Play();
    }

	private void StopShooting()
	{
		_shooting = false;
		ResetTimer();
	}

	private void FireMissile()
	{
		GD.Print("fire missile");
		SignalManager.EmitOnCreateHomingMissile(GlobalPosition);
	}

    private void OnHitBoxAreaEnter(Area2D area)
    {
        _healthBar.TakeDamage((area as HitBox).GetDamage());
    }

    private void OnHealthBarDied()
    {
        _hitBox.Deactivate();
		_healthBar.Hide();
		_healthBar.OnDied -= OnHealthBarDied;
		_shootTimer.Stop();
		SetProcess(false);
		ScoreManager.IncrementScore(_score);
		_stateMachinePlayBack.Travel("die");
		MakeBooms();
    }
 
    private async void MakeBooms()
    {
        foreach (Node2D b in _booms.GetChildren())
        {
            SignalManager.EmitOnCreateExplosion(b.GlobalPosition, (int)Defs.ExplosionType.Boom);
            await ToSignal(GetTree().CreateTimer(_boomDelay), "timeout");
        }
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if (!_shooting)
		{
			Progress += _speed * (float)delta;
		}
	}
}
