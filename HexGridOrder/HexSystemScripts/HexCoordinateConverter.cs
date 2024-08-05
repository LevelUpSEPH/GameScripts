using UnityEngine;

public static class HexCoordinateConverter
{

   public static Vector3Int ConvertToAxialCoordinates(Vector2Int hexCoordinates, int height) // -1 if odd count per grid, 1 if even count per grid
    {
        int offset = 0;
        if(height % 2 == 0)
            offset = 1;
        else
            offset = -1;
        
        int q = hexCoordinates.x - (int)((hexCoordinates.y - (offset * (hexCoordinates.y & 1))) / 2);
        int r = hexCoordinates.y;
        int s = -q - r;
        return new Vector3Int(q, r, s);
    }

    public static Vector2Int ConvertToHexagonalCoordinates(Vector3Int cubeCoordinates, int height)
    {
        int offset = 0;
        if(height % 2 == 0)
            offset = 1;
        else
            offset = -1;

        int col = cubeCoordinates.x + (int)((cubeCoordinates.y - (offset * (cubeCoordinates.y & 1))) / 2);
        int row = cubeCoordinates.y;
        return new Vector2Int(col, row);
    }
}
