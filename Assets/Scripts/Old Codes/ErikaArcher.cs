using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class ErikaArcher : MonoBehaviour
{
    public Animator anim;
    public GameObject BowMesh;
    public CharacterController charController;
    public Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    public GameObject mainCamera;
    //public CinemachineVirtualCamera VcamPlayer;
    public CinemachineStateDrivenCamera StateDrivenCam;
    Transform camCenter;
    
    //public Image crosshairImage;
    public GameObject arrowPrefab;
    GameObject arrow;
    GameObject arrowMesh;
    Transform firePoint;
    public GameObject hand;
    public Bow bow;

    //constantes
    float walk_speed = 3f;
    float run_speed = 6f;
    float vel_turn_max = 80f;
    float vel_pulo = 5f;
    float gravity = 24f;
    private float jumpHeight = 2.5f;
    private float maxIncline = 45f;

    [HideInInspector] public float entradaH = 0f;
    [HideInInspector] public float entradaV = 0f;
    [HideInInspector] public float mouseXInput = 0f;
    [HideInInspector] public float mouseYInput = 0f;
    float vel_turn;
    float velAnimation;
    float moveSpeed;
    float moveX, moveZ;
    bool grounded;
    Vector3 vel = Vector3.zero;

    private bool run = false;
    private bool jump = false;
    [HideInInspector] public bool jumping = false;
    [HideInInspector] public bool moving = false;
    private bool attacking = false;
    private bool falling = false;
    [HideInInspector] public bool landing = false;
    private bool armed = false;
    private bool isAiming = false;
    private bool drawing = false;
    [HideInInspector] public bool shoot = false;
    Coroutine shootingCoroutine;

    [Header("Camera and Character Syncing")]
    public float lookDistance = 5f;
    public float lookSpeed = 5f;

    [Header("Aiming Settings")]
    RaycastHit hit;
    public LayerMask aimLayers;
    Ray ray;

    [Header("Crosshair settings")]

    [Header("Spine Settings")]
    public Transform spine;
    Vector3 spineOffset = new Vector3(13, 101.7f, 8); 

    //[Header("Head Rotation Settings")]
    //public float lookAtPoint = 2.8f;

    //private float cameraTranslationF = 0f;
    //private float cameraTranslationB = 0f;
    //private float playerSpeed = 2.0f;
    //private float gravityValue = -9.81f;

    void Start()
    {
        anim = GetComponent<Animator>();
        charController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        bow.ShowBow(false);
        firePoint = hand.transform;
        shootingCoroutine = null;
    }

    // Update is called once per frame
    void Update()
    {
        
        isAiming = Input.GetKey(KeyCode.Mouse1); // Se clicar com o botão direito do mouse, executa o Aim
        UpdateWeaponState();
        UpdateMovementStates();
        //UpdateAttackStates();
        UpdateAnimations();
        Aim(isAiming);
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void LateUpdate()
    {
        if (isAiming)
        {
            RotateCharacterSpine();
        }
    }

    void UpdateMovementStates()
    {
        UpdateJumpState();
        UpdateRunState();

        entradaH = Input.GetAxis("Horizontal");
        entradaV = Input.GetAxis("Vertical");
        mouseXInput = Input.GetAxis("Mouse X");
        mouseYInput = Input.GetAxis("Mouse Y");

        vel_turn = vel_turn_max * Mathf.Clamp(mouseXInput, -1, 1);
        Vector3 moveHor = transform.right * entradaH + transform.forward * entradaV;
        vel = moveHor * moveSpeed + vel.y * Vector3.up;

        moving = moveHor.sqrMagnitude > float.Epsilon && !landing;
        velAnimation = moving ? velAnimation : 0f;
    }

    void UpdateJumpState()
    {
        //print(vel);
        //print(charController.isGrounded);
        //print(landing);
        grounded = IsGrounded();
        //print(run);
        if (Input.GetKeyDown("space"))
        {
            jump = true;
            jumping = true;
        }
        else if (jump && charController.isGrounded)
        {
            //print("AAA");
            anim.Play("JUMP");
            vel.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
            jump = false;
        }
        //else if(charController.isGrounded)
        //else if (rb.velocity.y < 0f && charController.isGrounded)
        //if (vel.y < 0f)
        //{
        //    //vel.y = 0f;
        //    jumping = false;
        //}

        //if (!charController.isGrounded && !jumping)
        if(!grounded && !jumping)
        {
            //print("BBB");
            falling = true;
        }
        //else if (charController.isGrounded && falling)
        else if (grounded && falling)
        {
            landing = true;
            falling = false;
        }

        // ------- Animaçoes --------
        anim.SetBool("jumping", jumping);
        anim.SetBool("falling", falling);
        anim.SetBool("landing", landing);
        anim.SetBool("falling", falling);

        // ------------- Apagar ------------------
        //anim.SetFloat("velocidade", velAnimation);
        //if (jump && charController.isGrounded)
        //{
        //    anim.Play("JUMP");
        //}
        //if (!charController.isGrounded && !jumping)
        //if (falling)
        //{
        //    anim.Play("FALLING");
        //}
        //else if (landing)
        //{
        //    //anim.Play("LANDING");
        //    anim.SetTrigger("landing");
        //}
        // -----------------------------------

        //if (charController.isGrounded)
        //{
        //    anim.SetBool("running", run);
        //}
        //else
        //{
        //    anim.SetBool("running", false);
        //}

        // ---- Velocidade vertical --------
        if (charController.isGrounded && !jumping)
        {
            vel.y = -gravity * Time.deltaTime;   
        }
        //else
        if (!charController.isGrounded)
        {
            vel.y -= gravity * Time.deltaTime;
        }
    }

    void UpdateRunState()
    {
        run = Input.GetKey(KeyCode.LeftShift);
        if (run && moving && !isAiming)
        {
            moveSpeed = run_speed;
            //print(moveSpeed);
            //velAnimation = 0.5f;
            //moving = true;
        }
        else
        {
            moveSpeed = walk_speed;
            //velAnimation = 0.2f;
            //moving = true;
        }
        anim.SetBool("walking", moving);
        anim.SetBool("running", run);
    }

    void UpdateWeaponState()
    {
        if (Input.GetKeyDown("1"))
        { // Se apertar 1, equipa e desequipa o arco
            if (!armed)
                //StartCoroutine("EquipBow");
                //StartCoroutine(bow.EquipBow());
                bow.EquipBow();
            else
                //StartCoroutine("DisarmBow");
                //StartCoroutine(bow.DisarmBow());
                bow.DisarmBow();

            armed = !armed;
            anim.SetBool("armed", armed);
        }
    }

    //IEnumerator EquipBow()
    //{
    //    anim.Play("equip_bow");
    //    yield return new WaitForSeconds(0.15f);
    //    ShowBow(true);
    //}
    //IEnumerator DisarmBow()
    //{
    //    anim.Play("disarm_bow");
    //    yield return new WaitForSeconds(0.5f);
    //    ShowBow(false);
    //}

    void UpdateAnimations()
    {
        anim.SetFloat("moveH", entradaH);
        anim.SetFloat("moveV", entradaV);
    }

    // Função que oculta e mostra o arco na cena.
    //void ShowBow(bool show)
    //{
    //    SkinnedMeshRenderer render = BowMesh.GetComponentInChildren<SkinnedMeshRenderer>();

    //    render.enabled = show;
    //}

    void Aim(bool aiming)
    {
        drawing = Input.GetKey(KeyCode.Mouse0) && armed && aiming;
        if(Input.GetKeyUp(KeyCode.Mouse0) && armed && aiming)
        {
            drawing = false;
            shoot = true;
        }
        if (shoot) {
            bow.Shoot();
        }
        if (!aiming)
        {
            bow.RemoveCrosshair();
            DisableArrow();
            ReleaseString();
        }
        //crosshairImage.enabled = aiming && armed;
        Animator animCamera = StateDrivenCam.GetComponent<Animator>();
        if (aiming && armed)
        {
            //crosshairImage.enabled = aiming;
            animCamera.Play("AimCam");
        }
        else
        {
            animCamera.Play("PlayerCam");
        }

        Vector3 camPosition = Camera.main.transform.position;
        Vector3 dir = Camera.main.transform.forward;
        ray = new Ray(camPosition, dir); 
        if(Physics.Raycast(ray,out hit, 500f))
        {
            Debug.DrawLine(ray.origin, hit.point,Color.green);
            //bow.ShowCrosshair(hit.point);
        }
        else
        {
            //bow.RemoveCrosshair();
        }

        anim.SetBool("drawing", drawing);
        anim.SetBool("aiming", aiming);
        anim.SetBool("shoot", shoot);
        //print(aiming);
    }

    //public void DrawArrow()
    //{
    //    //if (arrowPrefab != null && firePoint != null)
    //    //{
    //    //    arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(6.47f, 94.16f, 0f));
    //    //    arrowMesh = arrow.transform.Find("arrowMesh").gameObject;
    //    //    arrow.transform.parent = hand.transform;
    //    //}
    //    if(arrow != null)
    //    {

    //    }
    //}

    //void Shoot()
    //{
    //    arrow.transform.parent = null;
    //    arrow.transform.position += 2f*(arrowMesh.transform.Find("head").gameObject.transform.position - arrowMesh.transform.Find("tail").gameObject.transform.position);
    //    Rigidbody arrow_rb = arrow.AddComponent<Rigidbody>();
    //    BoxCollider arrow_bc = arrow.AddComponent<BoxCollider>();
    //    arrow_rb.drag = 1f;
    //    //arrow_bc.isTrigger = true;
    //    arrow.GetComponent<Rigidbody>().AddForce(3000f * arrow.transform.forward);
    //    arrow = null;
    //    arrowMesh = null;
    //}

    void MoveCharacter()
    {
        charController.Move(vel * Time.deltaTime);
        var impulse = (vel_turn * Mathf.Deg2Rad) * Vector3.up;
        rb.AddTorque(impulse, ForceMode.Impulse);
    }

    bool IsGrounded()
    {
        float extraHeight = .5f;
        
        //print(charController.bounds.extents.y + extraHeight);
        bool grounded = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + extraHeight);
        Debug.DrawRay(transform.position, Vector3.down, Color.red,1f);
        return grounded;
    }

    //void ShowCrosshair()
    //{
    //    bow.ShowCrosshair(crosshairPos);
    //}

    //void RemoveCrosshair()
    //{
    //    bow.RemoveCrosshair();
    //}

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
}

