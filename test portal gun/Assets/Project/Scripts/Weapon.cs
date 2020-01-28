using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // This script launches a projectile prefab by instantiating it at the position
    // of the GameObject on which it is placed, then then setting the velocity
    // in the forward direction of the same GameObject.
    public int width = 10;
    public int height = 4;

    public GameObject dupe1;
    public GameObject dupe2;
    public GameObject portal1;
    public GameObject portal2;
    public GameObject block;

    private void Start()
    {
        portal1.GetComponent<Teleport>().teleportTarget = portal2.transform;
        portal1.GetComponent<Teleport>().thePlayer = gameObject;
        //dupe1 = portal1;

        portal2.GetComponent<Teleport>().teleportTarget = portal1.transform;
        portal2.GetComponent<Teleport>().thePlayer = gameObject;
        //dupe2 = portal2;
    }

    void Update()
    {
        Ray myRay;
        RaycastHit hit;

        myRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(myRay, out hit))
            {
                Destroy(dupe1);
                GameObject a = Instantiate(portal1, hit.point, Quaternion.identity);
                a.transform.position = hit.point;
                a.transform.up = hit.normal;
                a.GetComponent<Teleport>().thePlayer = gameObject;
                dupe1 = a.gameObject;
                
                dupe2.GetComponent<Teleport>().teleportTarget = a.transform;
                a.GetComponent<Teleport>().teleportTarget = dupe2.transform;
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (Physics.Raycast(myRay, out hit))
            {
                Destroy(dupe2);
                GameObject b = Instantiate(portal2, hit.point, Quaternion.identity);
                b.transform.position = hit.point;
                b.transform.up = hit.normal;
                b.GetComponent<Teleport>().thePlayer = gameObject;
                dupe2 = b.gameObject;

                dupe1.GetComponent<Teleport>().teleportTarget = b.transform;
                b.GetComponent<Teleport>().teleportTarget = dupe1.transform;
            }
        }
    }
}
