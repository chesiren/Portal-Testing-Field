using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject thePlayer;

    private void OnTriggerEnter(Collider other)
    {
        Vector3 myVector = new Vector3(0,0,-1);
        thePlayer.transform.position = teleportTarget.transform.position + myVector;
       // thePlayer.transform.localRotation = teleportTarget.transform.localRotation;
        //thePlayer.transform.rotation = teleportTarget.transform.rotation;
        thePlayer.transform.eulerAngles = teleportTarget.transform.eulerAngles;

    }
}
