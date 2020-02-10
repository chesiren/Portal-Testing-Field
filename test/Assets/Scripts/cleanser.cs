using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cleanser : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Destroy(other.gameObject.GetComponent<Weapon>().dupe1);
            other.gameObject.GetComponent<Weapon>().dupe1.SetActive(false);
            //Destroy(other.gameObject.GetComponent<Weapon>().dupe2);
            other.gameObject.GetComponent<Weapon>().dupe2.SetActive(false);
            Debug.Log(other.gameObject + " a touché le cleanser");
        }
        if (other.CompareTag("Interactable"))
        {
            other.gameObject.GetComponent<PhysicsObject>().m_Cleanser = true;
            other.gameObject.GetComponent<PhysicsObject>().counter = 2f;
            other.gameObject.GetComponent<dead>().burn();
            Debug.Log(other.gameObject + " a touché le cleanser");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //playerOverlapping = false;
        }
    }
}
