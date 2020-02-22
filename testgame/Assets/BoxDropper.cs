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
    public GameObject previousobject;
    public GameObject player;
    private bool firstdrop = true;

    void Start()
    {
        anim = GetComponent<Animation>();
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
        }
    }
    public void Open()
    {
        if (working==false)
        {
            working = true;
            anim.Play("box_dropper_cover.qc_skeleton|open");
            GetComponent<Collider>().enabled = false;
            
            if (firstdrop == false)
            {
                previousobject.GetComponent<PhysicsObject>().m_Cleanser = true;
                previousobject.GetComponent<PhysicsObject>().counter = 2f;
                previousobject.GetComponent<Dead>().Burn();

                GameObject a = Instantiate(objectprefab, transform.position, Quaternion.identity);
                a.GetComponent<PhysicsObject>().playercontroller = player;
                previousobject = a.gameObject;
            } else
            {
                firstdrop = false;
            }

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
