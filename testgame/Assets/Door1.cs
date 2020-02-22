using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door1 : MonoBehaviour
{
    public bool state = false;
    public GameObject skeletonleft;
    public GameObject skeletonright;
    public GameObject boxdropper;
    private int nb;

    public void Close()
    {
        skeletonleft.transform.Translate(Vector3.right * 1, Space.Self);
        skeletonright.transform.Translate(Vector3.left * 1, Space.Self);
        boxdropper.GetComponent<BoxDropper>().Toggle();
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
    }
}
