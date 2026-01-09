using Godot;

public partial class Ladder : RigidBody2D
{
    [ExportCategory("Physics settings")]
    [Export] public Vector2 LadderSize  = new Vector2(12, 120);
    [Export] public float   RopeOffsetY = -20f;

    [ExportCategory("Utils")]
    [Export] public Color DebugColor = new Color(0.55f, 0.27f, 0.07f);

    private Marker2D         _ropeMarker;
    private Marker2D         _topPoint;
    private Marker2D         _bottomPoint;
    private CollisionShape2D _collisionShape;
    private Sprite2D         _sprite;

    public override void _Ready()
    {
        _ropeMarker     = GetNode<Marker2D>("RopeMarker");
        _topPoint       = GetNode<Marker2D>("TopPoint");
        _bottomPoint    = GetNode<Marker2D>("BottomPoint");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        _sprite         = GetNode<Sprite2D>("Sprite2D");

        SetupLadderDimensions();
    }

    private void SetupLadderDimensions()
    {
        var shape = new RectangleShape2D() 
        { 
            Size = LadderSize
        };

        _collisionShape.Shape = shape;
        _ropeMarker .Position = new Vector2(0, RopeOffsetY);
        _topPoint   .Position = new Vector2(0, -LadderSize.Y / 2);
        _bottomPoint.Position = new Vector2(0,  LadderSize.Y / 2);

        if (_sprite.Texture is PlaceholderTexture2D placeholder)
        {
            placeholder.Size = LadderSize;
        }
        else
        {
            _sprite.Scale = Vector2.One;
        }

        QueueRedraw();
    }

    public override void _Draw()
    {
        var rect = new Rect2(-LadderSize / 2, LadderSize);
        DrawRect(rect, DebugColor, false, 1.0f);
    }

    public Vector2 GetRopeAttachmentPosition()
    {
        return _ropeMarker.GlobalPosition;
    }

    public Vector2 GetTopPosition()
    {
        return _topPoint.GlobalPosition;
    }

    public Vector2 GetBottomPosition()
    {
        return _bottomPoint.GlobalPosition;
    }
}