using UnityEngine;
using UnityEngine.UIElements;

public class InspectorUI : MonoBehaviour
{
    [SerializeField] UIDocument builderUIDocument;
    public GlobalInventorable currentSelectedInventorable;

    VisualElement _inspectorRoot;
    void Start()
    {
        _inspectorRoot = builderUIDocument.rootVisualElement.Q<VisualElement>("inspector");
        DisplayBlueprint();
    }
    
    public void DisplayBlueprint()
    {
        currentSelectedInventorable.OnValueChanged += UpdateInspector;
        UpdateInspector(currentSelectedInventorable.Value);
    }

    public void UpdateInspector(IInventorable newInventorable)
    {
        if (currentSelectedInventorable.Value == null) return;
        
        var image = _inspectorRoot.Q<VisualElement>("icon");
        image.style.backgroundImage = new StyleBackground(newInventorable.Sprite.texture);
        
        var name_label = _inspectorRoot.Q<Label>("name-label");
        name_label.text = newInventorable.Name;
        
        var description_label = _inspectorRoot.Q<Label>("description-label");
        description_label.text = newInventorable.Description;
    }


}