using UnityEngine;
using System.Collections;
using System;
using JetBrains.Annotations;
using Unity.VisualScripting;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(TerrainManager))]
public class TerrainMesh : MonoBehaviour
{

    [SerializeField] Vector3 gridOffset;
    TerrainManager _terrainManager;

    private int _gridSize;
    float _cellSize;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Color[] colors;


    void Awake(){
        mesh = GetComponent<MeshFilter> ().mesh;
        _terrainManager = GetComponent<TerrainManager>();
        _gridSize = _terrainManager.gridSize;
        _cellSize = _terrainManager.cellSize;
    }

    void Start()
    {
        MakeProceduralGrid();
        InitializeMesh();
    }

private void MakeProceduralGrid()
{
    //set array sizes
    int totalVertices = 8 * _gridSize * _gridSize; // 8 vertices per prism
    int totalTriangles = 30 * _gridSize * _gridSize; // 10 triangles (5 sides + 1 top) * 3 indices each per prism

    vertices = new Vector3[totalVertices];
    triangles = new int[totalTriangles];
    colors = new Color[totalVertices];

    //tracker int
    int v = 0; //vertices
    int t = 0; //triangles
    for (int x = 0; x < _gridSize; x++)
    {
        for (int z = 0; z < _gridSize; z++)
        {
            Cell currentCell = _terrainManager.CellGrid[x, z];
            Vector3 cellOffset = new Vector3(x * _cellSize, 0, z * _cellSize);
            Vector3 totalOffset = cellOffset + gridOffset;

            AddRectangularPrism(totalOffset, currentCell, ref vertices, ref triangles, ref colors, ref v, ref t);
        }
    }
}

// Define a function to add a rectangular prism
void AddRectangularPrism(Vector3 topCenter, Cell cell, ref Vector3[] vertices, ref int[] triangles, ref Color[] colors, ref int v, ref int t)
{
    float height = cell.height;
    float halfWidth = _cellSize / 2;
    float halfDepth = _cellSize / 2;
    Color topColor = cell.GetTopColor();
    Color sideColor = cell.GetSideColor();

    // Top face vertices
    vertices[v + 0] = new Vector3(-halfWidth, height, -halfDepth) + topCenter;
    vertices[v + 1] = new Vector3(-halfWidth, height, halfDepth) + topCenter;
    vertices[v + 2] = new Vector3(halfWidth, height, -halfDepth) + topCenter;
    vertices[v + 3] = new Vector3(halfWidth, height, halfDepth) + topCenter;

    // Bottom face vertices (y = 0)
    vertices[v + 4] = new Vector3(-halfWidth, 0, -halfDepth) + topCenter;
    vertices[v + 5] = new Vector3(-halfWidth, 0, halfDepth) + topCenter;
    vertices[v + 6] = new Vector3(halfWidth, 0, -halfDepth) + topCenter;
    vertices[v + 7] = new Vector3(halfWidth, 0, halfDepth) + topCenter;

    // Top face triangles
    triangles[t + 0] = v + 0;
    triangles[t + 1] = v + 1;
    triangles[t + 2] = v + 2;
    triangles[t + 3] = v + 2;
    triangles[t + 4] = v + 1;
    triangles[t + 5] = v + 3;

    // Side face triangles
    // Side 1
    triangles[t + 6] = v + 0;
    triangles[t + 7] = v + 4;
    triangles[t + 8] = v + 1;
    triangles[t + 9] = v + 1;
    triangles[t + 10] = v + 4;
    triangles[t + 11] = v + 5;

    // Side 2
    triangles[t + 12] = v + 1;
    triangles[t + 13] = v + 5;
    triangles[t + 14] = v + 3;
    triangles[t + 15] = v + 3;
    triangles[t + 16] = v + 5;
    triangles[t + 17] = v + 7;

    // Side 3
    triangles[t + 18] = v + 2;
    triangles[t + 19] = v + 6;
    triangles[t + 20] = v + 0;
    triangles[t + 21] = v + 0;
    triangles[t + 22] = v + 6;
    triangles[t + 23] = v + 4;

    // Side 4
    triangles[t + 24] = v + 3;
    triangles[t + 25] = v + 7;
    triangles[t + 26] = v + 2;
    triangles[t + 27] = v + 2;
    triangles[t + 28] = v + 7;
    triangles[t + 29] = v + 6;

    // Top face colors
    colors[v + 0] = topColor;
    colors[v + 1] = topColor;
    colors[v + 2] = topColor;
    colors[v + 3] = topColor;

    // Side 1 colors
    colors[v + 0] = sideColor;
    colors[v + 4] = sideColor;
    colors[v + 1] = sideColor;
    colors[v + 5] = sideColor;

    // Side 2 colors
    colors[v + 1] = sideColor;
    colors[v + 5] = sideColor;
    colors[v + 3] = sideColor;
    colors[v + 7] = sideColor;

    // Side 3 colors
    colors[v + 2] = sideColor;
    colors[v + 6] = sideColor;
    colors[v + 0] = sideColor;
    colors[v + 4] = sideColor;

    // Side 4 colors
    colors[v + 3] = sideColor;
    colors[v + 7] = sideColor;
    colors[v + 2] = sideColor;
    colors[v + 6] = sideColor;


    v += 8; // 8 vertices per prism
    t += 30; // 10 triangles (5 sides + 1 top) * 3 indices each
}


    private void InitializeMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();
    }

}