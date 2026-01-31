using Godot;
using System;

public partial class EnemyBase : PathFollow2D
{
	[Export] private float _speed = 90.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
