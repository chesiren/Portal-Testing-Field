using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : PortalTraveller {

    public float walkSpeed = 3;
    public float runSpeed = 6;
    public float smoothMoveTime = 0.1f;
    public float jumpForce = 8;
    public float gravity = 18;

    public bool lockCursor;
    public float mouseSensitivity = 10;
    public Vector2 pitchMinMax = new Vector2 (-40, 85);
    public float rotationSmoothTime = 0.1f;

    CharacterController controller;
    Camera cam;
    public float yaw;
    public float pitch;
    float smoothYaw;
    float smoothPitch;

    float yawSmoothV;
    float pitchSmoothV;
    float verticalVelocity;
    Vector3 velocity;
    Vector3 smoothV;

    bool jumping;
    float lastGroundedTime;
    bool disabled;

    bool key_z;
    bool key_q;
    bool key_s;
    bool key_d;
    bool key_e;

    Vector3 modelpos;
    public double moovediranim;

    Animation anim;

    void Start () {
        modelpos = GraphicsObject.transform.position;
        print(modelpos);
        anim = GraphicsObject.GetComponent<Animation>();
        //anim.Play("chell.qc_skeleton|a_idle_portalgun");
        anim.Play("chell.qc_skeleton|a_idle_portalgun");

        cam = Camera.main;
        if (lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        controller = GetComponent<CharacterController> ();

        yaw = transform.eulerAngles.y;
        pitch = cam.transform.localEulerAngles.x;
        smoothYaw = yaw;
        smoothPitch = pitch;
    }

    void Update () {
        key_z = Input.GetKeyDown(KeyCode.Z);
        key_q = Input.GetKeyDown(KeyCode.Q);
        key_s = Input.GetKeyDown(KeyCode.S);
        key_d = Input.GetKeyDown(KeyCode.D);
        key_e = Input.GetKey(KeyCode.E);

        float zs = Input.GetAxis("Vertical");
        float qd = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown (KeyCode.P)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Break ();
        }

        if (PauseMenu.GameIsPaused) {
            return;
        }
        
        Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

        Vector3 inputDir = new Vector3 (input.x, 0, input.y).normalized;
        Vector3 worldInputDir = transform.TransformDirection (inputDir);

        float currentSpeed = (Input.GetKey (KeyCode.LeftShift)) ? runSpeed : walkSpeed;
        Vector3 targetVelocity = worldInputDir * currentSpeed;
        velocity = Vector3.SmoothDamp (velocity, targetVelocity, ref smoothV, smoothMoveTime);

        verticalVelocity -= gravity * Time.deltaTime;
        velocity = new Vector3 (velocity.x, verticalVelocity, velocity.z);

        var flags = controller.Move (velocity * Time.deltaTime);
        if (flags == CollisionFlags.Below) {
            jumping = false;
            lastGroundedTime = Time.time;
            verticalVelocity = 0;
        }

        if (Input.GetKeyDown (KeyCode.Space)) {
            float timeSinceLastTouchedGround = Time.time - lastGroundedTime;
            if (controller.isGrounded || (!jumping && timeSinceLastTouchedGround < 0.15f)) {
                jumping = true;
                verticalVelocity = jumpForce;
            }
        }

        float mX = Input.GetAxisRaw ("Mouse X");
        float mY = Input.GetAxisRaw ("Mouse Y");

        // Verrrrrry gross hack to stop camera swinging down at start
        float mMag = Mathf.Sqrt (mX * mX + mY * mY);
        if (mMag > 5) {
            mX = 0;
            mY = 0;
        }

        yaw += mX * mouseSensitivity;
        pitch -= mY * mouseSensitivity;
        pitch = Mathf.Clamp (pitch, pitchMinMax.x, pitchMinMax.y);
        smoothPitch = Mathf.SmoothDampAngle (smoothPitch, pitch, ref pitchSmoothV, rotationSmoothTime);
        smoothYaw = Mathf.SmoothDampAngle (smoothYaw, yaw, ref yawSmoothV, rotationSmoothTime);

        transform.eulerAngles = Vector3.up * smoothYaw;
        cam.transform.localEulerAngles = Vector3.right * smoothPitch;
        
        //print("zs " + zs);
        //print("qd " + qd);
        /*
        if (Input.GetKey(KeyCode.Space))
        {
            anim.Play("chell.qc_skeleton|portalgun_standing_jump");
        }
        if (!key_e)
        {
            if (zs > 0 && qd == 0) // avancer
            {
                //anim.Play("chell.qc_skeleton|a_runn_portalgun");
                
                print(moovediranim);
                if (moovediranim >= 4)
                {
                    modelpos = new Vector3(modelpos.x, modelpos.y, modelpos.z - (float)moovediranim);
                    moovediranim = 0;
                }
                else
                {
                    moovediranim += 0.01;
                    modelpos = new Vector3(modelpos.x, modelpos.y, modelpos.z + (float)moovediranim);
                }
            }
            else if (zs < 0 && qd == 0) // reculer
            {
                anim.Play("chell.qc_skeleton|a_runs_portalgun");
            }
            else if (zs == 0 && qd > 0) // droite
            {
                anim.Play("chell.qc_skeleton|a_rune_portalgun");
            }
            else if (zs == 0 && qd < 0) // gauche
            {
                anim.Play("chell.qc_skeleton|a_runw_portalgun");
            }
            else if (zs > 0 && qd > 0) // avance-droite
            {
                anim.Play("chell.qc_skeleton|a_runne_portalgun");
            }
            else if (zs > 0 && qd < 0) // avance-gauche
            {
                anim.Play("chell.qc_skeleton|a_runnw_portalgun");
            }
            else if (zs < 0 && qd > 0) // recule-droite
            {
                anim.Play("chell.qc_skeleton|a_runse_portalgun");
            }
            else if (zs < 0 && qd < 0) // recule-gauche
            {
                anim.Play("chell.qc_skeleton|a_runsw_portalgun");
            }
            else if (zs == 0 && qd == 0) // idle
            {
                anim.Play("chell.qc_skeleton|a_idle_portalgun");
            }
        } else {
            if (zs > 0 && qd == 0) // avancer crouch
            {
                anim.Play("chell.qc_skeleton|a_crouchn_portalgun");
            }
            else if (zs < 0 && qd == 0) // reculer crouch
            {
                anim.Play("chell.qc_skeleton|a_crouchs_portalgun");
            }
            else if (zs == 0 && qd > 0) // droite crouch
            {
                anim.Play("chell.qc_skeleton|a_crouche_portalgun");
            }
            else if (zs == 0 && qd < 0) // gauche crouch
            {
                anim.Play("chell.qc_skeleton|a_crouchw_portalgun");
            }
            else if (zs > 0 && qd > 0) // avance-droite crouch
            {
                anim.Play("chell.qc_skeleton|a_crouchne_portalgun");
            }
            else if (zs > 0 && qd < 0) // avance-gauche crouch
            {
                anim.Play("chell.qc_skeleton|a_crouchnw_portalgun");
            }
            else if (zs < 0 && qd > 0) // recule-droite crouch
            {
                anim.Play("chell.qc_skeleton|a_crouchse_portalgun");
            }
            else if (zs < 0 && qd < 0) // recule-gauche crouch
            {
                anim.Play("chell.qc_skeleton|a_crouchsw_portalgun");
            }
            else if (zs == 0 && qd == 0) // idle crouch 
            {
                anim.Play("chell.qc_skeleton|a_crouchidle_portalgun");
            }
        }*/
    }

    public override void Teleport (Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot) {
        transform.position = pos;
        Vector3 eulerRot = rot.eulerAngles;
        float delta = Mathf.DeltaAngle (smoothYaw, eulerRot.y);
        yaw += delta;
        smoothYaw += delta;
        transform.eulerAngles = Vector3.up * smoothYaw;
        velocity = toPortal.TransformVector (fromPortal.InverseTransformVector (velocity));
        Physics.SyncTransforms ();
    }

}