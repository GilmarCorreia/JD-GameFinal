using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]

public class Movement : MonoBehaviour
{
    public Animator anim;
    public CharacterController cc;
    public Rigidbody rb;

    [System.Serializable]
    public class AnimationStrings
    {
        public string forward = "moveV";
        public string strafe = "moveH";
        public string run = "running";
        public string aim = "aiming";
        public string pullString = "pullString";
        public string fire = "fire";
        public string equip = "equip";
        public string unequip = "unequip";
        public string fall = "falling";
        public string land = "land";
        public string landing = "landing";
        public string jump = "jump";
    }
    [SerializeField]
    public AnimationStrings animStrings;

    [Header("Gravity Settings")]
    public float gravity = 10f;

    public bool falling = false;
    public bool landing = false;
    public bool jumping = false;
    public float fallStep = 0.4f;
    public float landStep = 0.5f;
    Coroutine JumpCoroutine;
    Vector3 jumpMove = Vector3.zero;
    Vector3 jumpDestiny = Vector3.zero;
    Vector3 jumpDestinyHor = Vector3.zero;
    Vector3 jumpDestinyVert = Vector3.zero;
    Vector3 jumpMoveHor = Vector3.zero;
    Vector3 jumpMoveVert = Vector3.zero;
    Vector3[] jumpPositions = new Vector3[2];
    //public float fallStep = 0.05f;
    //public float landStep = 0.10f;

    InputSystem input;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        input = GetComponent<InputSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //print(cc.velocity);
        CharacterFall();
        CharacterLand();
    }

    public void AnimateCharacter(float forward,float strafe)
    {
        if (falling && landing)
        {
            forward = 0;
            strafe = 0;
        }
        anim.SetFloat(animStrings.forward, forward);
        anim.SetFloat(animStrings.strafe, strafe);
    }

    public void SprintCharacter(bool isSprinting)
    {
        isSprinting = isSprinting && !falling && !landing;
        anim.SetBool(animStrings.run,isSprinting);
    }

    public void RotateCharacter(float turningSpeed)
    {
        var impulse = (turningSpeed * Mathf.Deg2Rad) * Vector3.up;
        rb.AddTorque(impulse, ForceMode.Impulse);
    }

    public void CharacterEquipGun(bool equip)
    {
        if(equip)
            anim.SetTrigger(animStrings.equip);
        else
            anim.SetTrigger(animStrings.unequip);
    }

    public void CharacterAim(bool aiming)
    {
        anim.SetBool(animStrings.aim,aiming);
    }

    public void CharacterPullString(bool isPulling)
    {
        anim.SetBool(animStrings.pullString,isPulling);
    }

    public void CharacterFire()
    {
        //print("fire");
        anim.SetTrigger(animStrings.fire);
    }

    public void CharacterJump(bool jumpInput)
    {
        if (jumpInput && !falling && !landing)
        {
            print("jumping");
            jumping = true;
            //jumpDestiny = transform.position + 5f * transform.forward.normalized + 8f * transform.up.normalized;
            jumpDestinyHor = 5f * transform.forward.normalized;
            jumpDestinyVert = 5f * transform.up.normalized;
            //jumpPositions[0] = transform.position + 1.5f * transform.forward.normalized + 2.5f * transform.up.normalized;
            //jumpPositions[1] = transform.position + 5f * transform.forward.normalized;
            anim.SetBool(animStrings.jump, true);
        }
        if (jumping)
        {
            //jumpMove = Vector3.Lerp(jumpMove, jumpDestiny, 10f * Time.deltaTime);
            jumpMoveHor = Vector3.Lerp(jumpMoveHor, jumpDestinyHor, 10f * Time.deltaTime);
            jumpMoveVert = Vector3.Lerp(jumpMoveVert, jumpDestinyVert, 10f * Time.deltaTime);
            Vector3 jumpMove = jumpMoveHor *Input.GetAxis(input.input.forwardInput) + jumpMoveVert;
            Debug.DrawLine(transform.position, transform.position + jumpMove, Color.yellow);
            cc.Move(jumpMove * Time.deltaTime);
        }
        else
        {
            jumpPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
            //jumpMove = Vector3.zero;
            jumpMoveHor = Vector3.zero;
            jumpMoveVert = Vector3.zero;
        }
    }

    public void CharacterFall()
    {
        //if (!cc.isGrounded)
        if (!CloseToGround(fallStep,Color.green) && !jumping)
        {
            float pushForward = 0.1f;
            
            if (rb.velocity.y > -1f /*&& rb.velocity.y < 0.2*/)
                cc.Move(new Vector3(0, -gravity * Time.deltaTime, 0) + pushForward*transform.forward);
            else
                cc.Move(new Vector3(0, -gravity * Time.deltaTime, 0));
            //print(cc.velocity);
            falling = true;
            landing = false;
            //CharacterLand();
        }
        else
        {
            //cc.Move(new Vector3(0, 0, 0));
            if(!cc.isGrounded && !jumping)
                cc.Move(new Vector3(0, -gravity * Time.deltaTime, 0));
            landing = false;
            falling = false;
        }
        anim.SetBool(animStrings.fall, falling);
        anim.SetBool(animStrings.landing, landing);
    }

    public void CharacterLand()
    {
        //if(cc.isGrounded)
        //if (CloseToGround(landStep,Color.cyan))
        if (CloseToGround(landStep, Color.cyan) && falling)
        {
            falling = false;
            landing = true;
            //anim.SetBool(animStrings.land,true);
            anim.SetTrigger(animStrings.land);
        }
    }

    public void FinishLanding()
    {
        landing = false;
        anim.SetBool(animStrings.land, false);
    }

    public void FinishJump()
    {
        jumping = false;
        anim.SetBool(animStrings.jump,false);
    }

    public bool CloseToGround(float extraHeight, Color color)
    {
        RaycastHit hit;
        Vector3 dir = transform.forward;
        Vector3 center = transform.position;
        //Vector3 halfExtents = new Vector3(cc.radius, 0.5f * extraHeight, cc.radius);
        Vector3 halfExtents = new Vector3(cc.radius, extraHeight, cc.radius) * 0.5f;
        Quaternion orientation = Quaternion.LookRotation(dir);
        //print(charController.bounds.extents.y + extraHeight);

        bool grounded = Physics.Raycast(transform.position, Vector3.down, out hit, cc.bounds.extents.y + extraHeight);
        Debug.DrawRay(transform.position, Vector3.down, Color.red, 1f);
        //bool grounded = Physics.BoxCast(center, halfExtents, dir, out hit, orientation);
        //ExtDebug.DrawBoxCastOnHit(center, halfExtents, orientation, dir, 0, color);
        //print(transform.position - hit.point);
        return grounded;
    }
}
