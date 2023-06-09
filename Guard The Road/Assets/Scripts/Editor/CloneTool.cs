using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CloneTool
{
    private static string ENTITIES_FOLDER = "Assets/Scripts/Scriptable Objects/Agents";

    [MenuItem("GameObject/Build Observers", false, 0)]
    private static void BuildObservers()
    {
        MotionObserver motion;
        FieldObserver field;
        IMotionObserver[] motionComponents;
        IFieldObserver[] fieldComponents;


        string name = Selection.activeGameObject.name;

        string assetFolder = ENTITIES_FOLDER + "/" + name;
        
        // create a new folder with the same name as the game object that was clicked on
        AssetDatabase.CreateFolder(ENTITIES_FOLDER, Selection.activeGameObject.name);

        // create a new observers and put them in the newly created folder for this entity
        motion = ScriptableObject.CreateInstance<MotionObserver>();
        field = ScriptableObject.CreateInstance<FieldObserver>();

        AssetDatabase.CreateAsset(motion, assetFolder + "/" + name + " Motion.asset");
        AssetDatabase.CreateAsset(field, assetFolder + "/" + name + " Field.asset");

        // attach these newly created observers to any component that implements them
        motionComponents = Selection.activeGameObject.GetComponentsInChildren<IMotionObserver>();
        fieldComponents = Selection.activeGameObject.GetComponentsInChildren<IFieldObserver>();

        foreach(IMotionObserver motionComponent in motionComponents)
        {
            motionComponent.Motion = motion;
        }

        foreach(IFieldObserver fieldComponent in fieldComponents)
        {
            fieldComponent.Field = field;
        }
    }


}
