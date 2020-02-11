using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cleanser : MonoBehaviour
{
    private Weapon playerwep;

    void OnTriggerEnter(Collider other)
    {
        playerwep = other.gameObject.GetComponent<Weapon>();
        if (other.CompareTag("Player"))
        {
            //Destroy(other.gameObject.GetComponent<Weapon>().dupe1);
            playerwep.dupe1.SetActive(false);
            playerwep.dupe1.GetComponent<Portal>().pairPortal = null;
            playerwep.dupe1.GetComponent<Portal>().plane.GetComponent<SeamlessTeleport>().receiver = null;
            //Destroy(other.gameObject.GetComponent<Weapon>().dupe2);
            playerwep.dupe2.SetActive(false);
            playerwep.dupe2.GetComponent<Portal>().pairPortal = null;
            playerwep.dupe2.GetComponent<Portal>().plane.GetComponent<SeamlessTeleport>().receiver = null;
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
