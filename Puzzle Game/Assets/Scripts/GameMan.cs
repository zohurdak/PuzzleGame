using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMan : MonoBehaviour
{
    public int nextSceneToLoad;
   
    
    public void Start() {
        nextSceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    private void Update() {
        
    }
    public void LoadNextScene()
    {
        
        SceneManager.LoadScene(nextSceneToLoad);
    }
    

    
}