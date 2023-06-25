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
        string name = Selection.activeGameObject.name;
        string assetFolder = ENTITIES_FOLDER + "/" + name;

        ObserverManager observerManager = Selection.activeGameObject.GetComponent<ObserverManager>();
        if(observerManager == null){return;}
        
        // create a new folder with the same name as the game object that was clicked on
        if(!AssetDatabase.IsValidFolder(assetFolder)){
            AssetDatabase.CreateFolder(ENTITIES_FOLDER, Selection.activeGameObject.name);
        }

        observerManager.BuildObservers();

        // WORKFLOW: ADD NEW OBSERVER TYPES HERE //
        CreateObserverAsset(observerManager.Motion, assetFolder + "/" + name + " Motion.asset");
        CreateObserverAsset(observerManager.Field, assetFolder + "/" + name + " Field.asset");
        CreateObserverAsset(observerManager.Animation, assetFolder + "/" + name + " Animation.asset");
        CreateObserverAsset(observerManager.Weapon, assetFolder + "/" + name + " Weapon.asset");
        CreateObserverAsset(observerManager.Vitals, assetFolder + "/" + name + " Vitals.asset");
        CreateObserverAsset(observerManager.Agent, assetFolder + "/" + name + " Agent.asset");
        CreateObserverAsset(observerManager.Action,  assetFolder + "/" + name + " Action.asset");
        ///////////////////////////////////////////

        observerManager.InjectObservers();
        
        
    }

    private static void CreateObserverAsset(Observer observer, string path){
        AssetDatabase.DeleteAsset(path);
        AssetDatabase.CreateAsset(observer, path);
    }


}
