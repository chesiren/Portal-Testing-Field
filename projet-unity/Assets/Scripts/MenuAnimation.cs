using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuAnimation : MonoBehaviour
{
    [SerializeField]
    public GameObject bouton1;
    public GameObject bouton2;
    public GameObject bouton3;
    public GameObject bouton4;

    private void Start()
    {
        bouton1.SetActive(false);
        bouton2.SetActive(false);
        bouton3.SetActive(false);
        bouton4.SetActive(false);

        StartCoroutine(Temps1());
        StartCoroutine(Temps2());
        StartCoroutine(Temps3());
        StartCoroutine(Temps4());
    }

    IEnumerator Temps1()
    {
        yield return new WaitForSeconds(19f);
        bouton1.SetActive(true);
    }

    IEnumerator Temps2()
    {
        yield return new WaitForSeconds(21f);
        bouton2.SetActive(true);
    }
    IEnumerator Temps3()
    {
        yield return new WaitForSeconds(23f);
        bouton3.SetActive(true);
    }
    IEnumerator Temps4()
    {
        yield return new WaitForSeconds(25f);
        bouton4.SetActive(true);
    }
}
