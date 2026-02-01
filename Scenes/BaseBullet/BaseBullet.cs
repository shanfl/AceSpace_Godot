using Godot;
using System;

public partial class BaseBullet : HitBox
{
	[Export] private VisibleOnScreenNotifier2D _visiableOnScreenNotify;

	private Vector2 _direction = Vector2.Up;
	private float _speed = 50.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_visiableOnScreenNotify.ScreenExited += OnScreenExited;
	}

	public void Setup(Vector2 dir,float sp)
	{
		_direction = dir;
		_speed = sp;
	}

    private void OnScreenExited()
    {
        //throw new NotImplementedException();
		GD.Print("===> OnScreenExited");
		QueueFree();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		Position += _direction * (float) delta * _speed;
	}

	protected override void OnAreaEnter(Area2D area)
	{
		GD.Print("==> 1 OnAreaEnter");
		BlowUp();
	}

    private void BlowUp()
    {
		SignalManager.EmitOnCreateExplosion(GlobalPosition,(int)Defs.ExplosionType.Explosion);
        SetProcess(false);
		QueueFree();
    }

}
