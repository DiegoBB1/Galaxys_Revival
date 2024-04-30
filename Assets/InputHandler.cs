using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{

    [SerializeField] Player playerCharacter;
    private bool onCooldown = false;
    public float timer = 0.0f;
    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.Q)){
            SceneManager.LoadScene("ShipHub");
        }
        Vector3 input = Vector3.zero;

        //Following 4 if statements handle player movement.
        if(Input.GetKey(KeyCode.A)){
            input.x = -1;
        }
        if(Input.GetKey(KeyCode.D)){
            input.x = 1;
        }
        if(Input.GetKey(KeyCode.W)){
            input.y = 1;
        }
        if(Input.GetKey(KeyCode.S)){
            input.y = -1;
        }

        //If the weapon is not on cooldown, player is able to fire in one of the four directions. There are two general branches to handle the default weapon and charged weapon.
        //Default weapon branch also considers if the player has the multi shot upgrade.
        if(!onCooldown){

            if(Player.weaponEquipped == "Charge Shot"){
                //3 tiers of charge, depending on how long the player holds down the fire key.

                //Once player pushes key down, a timer starts, which is checked upon release to determine the charge level.
                if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
                    timer += Time.deltaTime;
                
                if(Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)){
                    if(timer < 1.0f)
                        Player.chargeLevel = 1;
                    else if(timer < 2.0f)
                        Player.chargeLevel = 2;
                    else
                        Player.chargeLevel = 3;

                    timer = 0.0f;

                    if(Input.GetKeyUp(KeyCode.UpArrow))
                        playerCharacter.ShootProjectile(new Vector3(0, 0, 90), new Vector3(0, 100, 0));
                    else if(Input.GetKeyUp(KeyCode.DownArrow))
                        playerCharacter.ShootProjectile(new Vector3(0, 0, -90), new Vector3(0, -100, 0));                  
                    else if(Input.GetKeyUp(KeyCode.LeftArrow))
                        playerCharacter.ShootProjectile(new Vector3(0, 0, 180), new Vector3(-100, 0, 0));                 
                    else if(Input.GetKeyUp(KeyCode.RightArrow))
                        playerCharacter.ShootProjectile(new Vector3(0, 0, 0), new Vector3(100, 0, 0));

                    FireCooldown();
                    
                }
            }
            else{
                if(Input.GetKey(KeyCode.UpArrow)){

                    if(Player.weaponEquipped == "Multi Shot"){
                        playerCharacter.ShootProjectile(new Vector3(0, 0, 105), new Vector3(-50, 100, 0));
                        playerCharacter.ShootProjectile(new Vector3(0, 0, 75), new Vector3(50, 100, 0));
                    }
                    playerCharacter.ShootProjectile(new Vector3(0, 0, 90), new Vector3(0, 100, 0));
                    FireCooldown();
                }
                else if(Input.GetKey(KeyCode.DownArrow)){
                    if(Player.weaponEquipped == "Multi Shot"){
                        playerCharacter.ShootProjectile(new Vector3(0, 0, -105), new Vector3(-50, -100, 0));
                        playerCharacter.ShootProjectile(new Vector3(0, 0, -75), new Vector3(50, -100, 0));
                    }
                    playerCharacter.ShootProjectile(new Vector3(0, 0, -90), new Vector3(0, -100, 0));
                    FireCooldown();
                }
                else if(Input.GetKey(KeyCode.LeftArrow)){
                    if(Player.weaponEquipped == "Multi Shot"){
                        playerCharacter.ShootProjectile(new Vector3(0, 0, 195), new Vector3(-100, -50, 0));
                        playerCharacter.ShootProjectile(new Vector3(0, 0, 165), new Vector3(-100, 50, 0));
                    }
                    playerCharacter.ShootProjectile(new Vector3(0, 0, 180), new Vector3(-100, 0, 0));
                    FireCooldown();        
                }
                else if(Input.GetKey(KeyCode.RightArrow)){
                    if(Player.weaponEquipped == "Multi Shot"){
                        playerCharacter.ShootProjectile(new Vector3(0, 0, -15), new Vector3(100, -50, 0));
                        playerCharacter.ShootProjectile(new Vector3(0, 0, 15), new Vector3(100, 50, 0));
                    }
                    playerCharacter.ShootProjectile(new Vector3(0, 0, 0), new Vector3(100, 0, 0));
                    FireCooldown();
                }
            }
        }
        playerCharacter.MovePlayer(input);

        if(Input.GetKeyDown(KeyCode.Space))
            playerCharacter.UseAbility();
        
    }

    public void FireCooldown(){
        if(onCooldown){
            return;
        }

        onCooldown = true;

        //wait for cooldown seconds until we can shoot again
        StartCoroutine(ShootCooldownRoutine());
        IEnumerator ShootCooldownRoutine(){
                yield return new WaitForSeconds(Player.projectileCooldown);
                onCooldown = false;
        }
    }
}
