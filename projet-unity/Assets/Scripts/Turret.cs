using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    Animation animA;

    public GameObject child;

    public Material[] material;
    Renderer rend;

    public void Inactive()
    {
        rend.sharedMaterial = material[1];
    }

    public void Active()
    {
        rend.sharedMaterial = material[0];
    }

    void Start()
    {
        rend = child.GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];

        animA = gameObject.GetComponent<Animation>();
        animA.Play("turret_01.qc_skeleton|idle");
    }
}
