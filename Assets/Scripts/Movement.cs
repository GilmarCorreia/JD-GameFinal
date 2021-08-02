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
        public string land = "landing";
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
        //CharacterLand();
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
        if(jumpInput && !falling && !landing)
        {
            jumping = true;
            jumpDestiny = 5f * transform.forward.normalized + 8f * transform.up.normalized;
            //Vector3 move = Vector3.Slerp(destiny, transform.position, .1f * Time.deltaTime);
            //cc.Move(new Vector3(10f*transform.forward.x,5f,10f*transform.transform.forward.z));

            //JumpCoroutine = StartCoroutine(JumpMovement(5f*transform.forward + 5f*transform.up));
            anim.SetBool(animStrings.jump, true);
        }
        if (jumping)
        {
            cc.Move(jumpMove * Time.deltaTime);
            jumpMove = Vector3.Slerp(jumpDestiny,transform.position, 1f * Time.deltaTime);
        }

    }


    IEnumerator JumpMovement(Vector3 destiny)
    {

        while (Vector3.SqrMagnitude(transform.position - destiny) > float.Epsilon)
        {
            jumpMove = Vector3.Slerp(transform.position, destiny,1f * Time.deltaTime);
            //cc.Move(move);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    public void CharacterFall()
    {
        //if (!cc.isGrounded)
        if (!CloseToGround(fallStep,Color.green) && !jumping)
        {
            float moveX = 0.01f, moveZ = 0.01f;
            
            if (rb.velocity.y > -1f /*&& rb.velocity.y < 0.2*/)
                cc.Move(new Vector3(moveX, -gravity * Time.deltaTime, moveZ));
            else
                cc.Move(new Vector3(0, -gravity * Time.deltaTime, 0));
            //print(cc.velocity);
            falling = true;
            CharacterLand();
        }
        else
        {
            //cc.Move(new Vector3(0, 0, 0));
            falling = false;
        }
        anim.SetBool(animStrings.fall, falling);
    }

    public void CharacterLand()
    {
        //if(cc.isGrounded)
        if (CloseToGround(landStep,Color.cyan))
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
        print(transform.position - hit.point);
        return grounded;
    }

    //public bool CloseToGround(float gap, Color color)
    //{
    //    float radius = cc.radius * (1f-gap);
    //    Vector3 pos = transform.position - Vector3.up * (radius * 0.9f);
    //    bool grounded = Physics.CheckSphere(pos, radius, 1 << 6);
    //    return grounded;
    //}

    //public void OnDrawGizmos()
    //{
    //    float extraHeight1 = .5f;
    //    float extraHeight2 = .2f;
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireCube(transform.position - 0.1f * Vector3.up, new Vector3(cc.radius, 0.5f * extraHeight1, cc.radius));
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireCube(transform.position - 0.1f * Vector3.up, new Vector3(cc.radius, 0.5f * extraHeight2, cc.radius));

    //}
}
