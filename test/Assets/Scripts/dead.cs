using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dead : MonoBehaviour
{
    public GameObject child;

    public Material[] material;
    Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = child.GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];
    }

    public void burn()
    { 
        rend.sharedMaterial = material[1];
        Debug.Log("burn");
    }
}
