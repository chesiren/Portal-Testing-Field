using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public AudioClip son;
    Animation animations;
    public bool state = true;

    void Start()
    {
        animations = gameObject.GetComponent<Animation>();
    }

    public void ButtonPressed()
    {
        ButtonDown();
        ButtonUp();
    }

    public void ButtonDown()
    {
        if (state) // faire l'animation si il est relaché
        {
            animations.Play("switch001.qc_skeleton|down");
        }
        AudioSource.PlayClipAtPoint(son, gameObject.transform.position);
        state = false;
    }

    public void ButtonUp()
    {
        if (state == false) // faire l'animation si il est enfoncé
        {
            animations.Play("switch001.qc_skeleton|up");
        }
        state = true;
    }
}
