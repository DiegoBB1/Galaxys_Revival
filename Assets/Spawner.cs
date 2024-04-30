using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //All alien gameobjects are stored in a list to be called in the spawner
    [SerializeField] List<GameObject> aliens;

    //All robot gameobjects are stored in a list to be called in the spawner
    [SerializeField] List<GameObject> robots;

    //High Value Target gameobject to be used for the HVT encounter
    [SerializeField] public GameObject HVT1;
    [SerializeField] public GameObject HVT2;

    [SerializeField] public Player player;

    [SerializeField] int maxEnemyCount = 10;

    public static int currentEnemies = 0;
    private string enemyType;
    private int randomEnemy;
    private int numHVTs = 3;
    ContactFilter2D contactFilter = new ContactFilter2D();

    void Start()
    {
        contactFilter.NoFilter(); //create filter for the physical object layer and uncomment the spawnenemyrandomy implementation
        currentEnemies = 0;
        int randNum= Random.Range(0,2);
        if(randNum == 0)
            enemyType = "Aliens";
        else 
            enemyType = "Robots";

        if(EncounterHandler.currentEncounter == "High Value Targets"){
            Debug.Log("Spawning HVTs");
            SpawnHVTs(numHVTs);
        }
        
        
    }

    void Update(){

    }

    public void SpawnEnemies(){
        StartCoroutine(SpawnRoutine());

        IEnumerator SpawnRoutine(){
            while(true){
                yield return new WaitForSeconds(.25f);            
                if(currentEnemies < maxEnemyCount){
                    if(enemyType == "Aliens")
                        SpawnEnemyRandom(aliens.ElementAt(Random.Range(0,aliens.Count)));
                    else if(enemyType == "Robots")
                        SpawnEnemyRandom(robots.ElementAt(Random.Range(0,robots.Count)));
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

    public void SpawnEnemyRandom(GameObject enemy){
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

        //Check if they spawned inside of an object, if so, despawn them
        // List<Collider2D> results = new List<Collider2D>();
        // Physics2D.OverlapCircle(transform.position, 2.5f, contactFilter, results);
        // foreach (var hitCollider in results)
        // {
        //     if(hitCollider.gameObject.tag == "Enemy")
        //         return true;
            
        // }
        // return false;
        List<Collider2D> results = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        if(newEnemy.GetComponent<BoxCollider2D>() != null && Physics2D.OverlapCollider(newEnemy.GetComponent<BoxCollider2D>(), filter, results) > 0){
            Debug.Log("Enemy spawned in wall, calling again");
            Destroy(newEnemy.gameObject);
            SpawnEnemyRandom(enemy);
        }
        else if(newEnemy.GetComponent<PolygonCollider2D>() != null && Physics2D.OverlapCollider(newEnemy.GetComponent<PolygonCollider2D>(), filter, results) > 0){
            Debug.Log("Enemy spawned in wall, calling again");
            Destroy(newEnemy.gameObject);
            SpawnEnemyRandom(enemy);
        }
        

        //Enemy becomes stronger based on how many systems have been completed
        newEnemy.GetComponent<Enemy>().enemyHealth += Player.systemsComplete;
        newEnemy.GetComponent<Enemy>().enemySpeed += Player.systemsComplete;
        newEnemy.GetComponent<Enemy>().damageDealt += Player.systemsComplete;
    }

    public void SpawnHVTs(int num){
        for(int i = 0; i < num; i++){
            int randNum = Random.Range(0,2);
            if(randNum == 0)
                SpawnEnemyRandom(HVT1);
            else 
                SpawnEnemyRandom(HVT2);
        }
    }

}