using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int enemyHealth;
    public int damageDealt;
    public float enemySpeed;
    SpriteRenderer enemySR;


    // Start is called before the first frame update
    void Start()
    {
        enemySR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay2D(Collider2D other){
        if(other.gameObject.tag == "Player" && !Player.isImmune){
            other.GetComponent<Player>().damageTaken(damageDealt);
        }
    }

    public void damageTaken(){
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        StartCoroutine(damageEffect());

        IEnumerator damageEffect(){
            yield return new WaitForSeconds(1);
            sr.color = Color.white;
        
    }
    }
    // public void MoveCreature(Vector3 direction)
    // {

    //     if (movementType == CreatureMovementType.tf)
    //     {
    //         MoveCreatureTransform(direction);
    //     }
    //     else if (movementType == CreatureMovementType.physics)
    //     {
    //         MoveCreatureRb(direction);
    //     }

    //     //set animation
    //     if(direction != Vector3.zero){
    //         foreach(AnimationStateChanger asc in animationStateChangers){
    //             asc.ChangeAnimationState("Walk",speed);
    //         }
    //     }else{
    //         foreach(AnimationStateChanger asc in animationStateChangers){
    //             asc.ChangeAnimationState("Idle");
    //         }
    //     }
    // }

    public void MoveToward(Vector3 target){
        Vector3 direction = target - transform.position;
        MoveCreatureTransform(direction.normalized);
    }

    public void Stop(){
        MoveCreatureTransform(Vector3.zero);
    }

    // public void MoveCreatureRb(Vector3 direction)
    // {
    //     Vector3 currentVelocity = Vector3.zero;
    //     if(perspectiveType == CreaturePerspective.sideScroll){
    //         currentVelocity = new Vector3(0, rb.velocity.y, 0);
    //     }

    //     rb.velocity = (currentVelocity) + (direction * speed);
    //     if(rb.velocity.x < 0){
    //         body.transform.localScale = new Vector3(-1,1,1);
    //     }else if(rb.velocity.x > 0){
    //         body.transform.localScale = new Vector3(1,1,1);
    //     }
    //     //rb.AddForce(direction * speed);
    //     //rb.MovePosition(transform.position + (direction * speed * Time.deltaTime))
    // }

    public void MoveCreatureTransform(Vector3 direction)
    {
        if(direction.x > 0){
            enemySR.flipX = true;
        }
        else if(direction.x < 0){
            enemySR.flipX = false;
        }

        transform.position += direction * Time.deltaTime * enemySpeed;
    }
}
