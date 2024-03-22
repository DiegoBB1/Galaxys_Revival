using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private ZoomTransition zoomTransition;
    [SerializeField] public GameObject mainCanvas;
    [SerializeField] public GameObject sideCanvas;

    [SerializeField] public Camera mainCamera;


    public void StartGame(){
        zoomTransition.zoomIn("ShipHub");
        //SceneManager.LoadScene("MainScene");
    }

    public void howToPlay(){
        zoomTransition.swapCanvas(mainCanvas, sideCanvas, mainCamera);
    }

    public void back(){
        zoomTransition.swapCanvas(sideCanvas, mainCanvas, mainCamera);

    }
    public void QuitGame(){
        Application.Quit();
    }
}
