using Godot;

public partial class Sisyphus : Node2D
{
    [Export] private CharacterBody2D _player;
    [Export] private RigidBody2D     _ladder;
    [Export] private PinJoint2D      _joint;
    [Export] private Line2D          _ropeVisual;

    private Node2D _playerShoulder;
    private Node2D _ladderHook;

    public override void _Ready()
    {
        _playerShoulder       = _player.GetNode<Node2D>("ShoulderMarker");
        _ladderHook           = _ladder.GetNode<Node2D>("RopeMarker");
        _joint.NodeA          = _player.GetPath();
        _joint.NodeB          = _ladder.GetPath();
        _joint.GlobalPosition = _playerShoulder.GlobalPosition;
    }

    public override void _Process(double delta)
    {
        if (_playerShoulder != null && _ladderHook != null)
        {
            _ropeVisual.ClearPoints();
            _ropeVisual.AddPoint(_ropeVisual.ToLocal(_playerShoulder.GlobalPosition));
            _ropeVisual.AddPoint(_ropeVisual.ToLocal(_ladderHook.GlobalPosition));
        }
    }
}