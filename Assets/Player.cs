using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float playerSpeed = 0f;
    [SerializeField] GameObject weaponProjectile;
    [SerializeField] float projectileSpeed = 5f;

    [SerializeField] public int enemiesRequired = 100;
    Rigidbody2D rb;

    public static int currency = 100;

    public bool objectiveCompleted = false; 
    public float playerHealth = 3;

    public int enemiesDefeated = 0;
    // void Awake(){
    //     this.playerHealth = 3;
    // }

    // Start is called before the first frame update
    void Start()
    {
        playerSpeed = 6;
        playerHealth = 3;
        rb = GetComponent<Rigidbody2D>();
    }

    public void movePlayer(Vector3 input){
        rb.velocity = input * playerSpeed;
    }

    public void shootProjectile(Vector3 pos, Vector3 direction){
        GameObject newProjectile = Instantiate(weaponProjectile, transform.position, Quaternion.Euler(pos));
        newProjectile.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed);
        Destroy(newProjectile, 5);
    }
    // public void gameOver(){
    //     //Randomly selects one of the three game over sound effects to play, then loads the game over screen.
    //     int randomSound = Random.Range(0,2);

    //     if(randomSound == 0){
    //         gameOver1.Play();
    //     }
    //     else if(randomSound == 1){
    //         gameOver2.Play();
    //     }
    //     else{
    //         gameOver3.Play();
    //     }

    //     gameOverMenu.displayMenu();
    // }
}
