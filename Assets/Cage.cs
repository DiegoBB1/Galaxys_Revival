using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    public GameObject prisoner;
    public bool cageOpen = false;
    [SerializeField] Sprite openCage;

    public void CageOpened(){
        Debug.Log("Cage opened, prisoner freed");
        GetComponent<SpriteRenderer>().sprite = openCage;
        cageOpen = true;
        prisoner.GetComponent<NPC>().freed = true;
    }
}
