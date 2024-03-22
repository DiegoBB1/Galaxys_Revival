using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    [SerializeField] Player playerCharacter;
    private bool onCooldown = false;
    // Update is called once per frame
    void Update()
    {
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

        //Following 4 if statements handle player projectile firing. If projectile is not on cooldown, player is able to fire in one of 4 directions.
        if(!onCooldown){
            if(Input.GetKey(KeyCode.UpArrow)){
                playerCharacter.shootProjectile(new Vector3(0, 0, 90), new Vector3(0, 100, 0));
                fireCooldown();
            }
            else if(Input.GetKey(KeyCode.DownArrow)){
                playerCharacter.shootProjectile(new Vector3(0, 0, -90), new Vector3(0, -100, 0));
                fireCooldown();
            }
            else if(Input.GetKey(KeyCode.LeftArrow)){
                playerCharacter.shootProjectile(new Vector3(0, 0, 180), new Vector3(-100, 0, 0));
                fireCooldown();        
            }
            else if(Input.GetKey(KeyCode.RightArrow)){
                playerCharacter.shootProjectile(new Vector3(0, 0, 0), new Vector3(100, 0, 0));
                fireCooldown();
            }
        }
        playerCharacter.movePlayer(input);
    }

    public void fireCooldown(){
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
