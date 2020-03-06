using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDropper : MonoBehaviour
{
    private bool working = false;
    Animation anim;
    private float counter = -1f;
    private int dontreplay = 0;
    public GameObject objectprefab;
    public GameObject spawnedobject;
    public GameObject previousobject;
    public GameObject player;
    public bool firstdrop = true;

    void Start()
    {
        // init animations
        anim = GetComponent<Animation>();

        // put cube in the dropper
        GameObject start = Instantiate(objectprefab, transform.position + Vector3.up * 3, Quaternion.identity);
        start.GetComponent<PhysicsObject>().playercontroller = player;
        spawnedobject = start.gameObject;
    }
    private void Update()
    {
        if (counter > -1)
        {
            counter -= Time.deltaTime;
            //Debug.Log("timer left" + counter);
            if (counter <= 0)
            {
                Close();
            }
        }
    }
    public void Close()
    {
        if (dontreplay == 1)
        {
            anim.Play("box_dropper_cover.qc_skeleton|close");
            GetComponent<Collider>().enabled = true;
            dontreplay = 0;
            working = false;

            GameObject a = Instantiate(objectprefab, transform.position + Vector3.up * 3, Quaternion.identity);
            a.GetComponent<PhysicsObject>().playercontroller = player;
            spawnedobject = a.gameObject;
        }
    }
    public void Open()
    {
        if (working == false)
        {
            working = true;
            anim.Play("box_dropper_cover.qc_skeleton|open");
            GetComponent<Collider>().enabled = false;
            
            if (previousobject != null) { 
                previousobject.GetComponent<PhysicsObject>().m_Cleanser = true;
                previousobject.GetComponent<PhysicsObject>().counter = 2f;
                previousobject.GetComponent<Dead>().Burn();
            }
            previousobject = spawnedobject;

            dontreplay = 1;
            counter = 1.9f;
        }
        else
        {
            Debug.Log("box dropper indisponible");
        }
    }

    public void Toggle()
    {
        Open();
    }
}
