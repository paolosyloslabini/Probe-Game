using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] int mapWidth;
    [SerializeField] int mapHeight;
    [SerializeField] int seed;
    [SerializeField] float noiseScale;
    [SerializeField] int octaves;
    [Range(0,1)] [SerializeField] float persistance;
    [SerializeField] float lacunarity;
    [SerializeField] Vector2 offset;
    [SerializeField] MapDisplay mapDisplay;
    [SerializeField] List<CellSO> cellSOs;
    [SerializeField] TerrainManager terrainManager;


    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity,offset);
        Texture2D texture = TextureGenerator.TextureFromHeightMap(noiseMap);
        mapDisplay.DrawTexture(texture);
        mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap), texture);
    } 

    public Cell[,] GenerateCells(float [,] map)
    {
        int height = map.GetLength(0);
        int width = map.GetLength(1);
        var cells = new Cell[height,width];
        for(int z = 0; z < height; z++)
            for (int x = 0; x < width; x++)
            {
                CellSO cellType = CellTypeColorMap(map[x,z]);
                cells[x,z] = new Cell(terrainManager, x, z, cellType);
            }
        return cells;
    }

    public CellSO CellTypeColorMap(float val)
    {
        if (val < 0 || val > 1) throw new InvalidOperationException("MakeCellfromFloat: val must be a fraction.");
        int idx = (int) val*cellSOs.Count;
        return cellSOs[idx];
    }

    void OnValidate()
    {
        GenerateMap();
    }
}
