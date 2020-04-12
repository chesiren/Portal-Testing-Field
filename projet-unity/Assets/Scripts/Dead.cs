using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : MonoBehaviour
{
    public GameObject child;

    public Material material;
    Renderer rend;
    
    void Start()
    {
        rend = child.GetComponent<Renderer>();
    }

    public void Burn()
    {
        rend.enabled = true;
        rend.sharedMaterial = material;
        //Debug.Log("burn");
    }
}
