using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    [SerializeField] Player playerCharacter;
    // Update is called once per frame
    void Update()
    {
        Vector3 input = Vector3.zero;

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
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            playerCharacter.shootProjectile(new Vector3(0, 0, 90), new Vector3(0, 100, 0));
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            playerCharacter.shootProjectile(new Vector3(0, 0, -90), new Vector3(0, -100, 0));
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow)){
            playerCharacter.shootProjectile(new Vector3(0, 0, 180), new Vector3(-100, 0, 0));
        }
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            playerCharacter.shootProjectile(new Vector3(0, 0, 0), new Vector3(100, 0, 0));
        }
        playerCharacter.movePlayer(input);
    }
}
