using Godot;
using System;

public class StageFailed : Control
{
    private Player _player;
    
    public override void _Ready()
    {
        _player = GetParent().GetParent().GetNode<Player>("Player");
        _player.Connect("CollidedWithSun", this, "PopUpWindow");
    }

    private void PopUpWindow()
    {
        Visible = true;
        GetTree().Paused = true;
    }

    private void RepeatStage()
    {
        GetTree().ReloadCurrentScene();
        GetTree().Paused = false;
    }

    private void QuitGame()
    {
        GD.Print("Quit Game");
    }
}
