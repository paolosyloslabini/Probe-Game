using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UISlot
{
    public IInventorable Inventorable {get; private set;}
    public int SlotIdx {get; private set;}
    public Action<PointerDownEvent, UISlot> OnClicked;

    bool _highlighted = false;
    public bool Highlighted{
        get => _highlighted;
        set{
            _highlighted = value;
            if (_highlighted) Highligth();
            else RemoveHighlight();
        }
    }

    [SerializeField] Color highlightBorderColor = Color.red;
    [SerializeField] Color standardBorderColor = Color.black;


    

    VisualElement slotRoot;

    public UISlot(VisualTreeAsset treeAsset, VisualElement baseRoot, IInventorable inventorable, int slotIdx)
    {
        this.Inventorable = inventorable;
        this.SlotIdx = slotIdx;
        this.slotRoot = treeAsset.CloneTree();
        baseRoot.Add(slotRoot);
        slotRoot.RegisterCallback<PointerDownEvent>(evt => OnClicked?.Invoke(evt, this));
        Highlighted = false;
        UpdateTarget(inventorable);
    }

    public void UpdateTarget(IInventorable newInventorable)
    {
        Unsubscribe(Inventorable);
        Inventorable = newInventorable;
        Subscribe(newInventorable);
        Display(newInventorable);
    }

    internal virtual void Subscribe(IInventorable newInventorable)
    {}

    internal virtual void Unsubscribe (IInventorable inventorable)
    {}

    public void Display(IInventorable inv)
    {
        if (inv == null) return;
        var iconElement = slotRoot.Q<VisualElement>("icon");
        iconElement.style.backgroundImage = new StyleBackground(inv.Sprite.texture);
        var labelElement = slotRoot.Q<Label>("label");
        labelElement.text = inv.Name;
    }

    internal void RemoveHighlight()
    {
        ChangeBorderStyle(standardBorderColor, 2);
    }

    internal void Highligth()
    {
        ChangeBorderStyle(highlightBorderColor, 4);
    }

    void ChangeBorderStyle(Color color, float width)
    {
        slotRoot.style.borderTopColor = new StyleColor(color);
        slotRoot.style.borderBottomColor = new StyleColor(color);
        slotRoot.style.borderLeftColor = new StyleColor(color);
        slotRoot.style.borderRightColor = new StyleColor(color);
        slotRoot.style.borderTopWidth = width;
        slotRoot.style.borderBottomWidth = width;
        slotRoot.style.borderLeftWidth = width;
        slotRoot.style.borderRightWidth = width;
    }
}