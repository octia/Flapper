using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class MainMenuManager : MonoBehaviour
{

    public void LoadMainGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadOptions()
    {
	
    }


}
