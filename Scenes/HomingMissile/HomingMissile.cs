using Godot;
using System;

public partial class HomingMissile : HitBox
{
	[Export] private float 	_rotationSpeed = 1.2f;
	[Export] private float 	_speed = 60.0f;
	[Export] private int 	_score = 8;

	private Player _playerRef;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_playerRef = GetTree().GetFirstNodeInGroup(Defs.GROUP_PLAYER) as Player;

		if (!IsInstanceValid(_playerRef))
		{
			GD.Print("HomingMissile queuefree");
			QueueFree();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Turn((float)delta);	
	}

	private void Turn(float delta)
	{
		var dtp = GlobalPosition.DirectionTo(_playerRef.GlobalPosition);
		var atp = Transform.X.AngleTo(dtp);

		var amountWeCanRotate = _rotationSpeed * delta;

		var angleWeWillTurn = Math.Min(Mathf.Abs(atp),amountWeCanRotate);
		Rotate(angleWeWillTurn * Math.Sign(atp));
		Position += Transform.X *_speed * delta;
	}


	protected override void OnAreaEnter(Area2D area)
	{
		GD.Print("==> 1 OnAreaEnter");
		BlowUp();
	}

    private void BlowUp()
    {
		SignalManager.EmitOnCreateExplosion(GlobalPosition,(int)Defs.ExplosionType.Explosion);
		ScoreManager.IncrementScore(_score);
        SetProcess(false);
		QueueFree();
    }
}
