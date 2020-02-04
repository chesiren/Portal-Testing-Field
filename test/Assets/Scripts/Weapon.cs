using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject dupe1;
    public GameObject dupe2;
    public GameObject portal1;
    public GameObject portal2;
    
    private void Start()
    {
        var _portal1 = portal1.GetComponent<Portal>();
        var _portal2 = portal2.GetComponent<Portal>();
        

        _portal1.plane.GetComponent<SeamlessTeleport>().receiver = _portal2.plane;
        _portal1.plane.GetComponent<SeamlessTeleport>().player = gameObject;
        //dupe1 = portal1;

        _portal2.plane.GetComponent<SeamlessTeleport>().receiver = _portal1.plane;
        _portal2.plane.GetComponent<SeamlessTeleport>().player = gameObject;
        //dupe2 = portal2;
    }

    void Update()
    {
        Ray myRay;
        RaycastHit hit;
        var _dupe1 = dupe1.GetComponent<Portal>();
        var _dupe2 = dupe2.GetComponent<Portal>();

        myRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetButtonDown("Fire3"))
        {
            if (Physics.Raycast(myRay, out hit))
            {
                
                GameObject a = Instantiate(portal1, hit.point, Quaternion.identity);
                var _a = a.GetComponent<Portal>();
                _a.pairPortal = dupe2.transform;
                _dupe2.pairPortal = a.transform;
                //a.GetComponent<Portal>().pairPortal = dupe2.transform;
                a.transform.position = hit.point;
                a.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                //a.transform.up = hit.normal;
                
                _a.plane.GetComponent<SeamlessTeleport>().player = gameObject;

                _dupe2.plane.GetComponent<SeamlessTeleport>().receiver = _a.plane;
                _a.plane.GetComponent<SeamlessTeleport>().receiver = _dupe2.plane;

                Destroy(dupe1);
                dupe1 = a.gameObject;
            }
        }

        if (Input.GetButtonDown("Fire3"))
        {
            if (Physics.Raycast(myRay, out hit))
            {
                GameObject b = Instantiate(portal2, hit.point, Quaternion.identity);
                var _b = b.GetComponent<Portal>();
                _b.GetComponent<Portal>().pairPortal = dupe1.transform;
                _dupe1.pairPortal = b.transform;
                //b.GetComponent<Portal>().pairPortal = dupe1.transform;
                b.transform.position = hit.point;
                b.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                //b.transform.up = hit.normal;
                _b.plane.GetComponent<SeamlessTeleport>().player = gameObject;

                _dupe1.plane.GetComponent<SeamlessTeleport>().receiver = _b.plane;
                _b.plane.GetComponent<SeamlessTeleport>().receiver = _dupe1.plane;

                Destroy(dupe2);
                dupe2 = b.gameObject;
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