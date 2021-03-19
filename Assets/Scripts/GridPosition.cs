using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPosition : MonoBehaviour
{
    [SerializeField] public GridPosition left = null;
    [SerializeField] public GridPosition right = null;
    [SerializeField] public GridPosition up = null;
    [SerializeField] public GridPosition down = null;
}
