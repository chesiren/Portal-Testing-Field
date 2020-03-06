using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : MonoBehaviour
{
    public GameObject child;

    public Material[] material;
    Renderer rend;
    
    void Start()
    {
        rend = child.GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];
    }

    public void Burn()
    { 
        rend.sharedMaterial = material[1];
        //Debug.Log("burn");
    }
}
