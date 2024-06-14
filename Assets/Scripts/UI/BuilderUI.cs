using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BuilderUI : MonoBehaviour
{
    [SerializeField] UIDocument builderUIDocument;
    [SerializeField] VisualTreeAsset equipSlotTemplate;
    public GlobalBlueprint _currentSelectionBlueprint;
    public GlobalInventorable currentSelectedInventorable;
    public BlueprintsManager dNABuilder;

    VisualElement _blueprintBuilderRoot;
    VisualElement _blueprintSlotRoot;

    void Start()
    {
        _blueprintBuilderRoot = builderUIDocument.rootVisualElement.Q<VisualElement>("character-builder").Q<VisualElement>("pieces");
        _blueprintSlotRoot = builderUIDocument.rootVisualElement.Q<VisualElement>("character-builder").Q<VisualElement>("blueprint");
        builderUIDocument.rootVisualElement.Q<Button>("load-button").clicked += () => dNABuilder.LoadBlueprintToCurrent(0);
        builderUIDocument.rootVisualElement.Q<Button>("save-button").clicked += () => dNABuilder.SaveBlueprint(0);
        dNABuilder.StartNewBlueprint(_currentSelectionBlueprint.Value.blueprintSO);
        SetupBuilderDisplay();
    }
    
    public void SetupBuilderDisplay()
    {
        _currentSelectionBlueprint.OnValueChanged += UpdateBlueprint;
        _blueprintSlotRoot.RegisterCallback<PointerDownEvent>(evt => OnBlueprintSlotClicked(evt));    
        UpdateBlueprint(_currentSelectionBlueprint.Value);
    }

    public void UpdateBlueprint(Blueprint oldBlueprint)
    {
        var newBlueprint = _currentSelectionBlueprint.Value;
        UpdateBlueprintSlot(newBlueprint.blueprintSO);
        UpdateEquipSlots(newBlueprint);
    }

    private void UpdateEquipSlots(Blueprint newBlueprint)
    {
        _blueprintBuilderRoot.Clear();
        int slotIdx = 0;
        foreach (var piece in newBlueprint.pieces)
        {
            var slotElement = CreateEquipSlot(piece, slotIdx);
            _blueprintBuilderRoot.Add(slotElement);
            slotIdx++;
        }
    }

    public VisualElement CreateEquipSlot(IInventorable piece, int slotIdx)
    {
        var slotElement = equipSlotTemplate.CloneTree();
        DisplayInventorable(slotElement, piece);
        int captureIdx = slotIdx; //needed to reference slotIdx by value and not by ref ;
        slotElement.RegisterCallback<PointerDownEvent>(evt => OnEquipItemClicked(evt, piece, captureIdx));
        return slotElement;
    }

    public void UpdateBlueprintSlot(BlueprintSO blueprintSO)
    {
        DisplayInventorable(_blueprintSlotRoot, blueprintSO);
    }

    public void DisplayInventorable(VisualElement slot, IInventorable inv)
    {
        var iconElement = slot.Q<VisualElement>("icon");
        iconElement.style.backgroundImage = new StyleBackground(inv.Sprite.texture);
        var labelElement = slot.Q<Label>("label");
        labelElement.text = inv.Name;
    }

    private void OnBlueprintSlotClicked(PointerDownEvent evt)
    {
        if (evt.button == (int)MouseButton.LeftMouse)
        {
            currentSelectedInventorable.Value = _currentSelectionBlueprint.Value.blueprintSO;
        }
        if (evt.button == (int)MouseButton.RightMouse)
        {
            BlueprintSO selectedPiece = currentSelectedInventorable.Value as BlueprintSO;
            if (selectedPiece == null) return;
            dNABuilder.StartNewBlueprint(selectedPiece);
            Debug.Log($"Creating a new Blueprint from {selectedPiece}");
        }
    }

    private void OnEquipItemClicked(PointerDownEvent evt, IInventorable piece, int slotIdx)
    {
        if (evt.button == (int)MouseButton.LeftMouse)
        {
            currentSelectedInventorable.Value = piece;
        }
        if (evt.button == (int)MouseButton.RightMouse)
        {
            BuildingPieceSO selectedPiece = currentSelectedInventorable.Value as BuildingPieceSO;
            if (selectedPiece == null) return;
            bool successfullyAttached =_currentSelectionBlueprint.SetPiece(selectedPiece, slotIdx);
            Debug.Log($"Attaching piece {selectedPiece} to Blueprint slot {slotIdx}, success: {successfullyAttached}");
        }

    }


}