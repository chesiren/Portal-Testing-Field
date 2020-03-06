using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundButton : MonoBehaviour
{ 
    public bool state = false;
    public GameObject line;
    public GameObject door2;
    public GameObject skeleton;
    public GameObject pointlight;
    public int nb;
      
    public void Up()
    {
        skeleton.transform.Translate(Vector3.up * 1 / 7, Space.World);
        door2.GetComponent<Door2>().Toggle();
        pointlight.SetActive(false);
        foreach (Transform child in line.transform)
        {
            child.gameObject.GetComponent<Line>().Toggle();
        }
        state = false;
    }
    public void Down()
    {
        skeleton.transform.Translate(Vector3.up * -1 / 7, Space.World);
        door2.GetComponent<Door2>().Toggle();
        pointlight.SetActive(true);
        foreach (Transform child in line.transform)
        {
            child.gameObject.GetComponent<Line>().Toggle();
        }
        state = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FPSController" || other.gameObject.GetComponent<PhysicsObject>().type == "cube" && other.gameObject.GetComponent<PhysicsObject>().burnbutton != true)
        {
            nb += 1;
            if (nb == 1)
                Down();
        }
        //Debug.Log(nb + " entrer " + other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "FPSController" || other.gameObject.GetComponent<PhysicsObject>().type == "cube" && other.gameObject.GetComponent<PhysicsObject>().burnbutton != true)
        {
            nb -= 1;
            if (nb == 0)
                Up();
        }
        //Debug.Log(nb + " sortie " + other.gameObject);
    }
    public void Refresh()
    {

    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name != "FPSController" && other.gameObject.GetComponent<PhysicsObject>().type == "cube")
        {
            if (other.gameObject.tag == "Untagged" && other.gameObject.GetComponent<PhysicsObject>().burnbutton == false)
            {
                Debug.Log("BURNING " + other.gameObject);
                other.gameObject.GetComponent<PhysicsObject>().burnbutton = true;
                nb -= 1;
                if (nb == 0)
                    Up();
            }
            else if (other.gameObject.tag == "Interactable")
            {
                Debug.Log("WORKING " + other.gameObject);
            }
        }
            
    }
}
