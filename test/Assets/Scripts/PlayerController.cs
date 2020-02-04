using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Transform m_CameraTransform = null;
    public Transform m_HandTransform = null;
    //[SerializeField] private Image m_CursorImage = null;
    public float m_ThrowForce = 200f;

    private RaycastHit m_RaycastFocus;
    private bool m_CanInteract = false;


    private void Start()
    {
        m_CameraTransform = GetComponentInChildren<Camera>().transform;
    }

    private void Update()
    {
        // Has interact button been pressed whilst interactable object is in front of player?
        if (Input.GetKeyDown("a") && m_CanInteract == true)
        {
            IInteractable interactComponent = m_RaycastFocus.collider.transform.GetComponent<IInteractable>();

            if (interactComponent != null)
            {
                // Perform object's interaction
                interactComponent.Interact(this);
            }
        }

        // Has action button been pressed whilst interactable object is in front of player?
        if (Input.GetButtonDown("Fire2") && m_CanInteract == true)
        {
            IInteractable interactComponent = m_RaycastFocus.collider.transform.GetComponent<IInteractable>();

            if (interactComponent != null)
            {
                // Perform object's action
                interactComponent.Action(this);
            }
        }
    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(m_CameraTransform.position, m_CameraTransform.forward);

        // Is interactable object detected in front of player?
        if (Physics.Raycast(ray, out m_RaycastFocus, 3) && m_RaycastFocus.collider.transform.tag == "Interactable")
        {
            //m_CursorImage.color = Color.green;
            m_CanInteract = true;
        }
        else
        {
            //m_CursorImage.color = Color.white;
            m_CanInteract = false;
        }
    }
}