/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject dupe1;
    public GameObject dupe2;
    public GameObject portal1;
    public GameObject portal2;
    public GameObject switch_prefab;
    public GameObject portalgun;

    public int debug = 2;

    Animation anim;

    [SerializeField] private Transform m_CameraTransform = null;
    public Transform m_HandTransform = null;
    //[SerializeField] private Image m_CursorImage = null;
    public float m_ThrowForce = 200f;

    private RaycastHit m_RaycastFocus;
    private bool m_CanInteract = false;
    public bool holding = false;

    private void Start()
    {
        anim = portalgun.GetComponent<Animation>();
        anim.Play("v_portalgun.qc_skeleton|draw");

        m_CameraTransform = GetComponentInChildren<Camera>().transform;

        var _portal1 = portal1.GetComponent<Portalold>();
        var _portal2 = portal2.GetComponent<Portalold>();
        

        //_portal1.plane.GetComponent<SeamlessTeleport>().receiver = _portal2.plane;
        _portal1.plane.GetComponent<SeamlessTeleport>().player = gameObject;
        //dupe1 = portal1;

        //_portal2.plane.GetComponent<SeamlessTeleport>().receiver = _portal1.plane;
        _portal2.plane.GetComponent<SeamlessTeleport>().player = gameObject;
        //dupe2 = portal2;
    } 

    void Update()
    {
        Ray myRay;
        RaycastHit hit;
        var _dupe1 = dupe1.GetComponent<Portalold>();
        var _dupe2 = dupe2.GetComponent<Portalold>();

        myRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Has interact button been pressed whilst interactable object is in front of player?
        if (Input.GetKeyDown("a") && m_CanInteract == true)
        {
            
            Physics.Raycast(myRay, out hit);
            //Debug.Log(hit.collider.gameObject);
            if (hit.collider.gameObject == switch_prefab)
            {
                hit.collider.gameObject.GetComponent<Switch>().ButtonPressed();
            }
            else
            {
                IInteractable interactComponent = m_RaycastFocus.collider.transform.GetComponent<IInteractable>();

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
                    IInteractable interactComponent = m_RaycastFocus.collider.transform.GetComponent<IInteractable>();

                    if (interactComponent != null)
                    {
                        // Perform object's action
                        interactComponent.Action(this);
                    }
                }
            }
            else
            {
                // fire portal
                if (Physics.Raycast(myRay, out hit))
                {
                    if (hit.collider.transform.CompareTag("Moonrock"))
                    {
                        anim.Play("v_portalgun.qc_skeleton|fire1");
                        GameObject a = Instantiate(portal1, hit.point, Quaternion.identity);
                        var _a = a.GetComponent<Portalold>();
                        if (dupe2.activeSelf == true)
                        {
                            _a.pairPortal = dupe2.transform;
                            _dupe2.pairPortal = a.transform;

                            _dupe2.plane.GetComponent<SeamlessTeleport>().receiver = _a.plane;
                            _a.plane.GetComponent<SeamlessTeleport>().receiver = _dupe2.plane;

                            _dupe2.plane.GetComponent<SeamlessTeleport>().otherportal = a;
                            _a.plane.GetComponent<SeamlessTeleport>().otherportal = dupe2;
                        }

                        a.transform.position = hit.point - myRay.direction.normalized * 0.01f;
                        a.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);

                        _a.plane.GetComponent<SeamlessTeleport>().player = gameObject;

                        Destroy(dupe1);
                        dupe1 = a.gameObject;
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
                    IInteractable interactComponent = m_RaycastFocus.collider.transform.GetComponent<IInteractable>();

                    if (interactComponent != null)
                    {
                        // Perform object's action
                        interactComponent.Action(this);
                    }
                }
            }
            else
            {
                // fire portal
                if (Physics.Raycast(myRay, out hit))
                {
                    if (hit.collider.transform.CompareTag("Moonrock"))
                    {
                        anim.Play("v_portalgun.qc_skeleton|fire1");
                        GameObject b = Instantiate(portal2, hit.point, Quaternion.identity);
                        var _b = b.GetComponent<Portalold>();
                        if (dupe1.activeSelf == true)
                        {
                            _b.pairPortal = dupe1.transform;
                            _dupe1.pairPortal = b.transform;

                            _dupe1.plane.GetComponent<SeamlessTeleport>().receiver = _b.plane;
                            _b.plane.GetComponent<SeamlessTeleport>().receiver = _dupe1.plane;

                            _dupe1.plane.GetComponent<SeamlessTeleport>().otherportal = b;
                            _b.plane.GetComponent<SeamlessTeleport>().otherportal = dupe1;
                        }

                        b.transform.position = hit.point - myRay.direction.normalized * 0.01f;
                        b.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);

                        _b.plane.GetComponent<SeamlessTeleport>().player = gameObject;

                        Destroy(dupe2);
                        dupe2 = b.gameObject;
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
        Ray ray = new Ray(m_CameraTransform.position, m_CameraTransform.forward);

        // Is interactable object detected in front of player?
        if (Physics.Raycast(ray, out m_RaycastFocus, 3) && m_RaycastFocus.collider.transform.CompareTag("Interactable"))
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

    public void PlayAnim(string animation)
    {
        anim.Play(animation);
    }
}*/