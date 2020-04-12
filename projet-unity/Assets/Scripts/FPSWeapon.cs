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
    }

    public static Bounds GetBounds(GameObject obj)
    {
        Bounds bounds = new Bounds();
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        if (renderers.Length > 0)
        {
            //Find first enabled renderer to start encapsulate from it
            foreach (Renderer renderer in renderers)
            {
                if (renderer.enabled)
                {
                    bounds = renderer.bounds;
                    break;
                }
            }

            //Encapsulate for all renderers
            foreach (Renderer renderer in renderers)
            {
                if (renderer.enabled)
                {
                    bounds.Encapsulate(renderer.bounds);
                }
            }
        }
        return bounds;
    }

    void Update()
    {
        Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition); ;
        RaycastHit hit;

        // Has interact button been pressed whilst interactable object is in front of player?
        if (Input.GetKeyDown("a") && m_CanInteract == true)
        {

            Physics.Raycast(myRay, out hit);
            if (hit.collider.gameObject.GetComponent<Switch>()) //switch
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
                    Debug.Log("whiteray center hit: " + hit.collider.transform.tag);
                    if (hit.collider.transform.CompareTag("Moonrock"))
                    {
                        anim.Play("v_portalgun.qc_skeleton|fire1");

                        dupe1.GetComponent<Portal>().box_direction = -hit.normal * 10;
                        dupe1.transform.position = hit.point - myRay.direction.normalized * 0.02f;
                        dupe1.transform.rotation = Quaternion.LookRotation(-hit.normal);

                        Portal _dupe1 = dupe1.GetComponent<Portal>();

                        Vector3 topleft = _dupe1.topleft.transform.position;
                        Vector3 topright = _dupe1.topright.transform.position;
                        Vector3 bottomleft = _dupe1.bottomleft.transform.position;
                        Vector3 bottomright = _dupe1.bottomright.transform.position;

                        Vector3 porigin = hit.point - myRay.direction.normalized * 0.05f;

                        Debug.DrawRay(myRay.origin, myRay.direction * hit.distance, Color.white, 10000);

                        bool bcast1 = Physics.Linecast(porigin, topleft, out RaycastHit bhit1, layerMask);
                        if (bcast1) { 
                            Debug.Log("bhit1 " + bhit1.collider.tag + bhit1.collider.name);
                            dupe1.transform.Translate(bhit1.point - topleft);
                        }
                        Debug.DrawLine(porigin, topleft, Color.blue, 10000);

                        bool bcast2 = Physics.Linecast(porigin, topright, out RaycastHit bhit2, layerMask);
                        if (bcast2)
                        {
                            Debug.Log("bhit2 " + bhit2.collider.tag + bhit2.collider.name);
                            dupe1.transform.Translate(bhit2.point - topright);
                        }
                        Debug.DrawLine(porigin, topright, Color.red, 10000);

                        bool bcast3 = Physics.Linecast(porigin, bottomleft, out RaycastHit bhit3, layerMask);
                        if (bcast3)
                        {
                            Debug.Log("bhit3 " + bhit3.collider.tag + bhit3.collider.name);
                            dupe1.transform.Translate(bhit3.point - bottomleft);
                        }
                        Debug.DrawLine(porigin, bottomleft, Color.yellow, 10000);

                        bool bcast4 = Physics.Linecast(porigin, bottomright, out RaycastHit bhit4, layerMask);
                        if (bcast4)
                        {
                            Debug.Log("bhit4 " + bhit4.collider.tag + bhit4.collider.name);
                            dupe1.transform.Translate(bhit4.point - bottomright);
                        }
                        Debug.DrawLine(porigin, bottomright, Color.magenta, 10000);
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