using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private LevelManager levelManager;
    
    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel() {
        levelManager.LoadNextLevel();
    }

    public void LoadLevel(int level) {
        levelManager.SetLevel(level);
        levelManager.LoadNextLevel();
    }
}
