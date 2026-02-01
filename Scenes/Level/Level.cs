using Godot;
using System;

public partial class Level : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GetTree().Paused = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if(Input.IsActionJustPressed("test"))
        {
           SignalManager.EmitOnCreatePowerUp(new Vector2(100,100),(int)Defs.PowerUpType.Shield);
           SignalManager.EmitOnCreateExplosion(new Vector2(100,200),(int)Defs.ExplosionType.Explosion);
        }
        if(Input.IsActionJustPressed("quit"))
        {
            GameManager.LoadMainScene();
        }
	}
}
