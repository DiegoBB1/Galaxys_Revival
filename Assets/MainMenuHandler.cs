using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private ZoomTransition zoomTransition;
    [SerializeField] public GameObject mainCanvas;
    [SerializeField] public GameObject sideCanvas;

    [SerializeField] public Camera mainCamera;
    [SerializeField] public Image backgroundImage;

    [SerializeField] public List<Sprite> backgroundImages;

    public void Awake(){
        backgroundImage.sprite = backgroundImages.ElementAt(Random.Range(0, backgroundImages.Count));
    }
    public void StartGame(){
        //Reset Enemy Stats
        //TODO: Determine best way to reset the unique values back to their default values.
        // Enemy.enemyHealth -= Player.systemsComplete;
        // Enemy.enemySpeed -= Player.systemsComplete;
        // Enemy.damageDealt -= Player.systemsComplete;
        EnemyAI.addedSight = 0;

        //Reset Player stats
        Player.currency = 150;
        Player.playerHealth = 3;
        Player.maxHealth = 3;
        Player.playerSpeed = 5;
        Player.strength = 1;
        Player.defense = 0;
        Player.luck = .1f;
        Player.projectileSpeed = 3f;
        Player.projectileCooldown = 1;

        //Reset Player Upgrades
        Player.weaponsPurchased.Clear();
        Player.abilitiesPurchased.Clear();
        Player.passivesPurchased.Clear();
        Player.weaponsPurchased.Add("Default");
        Player.passivesPurchased.Add("None");
        Player.abilitiesPurchased.Add("None");
        Player.weaponEquipped = "Default";
        Player.passiveEquipped = "None";
        Player.abilityEquipped = "None";

        //Reset other Player variables
        Player.enemiesRequired = 20;
        Player.systemsComplete = 0;
        Player.totalPlanets = 0;
       
        ShipHubHandler.planetsLiberated = 0;
        ShipHubHandler.planetsRequired = 3;

        zoomTransition.ZoomIn("ShipHub");
        //SceneManager.LoadScene("MainScene");
    }

    public void HowToPlay(){
        zoomTransition.SwapCanvas(mainCanvas, sideCanvas, mainCamera);
    }

    public void Back(){
        zoomTransition.SwapCanvas(sideCanvas, mainCanvas, mainCamera);

    }
    public void QuitGame(){
        Application.Quit();
    }
}
