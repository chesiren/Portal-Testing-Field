using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // This script launches a projectile prefab by instantiating it at the position
    // of the GameObject on which it is placed, then then setting the velocity
    // in the forward direction of the same GameObject.
    public GameObject dupe1;
    public GameObject dupe2;
    public GameObject portal1;
    public GameObject portal2;

    private void Start()
    {
        portal1.GetComponent<SeamlessTeleport>().receiver = portal2;
        portal1.GetComponent<SeamlessTeleport>().player = gameObject;
        //dupe1 = portal1;

        portal2.GetComponent<SeamlessTeleport>().receiver = portal1;
        portal2.GetComponent<SeamlessTeleport>().player = gameObject;
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
                
                GameObject a = Instantiate(portal1, hit.point, Quaternion.identity);
                a.transform.position = hit.point;
                a.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                //a.transform.up = hit.normal;
                a.GetComponent<SeamlessTeleport>().player = gameObject;
                a.GetComponent<Portal>().pairPortal = dupe2.transform;
                Destroy(dupe1);
                dupe1 = a.gameObject;

                dupe2.GetComponent<SeamlessTeleport>().receiver = a;
                a.GetComponent<SeamlessTeleport>().receiver = dupe2;

            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (Physics.Raycast(myRay, out hit))
            {
                GameObject b = Instantiate(portal2, hit.point, Quaternion.identity);
                b.transform.position = hit.point;
                b.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                //b.transform.up = hit.normal;
                b.GetComponent<SeamlessTeleport>().player = gameObject;
                b.GetComponent<Portal>().pairPortal = dupe1.transform;
                Destroy(dupe2);
                dupe2 = b.gameObject;

                dupe1.GetComponent<SeamlessTeleport>().receiver = b;
                b.GetComponent<SeamlessTeleport>().receiver = dupe1;
            }
        }
    }
}


/*using System.Collections;
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
        portal1.GetComponent<Teleport>().receiver = portal2.transform;
        portal1.GetComponent<Teleport>().player = gameObject;
        //dupe1 = portal1;

        portal2.GetComponent<Teleport>().receiver = portal1.transform;
        portal2.GetComponent<Teleport>().player = gameObject;
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
                
                GameObject a = Instantiate(portal1, hit.point, Quaternion.identity);
                a.transform.position = hit.point;
                a.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                //a.transform.up = hit.normal;
                a.GetComponent<Teleport>().player = gameObject;
                a.GetComponent<Portal>().pairPortal = dupe2.transform;
                Destroy(dupe1);
                dupe1 = a.gameObject;

                dupe2.GetComponent<Teleport>().receiver = a.transform;
                a.GetComponent<Teleport>().receiver = dupe2.transform;

            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (Physics.Raycast(myRay, out hit))
            {
                GameObject b = Instantiate(portal2, hit.point, Quaternion.identity);
                b.transform.position = hit.point;
                b.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                //b.transform.up = hit.normal;
                b.GetComponent<Teleport>().player = gameObject;
                b.GetComponent<Portal>().pairPortal = dupe1.transform;
                Destroy(dupe2);
                dupe2 = b.gameObject;

                dupe1.GetComponent<Teleport>().receiver = b.transform;
                b.GetComponent<Teleport>().receiver = dupe1.transform;
            }
        }
    }
}
*/