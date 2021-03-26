using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPosition : MonoBehaviour
{
    [SerializeField] public GridPosition left = null;
    [SerializeField] public GridPosition right = null;
    [SerializeField] public GridPosition up = null;
    [SerializeField] public GridPosition down = null;

    public GridPosition GetNext(SlideController.Direction dir)
    {
        switch (dir)
        {
            case SlideController.Direction.Up:
                return up;
            case SlideController.Direction.Right:
                return right;
            case SlideController.Direction.Left:
                return left;
            case SlideController.Direction.Down:
                return down;
        }
        return null; // should not occur
    }

    public GridPosition GetOpposite(SlideController.Direction dir)
    {
        switch (dir)
        {
            case SlideController.Direction.Up:
                return down;
            case SlideController.Direction.Right:
                return left;
            case SlideController.Direction.Left:
                return right;
            case SlideController.Direction.Down:
                return up;
        }
        return null; // should not occur
    }
}
