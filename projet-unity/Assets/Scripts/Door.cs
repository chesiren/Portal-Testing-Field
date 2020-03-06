using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool state = false;
    public GameObject skeletonleft;
    public GameObject skeletonright;
    private int nb;

    public void Close()
    {
        skeletonleft.transform.Translate(Vector3.right * 1, Space.Self);
        skeletonright.transform.Translate(Vector3.left * 1, Space.Self);
    }
    public void Open()
    {
        skeletonleft.transform.Translate(Vector3.right * -1, Space.Self);
        skeletonright.transform.Translate(Vector3.left * -1, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            nb += 1;
            state = true;
            if (nb == 1)
                Open();
        }
        //Debug.Log(nb + " entrer " + other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            nb -= 1;
            state = false;
            if (nb == 0)
                Close();
        }
        //Debug.Log(nb + " sortie " + other.gameObject);
    }
}
