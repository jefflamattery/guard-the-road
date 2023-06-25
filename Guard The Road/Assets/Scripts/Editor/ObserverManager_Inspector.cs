using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(ObserverManager))]
public class ObserverManager_Inspector : Editor
{
    public VisualTreeAsset _visualTree;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement observerInspector = new VisualElement();
        Button injectButton = new Button();

        injectButton.text = "Inject Observers into " + target.name;
        injectButton.clicked += OnMouseDown;

        observerInspector.Add(injectButton);
        _visualTree.CloneTree(observerInspector);

        
        return observerInspector;

    }

    void OnMouseDown()
    {
        ObserverManager observerManager = (ObserverManager)target;
        observerManager.InjectObservers();
    }

    
}
