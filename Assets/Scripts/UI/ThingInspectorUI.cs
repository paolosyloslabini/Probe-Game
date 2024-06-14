using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ThingInspectorUI : MonoBehaviour
{
    [SerializeField] UIDocument builderUIDocument;
    [SerializeField] GlobalThingBehaviour selectedThing;

    [SerializeField] GlobalInventorable currentSelectedInventorable;
    [SerializeField] VisualTreeAsset equipSlotAsset;

    List<UISlot> pieceSlots = new();
    List<UISlot> actionUISlots = new();

    VisualElement inspectorElement;
    VisualElement piecesRoot;
    VisualElement containerRoot;
    VisualElement actionsRoot;

    //Much copypaste from BuilderUI. TODO integrate.
    public void Start()
    {
        inspectorElement = builderUIDocument.rootVisualElement.Q<VisualElement>("INSPECT-WINDOW");
        piecesRoot = inspectorElement.Q<VisualElement>("parts-root");
        containerRoot = inspectorElement.Q<VisualElement>("container-root");
        actionsRoot = inspectorElement.Q<VisualElement>("actions-root");
        selectedThing.OnValueChangedWithHistory += UpdateInspector;
        UpdateInspector(null, selectedThing.Value);
    }
    
    void UpdateInspector(ThingBehaviour oldThing, ThingBehaviour newThing)
    {
        UnsubscribeFromThing(oldThing);
        SubscribeToThing(newThing);
        UpdateEquipSlots(newThing);
        UpdateActionSlots(newThing);
    }

    private void SubscribeToThing(ThingBehaviour newThing)
    {
        if (newThing) newThing.OnActionTaken += ShowActionTaken;
        if (newThing) newThing.OnDestroyed += Unset;
    }

    private void Unset(ThingBehaviour thing = null)
    {
        UpdateInspector(thing, null);
    }


    private void UnsubscribeFromThing(ThingBehaviour oldThing)
    {
        if (oldThing) oldThing.OnActionTaken -= ShowActionTaken;
        if (oldThing) oldThing.OnDestroyed -=Unset;

    }

    private void UpdateEquipSlots(ThingBehaviour thing)
    {
        piecesRoot.Clear();
        pieceSlots.Clear();
        if (!thing) return;
        int slotIdx = 0;
        foreach (var piece in thing.dna.blueprint.pieces)
        {
            var uiSlot = new UISlot(equipSlotAsset, piecesRoot,piece,slotIdx);
            uiSlot.OnClicked += (evt, slot) => OnItemClicked(evt, slot.Inventorable, slot.SlotIdx);
            pieceSlots.Add(uiSlot);
            slotIdx++;
        }
    }

    private void UpdateActionSlots(ThingBehaviour thing)
    {
        actionsRoot.Clear();
        actionUISlots.Clear();
        if (!thing) return;
        int slotIdx = 0;
        foreach (var action in thing.dna.blueprint.activeActions)
        {
            var uiSlot = new UISlot(equipSlotAsset, actionsRoot,action,slotIdx);
            actionUISlots.Add(uiSlot);
            uiSlot.OnClicked += (evt, slot) => OnItemClicked(evt, slot.Inventorable, slot.SlotIdx);
            slotIdx++;
        }
    }

    private void ShowActionTaken(int idx)
    {
        foreach (var slot in actionUISlots)
        {
            if (slot.SlotIdx == idx) slot.Highlighted = true;
            else slot.Highlighted = false;
        }
    }

    private void OnItemClicked(PointerDownEvent evt, IInventorable piece, int slotIdx)
    {
        if (evt.button == (int)MouseButton.LeftMouse)
        {
            currentSelectedInventorable.Value = piece;
        }
    }

}
