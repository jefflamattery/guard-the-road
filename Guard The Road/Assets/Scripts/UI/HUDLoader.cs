using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDLoader : MonoBehaviour
{
    [SerializeField] private string _globalUIScene;

    private string _currentScene;

    void Awake()
    {
        if(HUDManager.instance == null){
            // the GlobalUI scene needs to be loaded
            DontDestroyOnLoad(gameObject);

            _currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(_globalUIScene);

            SceneManager.LoadScene(_currentScene);
            


        }
    }
}
