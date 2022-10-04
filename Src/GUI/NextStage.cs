using Godot;
using System.Text.RegularExpressions;

public class NextStage : Control
{
    private Player _player;

    public override void _Ready()
    {
        _player = GetParent().GetParent().GetNode<Player>("Player");
        _player.Connect("LandedOnExit", this, "PopUpWindow");
    }

    private void PopUpWindow()
    {
        Visible = true;
        GetTree().Paused = true;
    }

    private void PlayNextStage()
    {
        var currentScene = GetTree().CurrentScene.Name;
        var matches = Regex.Matches(currentScene, @"\d+");
        var levelNumber = int.Parse(matches[0].Value) + 1;
        GetTree().ChangeScene("res://Src/Stages/Stage" + levelNumber.ToString() + ".tscn");
        GetTree().Paused = false;
    }

    private void RepeatStage()
    {
        GetTree().ReloadCurrentScene();
        GetTree().Paused = false;
    }
}
