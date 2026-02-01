using Godot;
using System;

public partial class ObjectMaker : Node2D
{
    private PackedScene _playerBulletScene = GD.Load<PackedScene>("res://Scenes/PlayerBullet/PlayerBullet.tscn");
    private PackedScene _enmeyBulletScene = GD.Load<PackedScene>("res://Scenes/BaseBullet/EnemyBullet.tscn");
    private PackedScene _enemyBoombScene = GD.Load<PackedScene>("res://Scenes/BaseBullet/EnemyBomb.tscn");
    private PackedScene _powerUpScene = GD.Load<PackedScene>("res://Scenes/PowerUp/PowerUp.tscn");
    private PackedScene _explosionScene = GD.Load<PackedScene>("res://Scenes/explosion/Explosion.tscn");
    private PackedScene _homingmissileScene = GD.Load<PackedScene>("res://Scenes/HomingMissile/HomingMissile.tscn");    

    public override void _Ready() 
    { 
        SignalManager.Instance.OnCreateExplosion += OnCreateExplosion;
        SignalManager.Instance.OnCreateHomingMissile += OnCreateHomingMissile;
        SignalManager.Instance.OnCreatePowerUp += OnCreatePowerUp;
        SignalManager.Instance.OnCreateRandomPowerUp += OnCreateRandomPowerUp;
        SignalManager.Instance.OnCreateBullet += OnCreateBullet;
        //SignalManager.Instance.OnCreateHomingMissile += OnCreateHomingMissile;
    }


    private PackedScene GetBulletScene(int type)
    {
        Defs.BulletType bulletType = (Defs.BulletType)type;
        switch (bulletType)
        {
            case Defs.BulletType.Player:
                return _playerBulletScene;
            case Defs.BulletType.Enemy:
                return _enmeyBulletScene;
            case Defs.BulletType.EnemyBomb:
                return _enemyBoombScene;
            default:
                return _playerBulletScene;
        }
    }

    private void OnCreateBullet(Vector2 startPos, Vector2 direction, float speed, int type)
    {
        // throw new NotImplementedException();
        var newScene = GetBulletScene(type).Instantiate<BaseBullet>();
        newScene.Setup(direction,speed);
        CallDeferred(MethodName.AddObject,newScene,startPos);
    }



    private void OnCreateRandomPowerUp(Vector2 startPos)
    {
        Defs.PowerUpType puType = SpaceUtils.GetRandomEnumValue<Defs.PowerUpType>();
        OnCreatePowerUp(startPos,(int)puType);
    }


    private void OnCreatePowerUp(Vector2 startPos, int puType)
    {
        var newScene = _powerUpScene.Instantiate<PowerUp>();
        newScene.SetupPowerUpType((Defs.PowerUpType)puType);

        CallDeferred(MethodName.AddObject,newScene,startPos);
    }


    private void OnCreateHomingMissile(Vector2 startPos)
    {
        GD.Print("------------------>objectmaker OnCreateHomingMissile");
        var newScene = _homingmissileScene.Instantiate<HomingMissile>();
        //newScene.SetupPowerUpType((Defs.PowerUpType)puType);

        CallDeferred(MethodName.AddObject,newScene,startPos);
    }


    private void OnCreateExplosion(Vector2 startPos, int explosionType)
    {
        var scene = _explosionScene.Instantiate<Explosion>();
        scene.Setup((Defs.ExplosionType)explosionType);
        CallDeferred(MethodName.AddObject,scene,startPos);
    }


    public override void _ExitTree()
    {
        SignalManager.Instance.OnCreateExplosion -= OnCreateExplosion;
        SignalManager.Instance.OnCreateHomingMissile -= OnCreateHomingMissile;
        SignalManager.Instance.OnCreatePowerUp -= OnCreatePowerUp;
        SignalManager.Instance.OnCreateBullet -= OnCreateBullet;
        SignalManager.Instance.OnCreateRandomPowerUp -= OnCreateRandomPowerUp;
    }

    private void AddObject(Node2D node, Vector2 globalPosition)
    {
        node.GlobalPosition = globalPosition;
        AddChild(node);
    }

    
}
