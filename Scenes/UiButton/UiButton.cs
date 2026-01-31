using Godot;
using System;
using System.ComponentModel;

public partial class UiButton : TextureButton
{

	[Export] private string _labelText;
	[Export] private Label _label;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_label.Text = _labelText;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
