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
        //Reset Enemy Stats

        //TODO: Determine best way to reset the unique values back to their default values.
        // Enemy.enemyHealth -= Player.systemsComplete;
        // Enemy.enemySpeed -= Player.systemsComplete;
        // Enemy.damageDealt -= Player.systemsComplete;
        EnemyAI.sightDistance -= Player.systemsComplete * 2;

        //Reset Player stats and variables
        Player.playerSpeed = 5f;
        Player.currency = 150;
        Player.projectileCooldown = 1;
        Player.playerHealth = 5;
        Player.strength = 1;
        Player.defense = 0;
        Player.pierceProjectiles = false;
        Player.systemsComplete = 0;
        Player.totalPlanets = 0;

        ShipHubHandler.planetsLiberated = 0;
        ShipHubHandler.planetsRequired = 3;

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
