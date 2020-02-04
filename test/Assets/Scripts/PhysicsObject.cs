using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour, IInteractable
{
    public bool m_Held = false;

    private Rigidbody m_ThisRigidbody = null;
    private FixedJoint m_HoldJoint = null;


    private void Start()
    {
        gameObject.tag = "Interactable";
        m_ThisRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // If the holding joint has broken, drop the object
        if (m_HoldJoint == null && m_Held == true)
        {
            m_Held = false;
            m_ThisRigidbody.useGravity = true;
        }
    }

    // Pick up the object, or drop it if it is already being held
    public void Interact(PlayerController playerScript)
    {
        // Is the object currently being held?
        if (m_Held)
        {
            Drop();
        }
        else
        {
            m_Held = true;
            m_ThisRigidbody.useGravity = false;

            m_HoldJoint = playerScript.m_HandTransform.gameObject.AddComponent<FixedJoint>();
            m_HoldJoint.breakForce = 10000f; // Play with this value
            m_HoldJoint.connectedBody = m_ThisRigidbody;
        }
    }

    // Throw the object
    public void Action(PlayerController playerScript)
    {
        // Is the object currently being held?
        if (m_Held)
        {
            Drop();

            // Force the object away in the opposite direction of the player
            Vector3 forceDir = transform.position - playerScript.m_HandTransform.position;
            m_ThisRigidbody.AddForce(forceDir * playerScript.m_ThrowForce);
        }
    }

    // Drop the object
    private void Drop()
    {
        m_Held = false;
        m_ThisRigidbody.useGravity = true;

        Destroy(m_HoldJoint);
    }
}