using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundButton : MonoBehaviour
{ 
    public bool state = false;
    public GameObject skeleton;
    private int nb;

    public void Up()
    {
        skeleton.transform.Translate(Vector3.up * 1 / 7, Space.World);
    }
    public void Down()
    {
        skeleton.transform.Translate(Vector3.up * -1 / 7, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FPSController" || other.gameObject.name == "companion_cube" || other.gameObject.name == "cube_heart")
        {
            nb += 1;
            state = true;
            if (nb == 1)
                Down();
        }
        //Debug.Log(nb + " entrer " + other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "FPSController" || other.gameObject.name == "companion_cube" || other.gameObject.name == "cube_heart")
        {
            nb -= 1;
            state = false;
            if (nb == 0)
                Up();
        }
        //Debug.Log(nb + " sortie " + other.gameObject);
    }
}
