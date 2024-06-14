using System;
using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "CellSO", menuName = "Cells/CellSO")]
public class CellSO : ScriptableObject
{
    public TerrainType terrainType;
    public bool available;
    [SerializeField] float avgHeight;
    [SerializeField] float varHeight;
    [SerializeField] ResourceDict maxResourcesDict;
    [SerializeField] ResourceDict startResourcesDict;
    [SerializeField] Color topColor;
    [SerializeField] Color sideColor;

    internal void Initialize(Cell cell)
    {

    }

    internal Container MakeContainer()
    {
        var container = new Container(maxResourcesDict, startResourcesDict);
        return container;
    }

    internal float CalculateHeight()
    {
        System.Random random = new();
        float randomFloat = (float)random.NextDouble();
        return avgHeight*(1 + varHeight*randomFloat);
    }

    internal Color GetTopColor()
    {
        return topColor;
    }

    internal Color GetSideColor()
    {
        return sideColor;
    }
}