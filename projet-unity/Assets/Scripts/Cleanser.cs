using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleanser : MonoBehaviour
{
    private FPSWeapon playerwep;

    void OnTriggerEnter(Collider other)
    {
        playerwep = other.gameObject.GetComponent<FPSWeapon>();
        if (other.CompareTag("Player"))
        {
            //Destroy(other.gameObject.GetComponent<Weapon>().dupe1);
            playerwep.dupe1.SetActive(false);
            playerwep.dupe1.GetComponent<Portalold>().pairPortal = null;
            playerwep.dupe1.GetComponent<Portalold>().plane.GetComponent<SeamlessTeleport>().receiver = null;
            //Destroy(other.gameObject.GetComponent<Weapon>().dupe2);
            playerwep.dupe2.SetActive(false);
            playerwep.dupe2.GetComponent<Portalold>().pairPortal = null;
            playerwep.dupe2.GetComponent<Portalold>().plane.GetComponent<SeamlessTeleport>().receiver = null;
            //Debug.Log(other.gameObject + " a touché le cleanser");
        }
        if (other.CompareTag("Interactable"))
        {
            other.gameObject.GetComponent<PhysicsObject>().m_Cleanser = true;
            other.gameObject.GetComponent<PhysicsObject>().counter = 2f;
            other.gameObject.GetComponent<Dead>().Burn();
            //Debug.Log(other.gameObject + " a touché le cleanser");
        }
    }
}
