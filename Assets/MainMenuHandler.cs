using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private ZoomTransition zoomTransition;

    public void StartGame(){
        zoomTransition.zoomIn();
        //SceneManager.LoadScene("MainScene");
    }

    public void QuitGame(){
        Application.Quit();
    }
}
