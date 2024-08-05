using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AxialCoordinateMovementHelper
{
    // Method to move to the right
    public static Vector3Int MoveDown(Vector3Int cubeCoordinates, int amountToMove)
    {
        return new Vector3Int(cubeCoordinates.x - amountToMove, cubeCoordinates.y, cubeCoordinates.z);
    }

    // Method to move to the upper-right
    public static Vector3Int MoveLowerRight(Vector3Int cubeCoordinates, int amountToMove)
    {
        return new Vector3Int(cubeCoordinates.x, cubeCoordinates.y - amountToMove, cubeCoordinates.z);
    }

    // Method to move to the lower-right
    public static Vector3Int MoveLowerLeft(Vector3Int cubeCoordinates, int amountToMove)
    {
        return new Vector3Int(cubeCoordinates.x - amountToMove, cubeCoordinates.y + amountToMove, cubeCoordinates.z);
    }

    // Method to move to the left
    public static Vector3Int MoveUp(Vector3Int cubeCoordinates, int amountToMove)
    {
        return new Vector3Int(cubeCoordinates.x + amountToMove, cubeCoordinates.y, cubeCoordinates.z);
    }

    // Method to move to the upper-left
    public static Vector3Int MoveUpperRight(Vector3Int cubeCoordinates, int amountToMove)
    {
        return new Vector3Int(cubeCoordinates.x + amountToMove, cubeCoordinates.y - amountToMove, cubeCoordinates.z);
    }

    // Method to move to the lower-left
    public static Vector3Int MoveUpperLeft(Vector3Int cubeCoordinates, int amountToMove)
    {
        return new Vector3Int(cubeCoordinates.x, cubeCoordinates.y + amountToMove, cubeCoordinates.z);
    }
}
