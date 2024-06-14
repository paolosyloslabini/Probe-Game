using UnityEditor;
using UnityEngine;
using System;

public class BuildingPieceCreatorWindow : EditorWindow
{
    private string baseName = "BuildingPiece";
    private int numberOfInstances = 1;
    private string savePath = "Assets/ScriptableObjects/Builder/Pieces";

    [MenuItem("Tools/Building Piece Creator")]
    public static void ShowWindow()
    {
        GetWindow<BuildingPieceCreatorWindow>("Building Piece Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create Multiple Building Piece Instances", EditorStyles.boldLabel);

        savePath = EditorGUILayout.TextField("Save Path", savePath);

        if (GUILayout.Button("Create"))
        {
            CreateStubs();
        }
    }

    private void CreateStubs()
    {
        Debug.Log($"Creating stubs for {Enum.GetValues(typeof(BuildingPieceType))}");
        foreach (var type in Enum.GetValues(typeof(BuildingPieceType)))
        {
            StubPieceSO buildingPiece = ScriptableObject.CreateInstance<StubPieceSO>();
            buildingPiece.buildingPieceType = (BuildingPieceType) type;
            buildingPiece.Sprite = Resources.Load<Sprite>("Sprites/stubImg");
            buildingPiece.Name = type + " stub";

            string assetPath = $"{savePath}/{type}_stub.asset";
            AssetDatabase.CreateAsset(buildingPiece, assetPath);
            Debug.Log($"Stub of type {type} created at {savePath}");

        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }
}
