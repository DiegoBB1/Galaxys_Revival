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

    [SerializeField] int maxEnemyCount;

    public int currentEnemies = 0;
    private int numHVTs = 3;
    ContactFilter2D contactFilter = new ContactFilter2D();

    public List<GameObject> ActiveEnemies;
    public List<GameObject> InactiveEnemies;

    void Awake(){
        //To start, there are at most 20 enemies spawned in, which increases by 10 for every system completed. Caps at 80.
        maxEnemyCount = (20 + (10 * Player.systemsComplete) <= 80)? 20 + (10 * Player.systemsComplete): 80;
        //Allocate x enemies (plus 25% more for the inactive pool) of a given enemy type , adding them to the inactive enemies list
        int randNum= Random.Range(0,2);
        if(randNum == 0){
            for(int i = 0; i < maxEnemyCount * 1.25; i++){
                GameObject newEnemy = Instantiate(aliens.ElementAt(Random.Range(0,aliens.Count)), new Vector3(0, 0, 0),Quaternion.identity);
                newEnemy.SetActive(false);
                InactiveEnemies.Add(newEnemy);
            }
        }
        else{ 
            for(int i = 0; i < maxEnemyCount * 1.25; i++){
                GameObject newEnemy = Instantiate(robots.ElementAt(Random.Range(0,robots.Count)), new Vector3(0, 0, 0),Quaternion.identity);
                newEnemy.SetActive(false);
                InactiveEnemies.Add(newEnemy);
            }
        }
    }
    void Start()
    {
        contactFilter.NoFilter(); //create filter for the physical object layer and uncomment the spawnenemyrandomy implementation
        currentEnemies = 0;

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
            while(!player.exfiltrated){
                yield return new WaitForSeconds(.25f);            
                if(currentEnemies < maxEnemyCount && !player.exfiltrated){
                    //When "Spawning" an enemy, we get a reference to a random enemy in the inactive enemy list.
                    int randomNum = Random.Range(0,InactiveEnemies.Count);
                    GameObject newEnemy = InactiveEnemies.ElementAt(randomNum);
                    
                    //Move them to a random position and determine if that position is valid
                    newEnemy.SetActive(true);
                    newEnemy.transform.position = RandomPosition(newEnemy);

                    if(newEnemy.transform.position.z == 1){
                        Debug.Log("Invalid Spawn");
                        newEnemy.SetActive(false);
                        continue;
                    }

                    //If the position is valid, the enemy is removed from the inactive enemy list and has its stats reset.
                    InactiveEnemies.RemoveAt(randomNum);
                    ResetEnemyStats(newEnemy);
                    
                    //Once enemy is reset, they are made active and added to the active enemy list
                    newEnemy.SetActive(true);
                    ActiveEnemies.Add(newEnemy);

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

    public Vector3 RandomPosition(GameObject enemy){
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

        //Check if they spawned inside of an object, if so, despawn them
        // List<Collider2D> results = new List<Collider2D>();
        // Physics2D.OverlapCircle(transform.position, 2.5f, contactFilter, results);
        // foreach (var hitCollider in results)
        // {
        //     if(hitCollider.gameObject.tag == "Enemy")
        //         return true;
            
        // }
        // return false;
        Vector3 resultPosition = new Vector3(randomX, randomY, 0);
        List<Collider2D> results = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D().NoFilter();

        if(enemy.GetComponent<BoxCollider2D>() != null && Physics2D.OverlapCollider(enemy.GetComponent<BoxCollider2D>(), filter, results) > 0)
            resultPosition = new Vector3(0, 0, 1);

        return resultPosition;
    }


    public bool HVTPosition(GameObject enemy){
        //Random number is generated to determine what axis to spawn the enemy along.
        float axis = Random.Range(0,2);
        float randomX;
        float randomY;

        //If axis is 0, then the enemy will potentially spawn along all available x values within the used range
        if(axis == 0){
            //Generates a random point off screen, within the boundaries on the entire map

            randomX = Random.Range(-95, 96);
            
            do 
            {
                randomY = Random.Range(1, 3)==1 ? Random.Range(-43, player.transform.position.y-13) : Random.Range(player.transform.position.y+13, 48);
            }
            while (randomY < -43 || randomY > 48);
        }
        //If axis is 1, then the enemy will potentially spawn along all available y values within the used range
        else{
            //Generates a random point off screen, within the boundaries on the entire map
            do 
            {
                randomX = Random.Range(1, 3)==1 ? Random.Range(-95, player.transform.position.x-22) : Random.Range(player.transform.position.x+22, 96);
            }
            while (randomX < -95 || randomX > 95);

            
            randomY = Random.Range(-43, 49);

        }

        //Check if they spawned inside of an object, if so, despawn them
        List<Collider2D> results = new List<Collider2D>();
        enemy.transform.position = new Vector3(randomX, randomY, 0);

        Physics2D.OverlapCircle(enemy.transform.position, 5f, contactFilter, results);
        if(results.Count > 0)
            return false;
        return true;
    }
    public void ResetEnemyStats(GameObject enemy){
        //Enemy stats are reset back to their default values, plus the difficulty bonuses
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemyScript.enemyHealth = enemyScript.baseHealth + Player.systemsComplete;
        enemyScript.enemySpeed = enemyScript.baseSpeed + Player.systemsComplete;
        enemyScript.damageDealt = enemyScript.baseDamage + Player.systemsComplete;
        enemyScript.speedReduced = false;
        enemyScript.damageReduced = false;
        enemyScript.poisoned = false;
        enemyScript.enemySR.color = Color.white;
    }

    public void SpawnHVTs(int num){
        int numSpawned = 0;
        while(numSpawned < num){
            int randNum = Random.Range(0,2);
            GameObject newEnemy;
            if(randNum == 0)
                newEnemy = Instantiate(HVT1, new Vector3(0,0,0),Quaternion.identity);
            else
                newEnemy = Instantiate(HVT2, new Vector3(0,0,0),Quaternion.identity);
            

            //If spawned correctly, increment the count for hvts spawned. Otherwise, destroy the invalid spawned HVT.
            if(HVTPosition(newEnemy)){
                numSpawned++;
                Debug.Log("HVT spawned correctly");
            }
            else{
                Debug.Log("HVT spawned in wall");
                Destroy(newEnemy);
            }
        }
        SpawnEnemies();
    }

}