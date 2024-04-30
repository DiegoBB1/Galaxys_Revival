using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    public GameObject prisoner;
    public bool cageOpen = false;
    [SerializeField] Sprite openCage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CageOpened(){
        Debug.Log("Cage opened, prisoner freed");
        GetComponent<SpriteRenderer>().sprite = openCage;
        cageOpen = true;
        prisoner.GetComponent<NPC>().freed = true;
    }
}
