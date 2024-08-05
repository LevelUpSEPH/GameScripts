using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHexXZ<TGridObject>
{
    private const float HEX_VERTICAL_OFFSET_MULTIPLIER = .85576923076f;

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int z;
    }

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private List<TGridObject>[,] gridArray;

    public GridHexXZ(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new List<TGridObject>[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridArray[x, y] = new List<TGridObject>();
            }
        }

        GridObject.GridChanged += OnGridChanged;
        GridObject.GridInitialized += OnGridInitialized;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        int oddRow;
        if(height % 2 == 0)
        {
            oddRow = 1;
        }
        else
        {
            oddRow = -1;
        }

        return
            new Vector3(x, 0, 0) * cellSize +
            new Vector3(0, 0, z) * cellSize * HEX_VERTICAL_OFFSET_MULTIPLIER + 
            ((Mathf.Abs(z) % 2) == 1 ? new Vector3(oddRow, 0, 0) * cellSize * .5f : Vector3.zero) +
            originPosition;
    }

    public void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        int roughX = Mathf.RoundToInt((worldPosition - originPosition).x / cellSize);
        int roughZ = Mathf.RoundToInt((worldPosition - originPosition).z / cellSize / HEX_VERTICAL_OFFSET_MULTIPLIER);

        Vector3Int roughXZ = new Vector3Int(roughX, 0, roughZ);

        bool oddRow = roughZ % 2 == 1;

        List<Vector3Int> neighbourXZList = new List<Vector3Int>
        {
             roughXZ + new Vector3Int(-1, 0, 0),
             roughXZ + new Vector3Int(+1, 0, 0),

             roughXZ + new Vector3Int(oddRow ? +1 : -1, 0, +1),
             roughXZ + new Vector3Int(+0, 0, +1),

             roughXZ + new Vector3Int(oddRow ? +1 : -1, 0, -1),
             roughXZ + new Vector3Int(+0, 0, -1),
        };

        Vector3Int closestXZ = roughXZ;

        foreach (Vector3Int neighbourXZ in neighbourXZList)
        {
            if (Vector3.Distance(worldPosition, GetWorldPosition(neighbourXZ.x, neighbourXZ.z)) <
                Vector3.Distance(worldPosition, GetWorldPosition(closestXZ.x, closestXZ.z)))
            {
                // Closer than closest
                closestXZ = neighbourXZ;
            }
        }

        x = closestXZ.x;
        z = closestXZ.z;
    }

    public void SetGridObject(int x, int z, TGridObject value)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            if(gridArray[x, z].Count > 0)
            {
                TGridObject genericGridObject = gridArray[x, z][gridArray[x, z].Count - 1];
                GridObject gridObject = (genericGridObject as GridObject);
                gridObject.HideContent(true);
            }
            
            gridArray[x, z].Add(value);
            (value as GridObject).ShowAndHandleContentInitialState();

            TriggerGridObjectChanged(x, z);
        }
    }

    public void TriggerGridObjectChanged(int x, int z)
    {
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, z = z });
    }

    public bool TryGetGridObject(int x, int z, out TGridObject gridObject)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            if(gridArray[x, z].Count > 0)
            {
                gridObject = gridArray[x, z][gridArray[x, z].Count - 1];
                return true;
            }
            else
            {
                gridObject = default(TGridObject);
                return false;
            }
        }
        else
        {
            gridObject =  default(TGridObject);
            return false;
        }
    }

    public bool TryGetAllGridObjects(int x, int z, out List<TGridObject> gridObjects)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            if (gridArray[x, z].Count > 0)
            {
                gridObjects = gridArray[x, z];
                return true;
            }
            else
            {
                gridObjects = default(List<TGridObject>);
                return false;
            }
        }
        else
        {
            gridObjects = default(List<TGridObject>);
            return false;
        }
    }

    public void RemoveGridObject(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            if(gridArray[x, z].Count > 0)
            {
                gridArray[x, z].RemoveAt(gridArray[x, z].Count - 1);
            }
        }
    }

    public void RemoveGridObjectFromPosition(int x, int z, TGridObject gridObject)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            if(gridArray[x, z].Contains(gridObject) && gridObject != null)
            {
                gridArray[x, z].Remove(gridObject);
            }
            else
            {
                Debug.LogWarning("Grid object not found in position: " + x + ", " + z);
                gridArray[x, z].RemoveAt(gridArray[x, z].Count - 1);
            }
        }
    }

    public bool TryGetGridObject(Vector3 worldPosition, out TGridObject gridObject)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);

        return TryGetGridObject(x, z, out gridObject);
    }

    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
    {
        return new Vector2Int
        (
            Mathf.Clamp(gridPosition.x, 0, width - 1),
            Mathf.Clamp(gridPosition.y, 0, height - 1)
        );
    }

    public bool IsValidGridPosition(Vector2Int gridPosition)
    {
        int x = gridPosition.x;
        int z = gridPosition.y;

        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsValidGridPositionWithPadding(Vector2Int gridPosition)
    {
        Vector2Int padding = new Vector2Int(2, 2);
        int x = gridPosition.x;
        int z = gridPosition.y;

        if (x >= padding.x && z >= padding.y && x < width - padding.x && z < height - padding.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnGridInitialized(Vector3 gridWorldPosition, GridObject gridObject)
    {
        int x;
        int z;
        GetXZ(gridWorldPosition, out x, out z);

        if(gridObject != null)
        {
            if(gridObject.GetIsEmpty())
            {
                gridObject.HideWithoutTween();
                RemoveGridObject(x, z);
                string randomKey = "InitialRoutine #" + UnityEngine.Random.Range(0, 100000);
                RocketCoroutine.CoroutineController.StartCoroutine(WaitAndShowNextGrid(x, z), randomKey);
            }
        }
    }

    private void OnGridChanged(Vector3 gridWorldPosition)
    {
        int x;
        int z;
        GetXZ(gridWorldPosition, out x, out z);

        TGridObject changedGridObject;

        if(TryGetGridObject(x, z, out changedGridObject))
        {
            if((changedGridObject as GridObject).GetIsEmpty())
            {
                (changedGridObject as GridObject).Hide();
                RemoveGridObjectFromPosition(x, z, changedGridObject);
                string randomKey = "Routine #" + UnityEngine.Random.Range(0, 100000);
                RocketCoroutine.CoroutineController.StartCoroutine(WaitAndShowNextGrid(x, z), randomKey);
            }
        }
    }

    private IEnumerator WaitAndShowNextGrid(int x, int z)
    {
        yield return new WaitForSeconds(.2f);
        TGridObject upcomingGridObject;
        if(TryGetGridObject(x, z, out upcomingGridObject))
        {
            (upcomingGridObject as GridObject).Show();
        }
    }

    public bool GetIsEmpty()
    {
        foreach(List<TGridObject> gridObjectStack in gridArray)
        {
            if(gridObjectStack.Count > 0)
                return false;
        }
        return true;
    }
}