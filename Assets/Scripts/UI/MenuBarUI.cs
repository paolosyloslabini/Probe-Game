using System;
using System.Data.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuBarUI : MonoBehaviour
{
    [SerializeField] UIDocument builderUIDocument;
    [SerializeField] GlobalBool isUIActive;
    [SerializeField] GlobalGameState gameState;

    VisualElement menuElement;
    VisualElement builderElement;
    VisualElement inspectorElement;


    bool UIenabled = false;
    public void Start()
    {
        builderElement = builderUIDocument.rootVisualElement.Q<VisualElement>("BUILD-WINDOW");
        inspectorElement = builderUIDocument.rootVisualElement.Q<VisualElement>("INSPECT-WINDOW");
        
        menuElement = builderUIDocument.rootVisualElement.Q<VisualElement>("MENU");
        menuElement.Q<Button>("toggle-build-button").clicked += () => ToggleElement(builderElement);
        menuElement.Q<Button>("toggle-inspect-button").clicked += () => ToggleElement(inspectorElement);
        DisableUI();
    }

    public void DisableUI()
    {
        // Set the picking mode to Ignore to disable interactions
        // Set the display style to None to hide the UI
        foreach (var element in new VisualElement[] {inspectorElement, builderElement})
        {        
            element.style.display = DisplayStyle.None;
            element.pickingMode = PickingMode.Ignore;
        }
        isUIActive.Value = false;
        gameState.Value = GameState.EXPLORING;
    }

    public void EnableUI(VisualElement element)
    {
        // Set the display style to None to hide the UI
        element.style.display = DisplayStyle.Flex;
        
        // Set the picking mode to Ignore to disable interactions
        element.pickingMode = PickingMode.Position;

        isUIActive.Value = true;
        gameState.Value = GameState.BUILDING;
    }

    public void ToggleElement(VisualElement element)
    {
        if (!isUIActive.Value) EnableUI(element);
        else DisableUI();
    }

}