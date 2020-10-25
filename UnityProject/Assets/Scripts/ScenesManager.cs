using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{

    // Get references to all of the scenes.
    Scene s_mainMenu;
    Scene s_inGame;
    
    
    
    void Awake()
    {
        // Keep this object around through the scenes (not necessary).
        // DontDestroyOnLoad(gameObject);

           

        // Get the scenes.
        s_mainMenu = SceneManager.GetSceneByBuildIndex(0);
        s_inGame = SceneManager.GetSceneByBuildIndex(1);

        // Set the onclick of the level button
        GameObject.Find("Level 1 Button").GetComponent<Button>().onClick.AddListener(() => { LoadScene(1); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadSceneAsync(index);
        Debug.Log("Loaded scene " + index);
    }
}
