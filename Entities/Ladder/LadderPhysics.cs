using Godot;
using System;

public partial class LadderPhysics : RigidBody2D
{
    public override void _Ready()
    {
        Sleeping = false;
        CanSleep = false;
    }
}
