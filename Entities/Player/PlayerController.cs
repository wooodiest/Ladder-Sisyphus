using Godot;

public partial class PlayerController : CharacterBody2D
{
    [ExportCategory("Physics settings")]
    [Export] public float Speed        = 200.0f;
    [Export] public float JumpVelocity = -400.0f;

    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    private Node2D   _visuals;
    private Marker2D _shoulderMarker;

    public override void _Ready()
    {
        _visuals        = GetNode<Node2D>("Visuals");
        _shoulderMarker = GetNode<Marker2D>("ShoulderMarker");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        if (!IsOnFloor())
        {
            velocity.Y += gravity * (float)delta;
        }

        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        float direction = Input.GetAxis("move_left", "move_right");

        if (direction != 0)
        {
            velocity.X = direction * Speed;
            _visuals.Scale = new Vector2(Mathf.Sign(direction), 1);
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }
    public Vector2 GetShoulderPosition()
    {
        return _shoulderMarker.GlobalPosition;
    }
}