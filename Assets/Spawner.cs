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
    [SerializeField] int maxEnemyCount = 10;
    GameObject [] enemies;

    void Start()
    {
        //SpawnObjects();
        countEnemies();
    }

    void Update(){
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemies.Length < maxEnemyCount){
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
        }
    }

    void countEnemies(){
        StartCoroutine(CountRoutine());

        IEnumerator CountRoutine(){
            while(true){
                yield return new WaitForSeconds(5);            
                enemies = GameObject.FindGameObjectsWithTag("Enemy");
                // Debug.Log("Current Enemy Count: " + enemies.Length);
            }
        }
    }

    // void SpawnObjects(){
    //     StartCoroutine(SpawnerRoutine());
    //     StartCoroutine(SpawnerRoutine());

    //     IEnumerator SpawnerRoutine(){
    //         while(true){
    //             yield return new WaitForSeconds(1);            
    //             SpawnStarRandom();
    //             SpawnStarRandom();
    //             SpawnObstaclesRandom();
    //         }
    //     }
    // }
    public void spawnEnemyRandom(GameObject enemy){
        float randomX = Random.Range(-8,8.5f);
        float randomY = Random.Range(8, 15);
        GameObject newStar = Instantiate(enemy, new Vector3(randomX, randomY, 0),Quaternion.identity);

    }

}