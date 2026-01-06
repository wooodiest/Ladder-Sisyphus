using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
    [ExportCategory("Movement Stats")]
    [Export] public float Speed = 200.0f;
    [Export] public float Acceleration = 1200.0f;
    [Export] public float Friction = 1200.0f;
    [Export] public float JumpVelocity = -450.0f;
    [Export] public float Gravity = 1200.0f;

    [ExportCategory("Ladder Control")]
    [Export] public RigidBody2D LadderRef;
    [Export] public float LadderTorque = 7000.0f;
    [Export] public float AngularDampBase = 5.0f;

    public override void _PhysicsProcess(double delta)
    {
        HandleMovement((float)delta);
        HandleLadderPhysics((float)delta);
    }

    private void HandleMovement(float dt)
    {
        Vector2 velocity = Velocity;

        if (!IsOnFloor())
            velocity.Y += Gravity * dt;

        if (Input.IsActionJustPressed("jump") && IsOnFloor())
            velocity.Y = JumpVelocity;

        float direction = Input.GetAxis("move_left", "move_right");
        if (direction != 0)
            velocity.X = Mathf.MoveToward(velocity.X, direction * Speed, Acceleration * dt);
        else
            velocity.X = Mathf.MoveToward(velocity.X, 0, Friction * dt);

        Velocity = velocity;
        MoveAndSlide();
    }

    private void HandleLadderPhysics(float dt)
    {
        if (LadderRef == null) return;

        Vector2 mousePos = GetGlobalMousePosition();
        Vector2 pivotPos = GlobalPosition;

        Vector2 directionToMouse = mousePos - pivotPos;
        float targetAngle = directionToMouse.Angle();

        float currentAngle = LadderRef.Rotation;
        float angleDifference = Mathf.AngleDifference(currentAngle, targetAngle);

        float torqueToApply = angleDifference * LadderTorque;

        torqueToApply -= LadderRef.AngularVelocity * 50.0f;

        LadderRef.ApplyTorqueImpulse(torqueToApply);
    }
}