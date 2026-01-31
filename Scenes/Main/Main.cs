using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Main : Control
{
	[Export] private UiButton _quitButton;
	[Export] private UiButton _playButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{        
        GetTree().Paused = false;


		_quitButton.Pressed +=()=>{GetTree().Quit();};
		_playButton.Pressed += ()=>{GameManager.LoadGameScene();};
	}
}
