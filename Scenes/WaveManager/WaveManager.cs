using Godot;
using System;
using System.IO;

public partial class WaveManager : Node2D
{

	[Export] private EnemyWaves _enemyWavesResource;
	[Export] private Node2D _pathsContainer;
	[Export] Timer _spawnTimer;

	private int _waveCount = 0;
	private Godot.Collections.Array<Path2D> _path2Ds = [];
	private float _waveGap = 4.0f;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_spawnTimer.Timeout += OnSpawnTimeOut;
		SetupPaths();

		//SpawnWave();
		CallDeferred(MethodName.SpawnWave);
	}

    private void SetupPaths()
    {
        foreach(var node in _pathsContainer.GetChildren())
		{
			if(node is Path2D)
			{
				_path2Ds.Add((Path2D)node);
			}
		}
    }

	private void StartSpawnTimer()
	{
		_spawnTimer.WaitTime = _waveGap;
		_spawnTimer.Start();
	}

	// --------------------------------------------------------
	private EnemyBase CreateEnemy(EnemyWave wave)
	{
		var newEnemy = (EnemyBase) wave.EnemyScen.Instantiate();
		newEnemy.Setup(wave.Speed);
		GD.Print($"ememy create speed {wave.Speed}");
		return newEnemy;
	}
	// --------------------------------------------------------

	private async void SpawnWave()
	{
		GD.Print($"spawn_wave() _waveCount {_waveCount}");

		var path = _path2Ds.PickRandom();
		var wave = _enemyWavesResource.GetWaveFromWaveCount(_waveCount);

		GD.Print($"wave() {_waveCount} spawing {wave.Number} enemies on path {path.Name}");

		for(int i = 0;i < wave.Number; i++)
		{
			path.AddChild(CreateEnemy(wave));
			await ToSignal(GetTree().CreateTimer(wave.Gap),"timeout");

			GD.Print($"wave() index:{i}  spawnd");
		}

		GD.Print($"wave() {_waveCount} spawnd");
		_waveCount ++;

		StartSpawnTimer();
	}

    private void OnSpawnTimeOut()
    {
        SpawnWave();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
