using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] public GameObject enemy1;
    [SerializeField] public GameObject enemy2;
    [SerializeField] public GameObject enemy3;
    [SerializeField] public GameObject enemy4;
    [SerializeField] public GameObject enemy5;
    [SerializeField] public GameObject enemy6;
    [SerializeField] public Player player;

    [SerializeField] int maxEnemyCount = 10;
    GameObject [] enemies;
    public static int currentEnemies = 0;
    void Start()
    {
        //SpawnObjects();
        // countEnemies();
        currentEnemies = 0;
    }

    void Update(){

    }

    public void spawnEnemies(){
        StartCoroutine(SpawnRoutine());

        IEnumerator SpawnRoutine(){
            while(true){
                yield return new WaitForSeconds(.25f);            
                // enemies = GameObject.FindGameObjectsWithTag("Enemy");
                if(currentEnemies < maxEnemyCount){
                //Spawn enemy function, switch case
                // 6 weakest 50%
                // 125 same 30%
                // 34 largest 20%
                    
                    int enemyType = Random.Range(0,10);

                    if(enemyType < 5){ //0,1,2,3,4
                        spawnEnemyRandom(enemy6);
                    }
                    else if(enemyType == 5){
                        spawnEnemyRandom(enemy1);
                    }
                    else if(enemyType == 6){
                        spawnEnemyRandom(enemy2);
                    }
                    else if(enemyType == 7){
                        spawnEnemyRandom(enemy5);
                    }
                    else if(enemyType == 8){
                        spawnEnemyRandom(enemy3);
                    }
                    else{
                        spawnEnemyRandom(enemy4);
                    }

                    currentEnemies++;

                }
            }
        }
    }
    // void countEnemies(){
    //     StartCoroutine(CountRoutine());

    //     IEnumerator CountRoutine(){
    //         while(true){
    //             yield return new WaitForSeconds(5);            
    //             enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //             // Debug.Log("Current Enemy Count: " + enemies.Length);
    //         }
    //     }
    // }

    public void spawnEnemyRandom(GameObject enemy){
        //Random number is generated to determine what axis to spawn the enemy along.
        float axis = Random.Range(0,2);
        float randomX;
        float randomY;

        //If axis is 0, then the enemy will potentially spawn along all available x values within the used range
        if(axis == 0){
            //Generates a random point off screen, within the boundaries on the entire map
            do 
            {
                randomX = Random.Range(player.transform.position.x -32 , player.transform.position.x + 32);
            }
            while (randomX < -97 || randomX > 97);

            
            do 
            {
                randomY = Random.Range(1, 3)==1 ? Random.Range(player.transform.position.y-23, player.transform.position.y-13) : Random.Range(player.transform.position.y+13, player.transform.position.y+23);
            }
            while (randomY < -45 || randomY > 49);
        }
        //If axis is 1, then the enemy will potentially spawn along all available y values within the used range
        else{
            //Generates a random point off screen, within the boundaries on the entire map
            do 
            {
                randomX = Random.Range(1, 3)==1 ? Random.Range(player.transform.position.x-32, player.transform.position.x-22) : Random.Range(player.transform.position.x+22, player.transform.position.x+32);
            }
            while (randomX < -97 || randomX > 97);

            
            do 
            {
                randomY = Random.Range(player.transform.position.y -23 , player.transform.position.y + 23);
            }
            while (randomY < -45 || randomY > 49);
        }

        GameObject newEnemy = Instantiate(enemy, new Vector3(randomX, randomY, 0),Quaternion.identity);

        //Enemy becomes stronger based on how many systems have been completed
        newEnemy.GetComponent<Enemy>().enemyHealth += Player.systemsComplete;
        newEnemy.GetComponent<Enemy>().enemySpeed += Player.systemsComplete;
        newEnemy.GetComponent<Enemy>().damageDealt += Player.systemsComplete;
    }

}