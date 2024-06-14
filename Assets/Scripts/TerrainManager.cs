using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    [SerializeField] ThingBehaviour[] objectsToPlace; // Array of prefabs
    [SerializeField] SpawnerSO spawner;

    [SerializeField] GlobalGameState gameState;

    [SerializeField] GlobalThingBehaviour currentSelectedThing;

    [SerializeField] List<CellSO> celltypes;

    [SerializeField] LayerMask terrainLayer; // Layer mask for the terrain
    [SerializeField] LayerMask selectLayer; // Layer mask for the terrain


    // CELL MANAGER_________________
    public Cell[,] CellGrid {get; private set;}

    public int gridSize;
    public float cellSize;

    // CELL MANAGER________________



    [SerializeField] bool stressTest = false;

    void Awake()
    {
        InitializeCellGrid();
    }

    void Start()
    {
        //STRESS TEST
        //place one object in each cell
        if (stressTest)
            for (int x = 0; x < gridSize; x++)
                for (int z = 0; z < gridSize; z++)
                    PlaceNewProbe(new Vector2Int(x, z));

    }

    private void InitializeCellGrid()
    {
        CellGrid = new Cell[gridSize, gridSize];
        Debug.Log($"Making cell grid, {gridSize} x {gridSize}");

        // Create all cells
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                CellGrid[x, z] = new Cell(this, x, z, celltypes[0]); //here change cell parameters 
            }
        }
    }

    Vector2Int WorldToGrid(Vector3 worldPosition) {
        int x = Mathf.FloorToInt(worldPosition.x / cellSize);
        int z = Mathf.FloorToInt(worldPosition.z / cellSize);
        x = Math.Clamp(x, 0, gridSize - 1);
        z = Math.Clamp(z, 0, gridSize - 1);
        return new Vector2Int(x, z);
    }

    public Vector3 GridToWorld(Vector2Int gridPosition) {
        float x = (gridPosition.x + 0.5f)* cellSize;
        float z = (gridPosition.y + 0.5f)* cellSize;  // Assuming the grid Y translates to world Z
        return new Vector3(x, 0, z);  // Assuming Y is always 0 in world space, adjust if needed
    }

    void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle();
        guiStyle.fontSize = 24; // Set the font size
        guiStyle.normal.textColor = Color.red; // Set the text color to red
        GUI.Label(new Rect(10, 10, 2000, 400), "NO SELECTION");

        // Display info at top left corner of the screen
        if (currentSelectedThing.Value)
        {
            string info = currentSelectedThing.Value.GetInfoString();
            GUI.Label(new Rect(10, 10, 2000, 400), info, guiStyle);
        }
    }

    void Update()
    {
        if (gameState.Value == GameState.EXPLORING) HandleTerrainClick();
    }

    private void HandleTerrainClick()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button clicked
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.Log("Cliked on " + Input.mousePosition.ToString());

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainLayer)) // Using the layer mask
            {
                Debug.Log("Trying to place object on " + hit.point.ToString());
                PlaceNewProbe(hit.point);
            }
        }

        if (Input.GetMouseButtonDown(1)) // Left mouse button clicked
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.Log("Clicked on " + Input.mousePosition.ToString());

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, selectLayer)) // Check for selectable objects
            {
                ThingBehaviour thingComponent = hit.collider.GetComponent<ThingBehaviour>();
                if (thingComponent != null)
                {
                    Debug.Log("Selected " + thingComponent);
                    currentSelectedThing.Value = thingComponent;
                }
            }
            else
            {
                if (currentSelectedThing.Value != null)
                {
                    currentSelectedThing.Value = null;
                }
            }
        }
    }

    public int PlaceNewProbe(Vector3 worldPosition){
        Vector2Int gridPosition = WorldToGrid(worldPosition);
        return PlaceNewProbe(gridPosition);
    }
    
    public int PlaceNewProbe(Vector2Int gridPosition)
    {
        Debug.Log("Making a new one");

        int radius = 0;
        if (IsAreaOccupied(gridPosition,radius))
        {
            Debug.Log("Cannot place here (area occupied)");
            return 0; 
        }        
        Cell currentCell = CellGrid[gridPosition[0], gridPosition[1]];
        ThingBehaviour instance = spawner.SpawnCurrentSelection(currentCell);
        instance.Active = true;
        instance.Container.AddResource(0.5f, ResourceType.energy, fractional: true);

        return 1; //placement successfull
    }

    bool IsAreaOccupied(Vector2Int gridPosition, int radius)
    {
        int x_min = Math.Max(gridPosition[0]-radius, 0);
        int z_min = Math.Max(gridPosition[1]-radius, 0);
        
        int x_max = Math.Min(gridPosition[0] + radius + 1, gridSize);
        int z_max = Math.Min(gridPosition[1] + radius + 1, gridSize);

        for (int x = x_min; x < x_max; x++)
            for (int z = z_min; z < z_max; z++)
                if (! CellGrid[x, z].available)
                    return true;

        return false;    
    }

    //this should be handled between the object and the cell, not here. 
    void MarkGridAsOccupied(Vector2Int gridPosition, int radius)
    {
        int x_min = Math.Max(gridPosition[0]-radius, 0);
        int y_min = Math.Max(gridPosition[1]-radius, 0);
        
        int x_max = Math.Min(gridPosition[0] + radius + 1, gridSize);
        int y_max = Math.Min(gridPosition[1] + radius + 1, gridSize);

        Debug.Log("Marking area as occupied: x=[" + x_min + "," + x_max + "[ y=[" + y_min + "," + y_max + "[");

        for (int x = x_min; x < x_max; x++)
            for (int y = y_min; y < y_max; y++)
            {
                Debug.Log("MARKED (" + x + "," + y + ")");
                CellGrid[x, y].available = false;
            }            
    }

    void OnDrawGizmos()
    {
        if (CellGrid == null) return;

        Gizmos.color = Color.white;

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 cellCenter = new Vector3((x + 0.5f)* cellSize, 0.0f, (y + 0.5f)* cellSize);

                // Color occupied cells differently
                Gizmos.color = CellGrid[x, y].available ? Color.green : Color.red;

                // Draw a small cube at the center of each grid cell
                Gizmos.DrawCube(cellCenter, new Vector3(0.5f, 0.5f, 0.5f)); // Adjust the size as needed
            }
        }
    }
}

