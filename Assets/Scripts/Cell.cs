using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public enum TerrainType { Grass, Water, Mountain }

public class Cell
{
    public TerrainType terrainType;
    public float height;
    public bool available;
    public Container container;
    public ThingBehaviour attached;
    private TerrainManager manager;
    public CellSO cellSO;

    public int x_coord;
    public int z_coord;

    public Cell(TerrainManager manager, int x_coord, int z_coord, ResourceDict maxCapacity, TerrainType terrainType = 0, float height = 0)
    {
        this.manager = manager;
        this.x_coord = x_coord;
        this.z_coord = z_coord;
        this.terrainType = terrainType;
        this.height = height;
        this.available = terrainType != TerrainType.Water; //todo use scriptable objects
        this.container = new Container(maxCapacity);
        container.AddResource(100, ResourceType.iron);
    }

    public Cell(TerrainManager manager, int x_coord, int z_coord, CellSO cellSO)
    {
        this.manager = manager;
        this.x_coord = x_coord;
        this.z_coord = z_coord;
        this.cellSO = cellSO;
        terrainType = cellSO.terrainType;
        height = cellSO.CalculateHeight();
        available = cellSO.available;
        container = cellSO.MakeContainer();
        cellSO.Initialize(this);
    }

    public Vector3 WorldPosition
    {
        get {return manager.GridToWorld(new Vector2Int(x_coord, z_coord));}
    }

    public Cell FindInPos(Vector2Int relativePos){
        return manager.CellGrid[x_coord + relativePos[0], z_coord + relativePos[1]];
    }

    public string GetInfoString()
    {
        string info = $"cell {x_coord},{z_coord}\n";
        info += "CONTENT: \n";
        info += container.GetContentString();
        return info;
    }

    public List<Cell> GetNeighbours(bool onlyAvailable, int radius = 1, bool noDiagonal = true)
    {
        List<Cell> potentialCells = new List<Cell>();

        // Loop through a square surrounding the current cell defined by the radius
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dz = -radius; dz <= radius; dz++)
            {
                // Skip the center cell where dx and dz are 0
                if (dx == 0 && dz == 0) continue;
                if (noDiagonal && dx != 0 && dz != 0) continue;

                try
                {
                    // Get the cell at this relative position
                    Cell neighbour = FindInPos(new Vector2Int(dx, dz));
                    if (neighbour != null)
                    {
                        // If only available cells are required, check the condition
                        if (!onlyAvailable || (onlyAvailable && neighbour.available))
                        {
                            potentialCells.Add(neighbour);
                        }
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    //do nothing if cell out of bound
                    continue;
                }
            }
        }
        return potentialCells;

    }

    public Cell GetRandomNeighbour(bool onlyAvailable = true, int radius = 1, bool noDiagonal = true)
    {
        
        var potentialCells = GetNeighbours(onlyAvailable, radius, noDiagonal);
        // Randomly select one of the potential cells if any are available
        if (potentialCells.Count == 0) return null;

        System.Random rand = new System.Random();
        return potentialCells[rand.Next(potentialCells.Count)];
    }

    public void Attach(ThingBehaviour thing)
    {
        attached = thing;
        available = false;
    }

    public void Detach(ThingBehaviour thing)
    {
        attached = null;
        available = true;
    }

    internal UnityEngine.Color GetTopColor()
    {
        return cellSO.GetTopColor();
    }

    internal UnityEngine.Color GetSideColor()
    {
        return cellSO.GetSideColor();
    }
}
