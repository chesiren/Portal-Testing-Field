using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private bool state = false;
    public Material[] material;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];
    }

    public void Toggle()
    {
        if (state) { 
            rend.sharedMaterial = material[0];
            state = false;
        }else { 
            rend.sharedMaterial = material[1];
            state = true;
        }
    }
}
