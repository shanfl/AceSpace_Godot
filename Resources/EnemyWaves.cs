using Godot;

[GlobalClass]
public partial class EnemyWaves : Resource
{
    [Export] private Godot.Collections.Array<EnemyWave> _waves = [];

    public EnemyWave GetWaveFromWaveCount(int c)
    {
        return _waves[c%_waves.Count];
    }

    public bool WaveIsStart(int wc)
    {
        return wc%_waves.Count == 0;
    }
}