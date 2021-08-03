using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]

public class InputSystem : MonoBehaviour
{
    Movement MoveScript;

    [System.Serializable]
    public class InputSettings
    {
        public string forwardInput = "Vertical";
        public string strafeInput = "Horizontal";
        public string sprintInput = "Sprint";
        public string crouch = "Crouch";
        public string jump = "Jump";
        public string dodge = "Dodge";
        public string aim = "Fire2";
        public string fire = "Fire1";
        public string equip = "1";
    }
    [SerializeField]
    public InputSettings input;

    [Header("Aiming Settings")]
    RaycastHit hit;
    public LayerMask aimLayers;
    Ray ray;

    //[Header("Crosshair settings")]

    [Header("Spine Settings")]
    public Transform spine;
    public Vector3 spineOffset = new Vector3(13, 101.7f, 8);

    [Header("Head Rotation Settings")]
    public float lookAtPosition = 2.8f;

    public float rotatingSpeed = 80f;

    bool hitDetected = false;
    public bool isAiming;
    public bool testAim;
    Transform camCenter;
    public Bow bow;
    public bool armed = false;
    Animator playerAnim;
    Rigidbody rb;
    CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        MoveScript = GetComponent<Movement>();
        playerAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //print(cc.isGrounded);
        isAiming = Input.GetButton(input.aim) && armed;
        if (testAim)
            isAiming = true;
        if (Input.GetKeyDown(input.equip))
        {
            armed = !armed;
            MoveScript.CharacterEquipGun(armed);
        }
        MoveScript.AnimateCharacter(Input.GetAxis(input.forwardInput),Input.GetAxis(input.strafeInput));
        MoveScript.SprintCharacter(Input.GetButton(input.sprintInput));
        MoveScript.CharacterCrouch(Input.GetButton(input.crouch));
        MoveScript.CharacterJump(Input.GetButtonDown(input.jump));
        MoveScript.CharacterDodge(Input.GetButtonDown(input.dodge));
        if (Input.GetAxis(input.forwardInput)!=0 || Input.GetAxis(input.strafeInput)!=0 || !isAiming)
        {
            MoveScript.RotateCharacter(rotatingSpeed * Input.GetAxis("Mouse X"));
        }
        MoveScript.CharacterAim(isAiming);
        if (isAiming)
        {
            Aim();
            //bow.EquipBow();
            MoveScript.CharacterPullString(Input.GetButton(input.fire));
            if (Input.GetButtonUp(input.fire))
            {
                MoveScript.CharacterFire();
                if (hitDetected)
                {
                    bow.Fire(hit.point);
                }
                else
                {
                    bow.Fire(ray.GetPoint(300));
                }
            }
                
        }
        else
        {
            //bow.DisarmBow();
            bow.RemoveCrosshair();
            ReleaseString();
            DisableArrow();
        }
    }

    private void LateUpdate()
    {
        if (isAiming)
        {
            RotateCharacterSpine();
        }
    }

    public void Aim()
    {
        Vector3 camPosition = Camera.main.transform.position;
        Vector3 dir = Camera.main.transform.forward;
        ray = new Ray(camPosition, dir);
        if (Physics.Raycast(ray, out hit, 500f))
        {
            hitDetected = true;
            Debug.DrawLine(ray.origin, hit.point, Color.green);
            bow.ShowCrosshair(hit.point);
        }
        else
        {
            hitDetected = false;
            bow.RemoveCrosshair();
        }
    }

    void RotateCharacterSpine()
    {
        spine.LookAt(ray.GetPoint(50));
        spine.Rotate(spineOffset);
    }

    public void PullString()
    {
        bow.PullString();
    }

    public void ReleaseString()
    {
        bow.ReleaseString();
    }

    public void EnableArrow()
    {
        bow.PickArrow();
    }

    public void DisableArrow()
    {
        bow.DisableArrow();
    }

    public void EquipBow()
    {
        bow.EquipBow();
    }

    public void DisarmBow()
    {
        bow.DisarmBow();
    }

    //private void OnAnimatorIK(int layerIndex)
    //{
    //    if (isAiming)
    //    {
    //        playerAnim.SetLookAtWeight(1f);
    //        playerAnim.SetLookAtPosition(ray.GetPoint(lookAtPosition));
    //    }
    //    else
    //    {
    //        playerAnim.SetLookAtWeight(0);
    //    }
    //}
}
