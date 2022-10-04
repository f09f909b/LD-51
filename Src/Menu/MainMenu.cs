using Godot;
using System;

public class MainMenu : Control
{
    private void StartGame()
    {
        GetTree().ChangeScene("res://Src/Stages/Stage1.tscn");
    }
}
