using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour, IInteractable
{
    public bool m_Held = false;
    public bool m_Cleanser = false;

    private Rigidbody m_ThisRigidbody = null;
    private FixedJoint m_HoldJoint = null;
    public GameObject playercontroller;
    private Weapon playerwep;

    public float counter = -1f;


    private void Start()
    {
        playerwep = playercontroller.GetComponent<Weapon>();
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
    public void Interact(Weapon playerScript)
    {
        // Is the object currently being held?
        if (m_Held)
        {
            playerwep.PlayAnim("v_portalgun.qc_skeleton|release");
            Drop();
        }
        else
        {
            playerwep.PlayAnim("v_portalgun.qc_skeleton|pickup");
            playerwep.holding = true;
            m_Held = true;
            m_ThisRigidbody.useGravity = false;

            m_HoldJoint = playerScript.m_HandTransform.gameObject.AddComponent<FixedJoint>();
            //m_HoldJoint.breakForce = 10000f; // Play with this value
            m_HoldJoint.breakForce = 5000f;
            m_HoldJoint.connectedBody = m_ThisRigidbody;
        }
    }

    // Throw the object
    public void Action(Weapon playerScript)
    {
        // Is the object currently being held?
        if (m_Held)
        {
            playerwep.PlayAnim("v_portalgun.qc_skeleton|release");
            Drop();

            // Force the object away in the opposite direction of the player
            Vector3 forceDir = transform.position - playerScript.m_HandTransform.position;
            m_ThisRigidbody.AddForce(forceDir * playerScript.m_ThrowForce);
        }
    }

    // Drop the object
    private void Drop()
    {
        playerwep.holding = false;

        m_Held = false;
        m_ThisRigidbody.useGravity = true;

        Destroy(m_HoldJoint);
    }
}