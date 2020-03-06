using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour, IFPSInteractable
{
    public bool m_Held = false;
    public string type = "decoration";

    public bool m_Cleanser = false;
    public bool burnbutton = false;

    public Rigidbody m_ThisRigidbody = null;
    public FixedJoint m_HoldJoint = null;
    public GameObject playercontroller;
    //public Weapon playerwep;

    public float counter = -1f;


    private void Start()
    {
        //playerwep = GetComponent<Weapon>();
        gameObject.tag = "Interactable";
        m_ThisRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (m_Cleanser == true)
        {
            Drop();
            m_ThisRigidbody.useGravity = false;
            m_Cleanser = false;
            gameObject.tag = "Untagged";
        }
        if (counter > -1)
        {
            counter -= Time.deltaTime;
            //Debug.Log("timer left" + counter);
            if (counter <= 0)
            {
                gameObject.GetComponent<Collider>().enabled = false;
                Destroy(gameObject);
            }
        }

        // If the holding joint has broken, drop the object
        if (m_HoldJoint == null && m_Held == true)
        {
            //m_Held = false;
            //m_ThisRigidbody.useGravity = true;
            //playerwep.holding = false;
            Drop();
        }
    }

    // Pick up the object, or drop it if it is already being held
    public void Interact(FPSWeapon script)
    {
        // Is the object currently being held?
        if (m_Held)
        {
            script.PlayAnim("v_portalgun.qc_skeleton|release");
            Drop();
        }
        else
        {
            script.PlayAnim("v_portalgun.qc_skeleton|pickup");
            script.holding = true;
            m_Held = true;
            m_ThisRigidbody.useGravity = false;

            m_HoldJoint = script.m_HandTransform.gameObject.AddComponent<FixedJoint>();
            //m_HoldJoint.breakForce = 10000f; // Play with this value
            m_HoldJoint.breakForce = 5000f;
            m_HoldJoint.connectedBody = m_ThisRigidbody;
        }
    }

    // Throw the object
    public void Action(FPSWeapon script)
    {
        // Is the object currently being held?
        if (m_Held)
        {
            script.PlayAnim("v_portalgun.qc_skeleton|release");
            Drop();

            // Force the object away in the opposite direction of the player
            Vector3 forceDir = transform.position - script.m_HandTransform.position;
            m_ThisRigidbody.AddForce(forceDir * script.m_ThrowForce);
        }
    }

    // Drop the object
    public void Drop()
    {
        playercontroller.GetComponent<FPSWeapon>().holding = false;

        m_Held = false;
        m_ThisRigidbody.useGravity = true;

        Destroy(m_HoldJoint);
    }
}