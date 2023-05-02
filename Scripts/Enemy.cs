using Godot;

public class Enemy : KinematicBody2D
{
    const int moveDistance = 128;
    KinematicBody2D player;
    Vector2 moveDirection;
    float moveTimer;
    TileMap BG;
    TileMap Walls;

    public override void _Ready()
    {
        player = GetNode<KinematicBody2D>("../Player");
        BG = GetNode<TileMap>("../BG");
        Walls = GetNode<TileMap>("../BG/Walls/WallSprites");
    }

    public override void _Process(float delta)
    {
        //Tracking and moving to the player every 3 seconds
        moveDirection = (player.Position - Position).Normalized();
        moveDirection = Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y) ? new Vector2(Mathf.Sign(moveDirection.x), 0) : new Vector2(0, Mathf.Sign(moveDirection.y));
        moveTimer += delta;
        if (moveTimer >= 3f)
        {
            Vector2 targetPos = Position + moveDirection * moveDistance;
            bool canMove = true;
            Vector2[] corners = new Vector2[]
            {
            new Vector2(targetPos.x - 60, targetPos.y - 60),
            new Vector2(targetPos.x + 60, targetPos.y - 60),
            new Vector2(targetPos.x - 60, targetPos.y + 60),
            new Vector2(targetPos.x + 60, targetPos.y + 60)
            };
            foreach (Vector2 corner in corners)
            {
                int targetTileId = BG.GetCellv(corner / BG.CellSize);
                int targetWallId = Walls.GetCellv(corner / Walls.CellSize);
                if (targetTileId == 5 || targetWallId != -1)
                {
                    canMove = false;
                }
            }
            if (canMove)
            {
                Position = targetPos;
                moveTimer = 0f;
            }
        }
    }
}