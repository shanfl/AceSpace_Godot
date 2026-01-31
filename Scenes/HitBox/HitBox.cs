using Godot;
using System;

[GlobalClass]
public partial class HitBox : Area2D
{
	[Export] protected int _damage = 10;
	[Export] private CollisionShape2D _collisionShape2D;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AreaEntered += OnAreaEnter;
	}

	public int GetDamage()
	{
		return _damage;
	}

	public void Deactivate()
	{
		_collisionShape2D.SetDeferred(CollisionShape2D.PropertyName.Disabled,true);
	}

	protected virtual void OnAreaEnter(Area2D area)
    {
        GD.Print("==>  2 OnAreaEnter");
		//throw new NotImplementedException();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
