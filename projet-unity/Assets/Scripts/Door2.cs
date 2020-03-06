using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door2 : MonoBehaviour
{
    public bool state = false;
    public GameObject skeletonleft;
    public GameObject skeletonright;

    public void Close()
    {
        skeletonleft.transform.Translate(Vector3.right * -1, Space.Self);
        skeletonright.transform.Translate(Vector3.left * -1, Space.Self);
    }
    public void Open()
    {
        skeletonleft.transform.Translate(Vector3.right * 1, Space.Self);
        skeletonright.transform.Translate(Vector3.left * 1, Space.Self);
    }

    public void Toggle()
    {
        if (state)
        {
            Open();
            state = false;
        }
        else
        {
            Close();
            state = true;
        }
    }
}
