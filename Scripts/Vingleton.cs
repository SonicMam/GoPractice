using Godot;

//Checks all 4 corners on if it's on a wall
public class Vingleton : Sprite
{
    TileMap walls;
    Vector2[] checkPositions = { new Vector2(-16, -16), new Vector2(16, -16), new Vector2(16, 16), new Vector2(-16, 16) };
    public override void _Ready() => walls = GetNode<TileMap>("../../../BG/Walls/WallSprites");
    public int[] GetTileIDs(Vector2 position)
    {
        int[] tileIds = new int[checkPositions.Length];
        for (int i = 0; i < checkPositions.Length; i++)
        {
            Vector2 checkPos = position + checkPositions[i];
            tileIds[i] = walls.GetCellv(checkPos / walls.CellSize);
        }
        return tileIds;
    }
}
