using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPosition : MonoBehaviour
{
    [SerializeField] public GridPosition left = null;
    [SerializeField] public GridPosition right = null;
    [SerializeField] public GridPosition up = null;
    [SerializeField] public GridPosition down = null;

    public GridPosition GetNext(DrugTile.Direction dir)
    {
        switch (dir)
        {
            case DrugTile.Direction.Up:
                return up;
            case DrugTile.Direction.Right:
                return right;
            case DrugTile.Direction.Left:
                return left;
            case DrugTile.Direction.Down:
                return down;
        }
        return null; // should not occur
    }

    public GridPosition GetOpposite(DrugTile.Direction dir)
    {
        switch (dir)
        {
            case DrugTile.Direction.Up:
                return down;
            case DrugTile.Direction.Right:
                return left;
            case DrugTile.Direction.Left:
                return right;
            case DrugTile.Direction.Down:
                return up;
        }
        return null; // should not occur
    }
}
