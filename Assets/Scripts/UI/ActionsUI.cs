using UnityEngine;
using UnityEngine.UIElements;

public class ActionBuilderUI : MonoBehaviour
{
    [SerializeField] UIDocument builderUIDocument;
    [SerializeField] VisualTreeAsset actionSlotTemplate;
    public GlobalBlueprint _currentSelectionBlueprint;
    public GlobalInventorable currentSelectedInventorable;

    VisualElement _blueprintBuilderRoot;
    void Start()
    {
        _blueprintBuilderRoot = builderUIDocument.rootVisualElement.Q<VisualElement>("character-builder").Q<VisualElement>("actions");
        DisplayBlueprint();
    }
    
    public void DisplayBlueprint()
    {
        _currentSelectionBlueprint.OnValueChanged += UpdateBlueprint;
        UpdateBlueprint(_currentSelectionBlueprint.Value);
    }

    public void UpdateBlueprint(Blueprint newBlueprint)
    {
        _blueprintBuilderRoot.Clear();
        int slotIdx = 0;
        foreach (var action in _currentSelectionBlueprint.Value.activeActions)
        {
            var slotElement = DisplayActionSlot(action, slotIdx);
            _blueprintBuilderRoot.Add(slotElement);
            slotIdx++;
        }
    }

    public VisualElement DisplayActionSlot(ActionSO action, int slotIdx)
    {
        var slotElement = actionSlotTemplate.CloneTree();
        var iconElement = slotElement.Q<VisualElement>("icon");
        if (action.Sprite) iconElement.style.backgroundImage = new StyleBackground(action.Sprite.texture);
        var labelElement = slotElement.Q<Label>("label");
        labelElement.text = action.Name;
        int captureIdx = slotIdx; //needed to reference slotIdx by value and not by ref ;
        slotElement.RegisterCallback<PointerDownEvent>(evt => OnEquipItemClicked(evt, action, captureIdx));
        return slotElement;
    }

    private void OnEquipItemClicked(PointerDownEvent evt, ActionSO piece, int slotIdx)
    {
        if (evt.button == (int)MouseButton.LeftMouse)
        {
            currentSelectedInventorable.Value = piece;
        }
        if (evt.button == (int)MouseButton.RightMouse)
        {
            var selectedAction = currentSelectedInventorable.Value as ActionSO;
            if (selectedAction == null) return;
            bool successfullyAttached =_currentSelectionBlueprint.SetAction(slotIdx, selectedAction);
            Debug.Log($"Attaching piece {selectedAction} to Blueprint slot {slotIdx}, success: {successfullyAttached}");
        }

    }


}