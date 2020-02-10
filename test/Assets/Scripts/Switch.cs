using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public AudioClip son;

    private void OnMouseDown()
    {
        AudioSource.PlayClipAtPoint(son, gameObject.transform.position);
    }
}
