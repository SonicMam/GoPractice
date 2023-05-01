using Godot;
using System;
using System.Windows;

public class Player : KinematicBody2D
{
    private const int acceleration = 800; // Speeding up
    private const int maxSpeed = 800;
    private const int friction = 3000; // Slowing down
    private Vector2 velocity;
    private int screenWidth;
    private int screenHeight;
    private float padding = 1f; // Adjust as needed
    private string facing = "downR";

    public override void _Ready() // Done at start
    {
        Rect2 visibleRect = GetViewport().GetVisibleRect();
        screenWidth = (int)visibleRect.Size.x;
        screenHeight = (int)visibleRect.Size.y;
    }
    private void Death() // What happens when you die because you were killed
    {
        SceneTree sceneTree = GetTree();
        sceneTree.ReloadCurrentScene();
        GD.Print("RIP Dingleton");
    }

    //Collision detection for bad things that kill you
    public void _on_Enemy_body_entered(Node body)
    {
        if (body is Player)
        {
            Player player = (Player)body;
            player.Death();
        }
    }
    public void _on_Spikes_body_entered(Node body)
    {
        if (body is Player)
        {
            Player player = (Player)body;
            player.Death();
        }
    }

    [Obsolete]
    public override void _PhysicsProcess(float delta)  // Logic every frame
    {
        // Movement
        var input_vector = new Vector2(
            Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left"),
            Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up")
        ).Normalized();

        velocity = input_vector == Vector2.Zero
            ? velocity.MoveToward(Vector2.Zero, friction * delta)
            : velocity.MoveToward(input_vector * maxSpeed, acceleration * delta);

        velocity = MoveAndSlide(velocity);

        // Teleport to other side if player hits screen edge
        Position = new Vector2(
            Position.x < -padding ? screenWidth + padding :
            Position.x > screenWidth + padding ? -padding :
            Position.x,
            Position.y < -padding ? screenHeight + padding :
            Position.y > screenHeight + padding ? -padding :
            Position.y
        );

        // "Animation" - (Alex writes  'Best code ever'. Asked to commit Third Impact.)
        Godot.Sprite dingleton = this.GetNode<Godot.Sprite>("Dingleton");
        int frame = facing switch
        {
            "upR" => Input.IsActionPressed("ui_down") ? 0 :
                     Input.IsActionPressed("ui_left") ? 3 : -1,
            "upL" => Input.IsActionPressed("ui_down") ? 1 :
                     Input.IsActionPressed("ui_right") ? 2 : -1,
            "downR" => Input.IsActionPressed("ui_up") ? 2 :
                       Input.IsActionPressed("ui_left") ? 1 : -1,
            "downL" => Input.IsActionPressed("ui_up") ? 3 :
                       Input.IsActionPressed("ui_right") ? 0 : -1,
            _ => 0
        };
        if (frame >= 0)
        {
            dingleton.SetFrame(frame);
            facing = frame switch
            {
                0 => "downR",
                1 => "downL",
                2 => "upR",
                3 => "upL",
                _ => facing
            };
        }
    }
}