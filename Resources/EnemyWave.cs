using Godot;

[GlobalClass]
public partial class EnemyWave : Resource
{
    [Export] public PackedScene EnemyScen {get;private set;}
    [Export] public float Speed {get;private set;}
    [Export] public float Gap {get;private set;}
    [Export] public int Number{get;private set;}


}