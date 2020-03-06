using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSWeapon : MonoBehaviour
{
    public GameObject dupe1;
    public GameObject dupe2;
    public GameObject template;
    public GameObject portalgun;
    public Image CrossHair;

    int layerMask = 1 << 12;
    Animation anim;

    //[SerializeField] private Transform m_CameraTransform = null;
    public Transform m_HandTransform = null;
    public float m_ThrowForce = 200f;

    private RaycastHit m_RaycastFocus;
    private bool m_CanInteract = false;
    public bool holding = false;

    private void Start()
    {
        layerMask = ~layerMask;
        anim = portalgun.GetComponent<Animation>();
        anim.Play("v_portalgun.qc_skeleton|draw");

        //m_CameraTransform = GetComponentInChildren<Camera>().transform;

        //var _portal1 = portal1.GetComponent<Portalold>();
        //var _portal2 = portal2.GetComponent<Portalold>();

        //_portal1.plane.GetComponent<SeamlessTeleport>().player = gameObject;

        //_portal2.plane.GetComponent<SeamlessTeleport>().player = gameObject;
    }

    void Update()
    {
        Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition); ;
        RaycastHit hit;

        // Has interact button been pressed whilst interactable object is in front of player?
        if (Input.GetKeyDown("a") && m_CanInteract == true)
        {

            Physics.Raycast(myRay, out hit);
            if (hit.collider.gameObject.name == "switch_prefab")
            {
                hit.collider.gameObject.GetComponent<Switch>().ButtonPressed();
            }
            else
            {
                IFPSInteractable interactComponent = m_RaycastFocus.collider.transform.GetComponent<IFPSInteractable>();
                if (interactComponent != null)
                {
                    interactComponent.Interact(this);
                }
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (holding)
            {
                // throw object
                if (m_CanInteract == true)
                {
                    IFPSInteractable interactComponent = m_RaycastFocus.collider.transform.GetComponent<IFPSInteractable>();
                    if (interactComponent != null)
                    {
                        // Perform object's action
                        interactComponent.Action(this);
                    }
                }
            }
            else
            {
                // fire portal 1
                if (Physics.Raycast(myRay, out hit, Mathf.Infinity, layerMask))
                {
                    Debug.Log(hit.collider.transform.tag);
                    if (hit.collider.transform.CompareTag("Moonrock"))
                    {
                        anim.Play("v_portalgun.qc_skeleton|fire1");

                        dupe1.GetComponent<Portal>().box_direction = -hit.normal * 10;
                        dupe1.transform.position = hit.point - myRay.direction.normalized * 0.01f;
                        dupe1.transform.rotation = Quaternion.LookRotation(-hit.normal);
                        Debug.Log(hit.point);
                        Debug.Log(dupe1.transform.position);
                    }
                    else
                    {
                        anim.Play("v_portalgun.qc_skeleton|fizzle");
                    }
                }
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (holding)
            {
                // throw object
                if (m_CanInteract == true)
                {
                    IFPSInteractable interactComponent = m_RaycastFocus.collider.transform.GetComponent<IFPSInteractable>();
                    if (interactComponent != null)
                    {
                        // Perform object's action
                        interactComponent.Action(this);
                    }
                }
            }
            else
            {
                // fire portal 2
                if (Physics.Raycast(myRay, out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.collider.transform.CompareTag("Moonrock"))
                    {
                        anim.Play("v_portalgun.qc_skeleton|fire1");

                        dupe2.GetComponent<Portal>().box_direction = -hit.normal * 10;
                        dupe2.transform.position = hit.point - myRay.direction.normalized * 0.1f;
                        dupe2.transform.rotation = Quaternion.LookRotation(hit.normal);
                    }
                    else
                    {
                        anim.Play("v_portalgun.qc_skeleton|fizzle");
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        //Ray ray = new Ray(m_CameraTransform.position, m_CameraTransform.forward);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raybool = Physics.Raycast(ray, out m_RaycastFocus, 3);
        // Is interactable object detected in front of player?
        if ( raybool && m_RaycastFocus.collider.transform.CompareTag("Interactable"))
            m_CanInteract = true;
        else
            m_CanInteract = false;

        // crosshair colors
        if (raybool && m_RaycastFocus.collider.transform.CompareTag("Interactable"))
            CrossHair.color = Color.green;
        else if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask) && hit.collider.transform.tag != "Moonrock")
            CrossHair.color = Color.red;
        else
            CrossHair.color = Color.white;
    }

    public void PlayAnim(string animation)
    {
        anim.Play(animation);
    }
}