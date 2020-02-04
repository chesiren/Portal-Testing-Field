using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject thePlayer;

    private void OnTriggerEnter(Collider other)
    {
        //Vector3 myVector = new Vector3(0, 0, 1);
        //spawnedTrans.position = playerTrans.position + (playerTrans.forward * someDistanceFloat);
        //otherObject.transform.position + otherObject.transform.forward * 10
        thePlayer.transform.rotation = teleportTarget.transform.rotation;
        thePlayer.transform.position = teleportTarget.transform.position + teleportTarget.transform.forward;
    }
}
