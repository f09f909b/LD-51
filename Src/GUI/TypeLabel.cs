using Godot;
using System;

public class TypeLabel : Control
{
    private string _labelText;
    private Label _typeLabel;
    private Player _player;

    public override void _Ready()
    {
        _player = GetParent().GetParent().GetParent().GetNode<Player>("Player");
        _typeLabel = GetNode<Label>("Type");
        _player.Connect("LandedOnRegularPlanet", this, "ApplyRegularPlanetTime");
        _player.Connect("LandedOnHeavyPlanet", this, "ApplyHeavyPlanetTime");
        _player.Connect("LandedOnLightPlanet", this, "ApplyLightPlanetTime");
    }

    private void ApplyRegularPlanetTime()
    {
        _typeLabel.Text = "Standard";
    }

    private void ApplyHeavyPlanetTime()
    {
        _typeLabel.Text = "High Mass";
    }
    
    private void ApplyLightPlanetTime()
    {
        _typeLabel.Text = "Low Mass";
    }
}
