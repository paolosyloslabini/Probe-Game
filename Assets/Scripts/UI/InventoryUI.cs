using System;
using UnityEngine;
using UnityEngine.UIElements;

public enum InventoryType{PIECES, ACTIONS,BLUEPRINTS}

public class InventoryUI : MonoBehaviour
{
    [SerializeField] UIDocument builderUIDocument;
    [SerializeField] VisualTreeAsset inventorySlotTemplate;
    public GlobalInventorable currentSelection;
    [SerializeField] PiecesInventory piecesInventory;    
    [SerializeField] ActionsInventory actionsInventory;
    [SerializeField] BlueprintSOInventory blueprintSOInventory;

    [SerializeField] InventoryType visualizedInventory;   


//todo switch between blueprint inventory, action inventory, etc. 

    VisualElement _inventoryRoot;
    void Start()
    {
        var topRoot = builderUIDocument.rootVisualElement.Q<VisualElement>("inventory");
        _inventoryRoot = topRoot.Q<VisualElement>("SlotContainer");
        Button actionButton = topRoot.Q<Button>("button-actions");
        actionButton.clicked += () => VisualizeInventory(InventoryType.ACTIONS);

        Button piecesButton = topRoot.Q<Button>("button-pieces");
        piecesButton.clicked += () => VisualizeInventory(InventoryType.PIECES);

        Button blueprintButton = topRoot.Q<Button>("button-blueprints");
        blueprintButton.clicked += () => VisualizeInventory(InventoryType.BLUEPRINTS);

        VisualizeInventory(InventoryType.PIECES);
    }

    public void VisualizeInventory(InventoryType requestedInventory)
    {
        _inventoryRoot.Clear();
        visualizedInventory = requestedInventory;
        switch (requestedInventory) //this looks weird but its not possible to cast inventories onto the same variable
        {
            case InventoryType.PIECES:
                foreach (var piece in piecesInventory.availablePieces)
                {
                    AddInventorySlotUI(piece);
                }
                break;

            case InventoryType.ACTIONS:
                foreach (var action in actionsInventory.availablePieces)
                {
                    AddInventorySlotUI(action);
                }
                break;

            case InventoryType.BLUEPRINTS:
                foreach (var blueprint in blueprintSOInventory.availablePieces)
                {
                    AddInventorySlotUI(blueprint);
                }
                break;

    default:
        Debug.LogWarning("Unsupported InventoryType: " + requestedInventory);
        break;
}
    }

    public void AddInventorySlotUI(IInventorable piece)
    {
        // Create UI element for the item
        var slotElement = inventorySlotTemplate.CloneTree();
        var iconElement = slotElement.Q<VisualElement>("icon");
        iconElement.style.backgroundImage = new StyleBackground(piece.Sprite.texture);

        // Add click event listener
        slotElement.RegisterCallback<ClickEvent>(evt => OnInventoryItemClicked(piece));

        _inventoryRoot.Add(slotElement);
    }

    public void OnInventoryItemClicked(IInventorable piece)
    {
        currentSelection.Value = piece;        
    }

}

public class InventorySlotUI
{

}