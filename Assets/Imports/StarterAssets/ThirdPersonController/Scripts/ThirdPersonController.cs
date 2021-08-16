using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
using Cinemachine;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class ThirdPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 2.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 5.335f;
		[Tooltip("How fast the character turns to face movement direction")]
		[Range(0.0f, 0.3f)]
		public float RotationSmoothTime = 0.12f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.50f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.28f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 70.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -30.0f;
		[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
		public float CameraAngleOverride = 0.0f;
		[Tooltip("For locking the camera position on all axis")]
		public bool LockCameraPosition = false;
		
		[Header("Items getting settings")]
		public Transform ItemHandPos;
		public Transform ItemHandParent;

		
		// cinemachine
		private float _cinemachineTargetYaw;
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _animationBlend;
		private float _targetRotation = 0.0f;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;
		bool diving = false;

		private bool _canPickItem = false;

		private GameObject _inRangeItem;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		// animation IDs
		private int _animIDSpeed;
		private int _animIDGrounded;
		private int _animIDJump;
		private int _animIDFreeFall;
		private int _animIDMotionSpeed;
		private int _animIDDive;
		private int _animIDCrouch;
		private int _animIDPickBow;
		private int _animIDArmedBow;
		private int _animIDAiming;
		private int _animIDShoot;
		private int _animIDPullString;

		private Animator _animator;
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;

		private const float _threshold = 0.01f;

		private bool _hasAnimator;
		public bool hasShootingMechanics = false;

		public Bow bow;

		[Header("Aimiming Settings")]
		public CinemachineStateDrivenCamera StateDrivenCam;
		Animator camAnim;

		[Header("Spine Settings")]
		public Transform spine;
		public Vector3 spineOffset = new Vector3(13, 101.7f, 8);

		Ray ray;
		RaycastHit hit;
		bool hitDetected = false;

		bool doSpineRotation = false;
		float delayToRotateSpine = .5f;
		float rotateSpineTimer = 0f;

		//bool lastShootInput = false;
		//bool pullingString = false;
		public bool testAim = false;
		public bool pickingObject = false;

		[Header("Aim Camera Settings")]
		public float cameraYrotation = 0;
		public float cameraXrotation = 0;
		Transform center;
		public CinemachineVirtualCamera VCam;

		[System.Serializable]
		public class CameraSettings
		{
			[Header("Camera Move Settings")]
			public float rotationSpeed = 5;
			public float mouseXSense = 5;
			public float mouseYSense = 5;
			public float maxClampAngle = 90;
			public float minClampAngle = -30;
		}
		[SerializeField]
		public CameraSettings camSettings;

		[System.Serializable]
		public class CameraInputSettings
		{
			public string MouseXAxis = "Mouse X";
			public string MouseYAxis = "Mouse Y";
			//public string AimingInput = "Fire2";
		}
		[SerializeField]
		public CameraInputSettings camInputSettings;

		[Header("Capsule Settings")]
		public float _capsuleHeight = 3.4f;
		public Vector3 _capsuleCenter = new Vector3(0, 1.6f, 0);
		public float _capsuleHeightDown = 2.4f;
		public Vector3 _capsuleCenterDown = new Vector3(0, 1f, 0);

		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			_hasAnimator = TryGetComponent(out _animator);
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();

			AssignAnimationIDs();

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;

			//Animator das Cameras
			camAnim = StateDrivenCam.GetComponent<Animator>();
			center = VCam.Follow.transform;
		}

		private void Update()
		{
			_hasAnimator = TryGetComponent(out _animator);

			JumpAndGravity();
			GroundedCheck();
			Move();
			PickObject();
			if(hasShootingMechanics)
				ShootingMecanics();
            if (Input.GetKeyDown(KeyCode.J))
            {
				Cursor.lockState = CursorLockMode.None;
            }
		}

		public void UpdatePickItemFlag(bool nearToPlayer)
		{
			this._canPickItem = nearToPlayer;
		}

		public void UpdateNearGameObject(GameObject item)
		{
			Debug.Log("Item: " + item.name);
			this._inRangeItem = item;
		}

		private void PickObject()
        {
			if (_input.pickObject && this._canPickItem)
			{
				_animator.SetBool(_animIDPickBow, true);
			}
		}

		public void GrabItem()
		{
			Debug.Log("Grabing ITEM...");
			// Esta bugando a posicao...
			// _inRangeItem.transform.position = ItemHandPos.position;
			// _inRangeItem.transform.rotation = ItemHandPos.rotation;
			// _inRangeItem.transform.parent = ItemHandParent;
			
		}

		public void StoreItem()
		{
			Debug.Log("Grabing ITEM...");
			// Destroy(_inRangeItem);
		}

		public void StartPickingItem()
		{
			pickingObject = true;
		}

		public void FinishPickingItem()
        {
			print("Acabou de pegar");
			pickingObject = false;
		}
   
        private void ShootingMecanics()
        {
			if (_input.equipBow)
			{
				_animator.SetBool(_animIDArmedBow, true);
			}

			if (testAim)
				_input.aiming = true;

			if (_input.aiming && _input.equipBow)
			{
				_animator.SetBool(_animIDAiming, true);
				camAnim.Play("AimCam");
				Aim();
				RotateCamera();
				_animator.SetBool(_animIDPullString, _input.shoot);
                if (Input.GetButtonUp("Fire1"))
                {
					_animator.SetTrigger(_animIDShoot);
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
				camAnim.Play("PlayerCam");
				bow.RemoveCrosshair();
				bow.ReleaseString();
				bow.DisableArrow();
			}

			//if (_input.shoot)
			//{
			//	//_animator.SetBool(_animIDShoot, true);
			//}

		}



		private void LateUpdate()
		{
			CameraRotation();
            if (hasShootingMechanics)
            {
				if (_input.aiming && _input.equipBow)
				{
					RotateCharacterSpine();
				}
            }
		}

		private void AssignAnimationIDs()
		{
			_animIDSpeed = Animator.StringToHash("Speed");
			_animIDGrounded = Animator.StringToHash("Grounded");
			_animIDJump = Animator.StringToHash("Jump");
			_animIDFreeFall = Animator.StringToHash("FreeFall");
			_animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
			_animIDDive = Animator.StringToHash("Dive");
			_animIDCrouch = Animator.StringToHash("Crouch");
			_animIDPickBow = Animator.StringToHash("PickBow");
			_animIDArmedBow = Animator.StringToHash("ArmedBow");
			_animIDAiming = Animator.StringToHash("AimingBow");
			_animIDShoot = Animator.StringToHash("Shoot");
			_animIDPullString = Animator.StringToHash("PullString");
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

			// update animator if using character
			if (_hasAnimator)
			{
				_animator.SetBool(_animIDGrounded, Grounded);
			}
		}

		private void CameraRotation()
		{
			// if there is an input and camera position is not fixed
			if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
			{
				_cinemachineTargetYaw += _input.look.x * Time.deltaTime;
				_cinemachineTargetPitch += _input.look.y * Time.deltaTime;
			}

			// clamp our rotations so our values are limited 360 degrees
			_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

			// Cinemachine will follow this target
			CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
		}

		private void Move()
		{
            if (pickingObject == true)
            {
                return;
            }

            if (_input.dive){
				_animator.SetBool(_animIDDive, true);
			}

            if (_input.crouch)
            {
				_animator.SetBool(_animIDCrouch, true);
			}

			ReduceHitbox(_input.crouch || diving);

            /*if (_hasAnimator)
            {
				
			}*/

			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}
			_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero)
			{
				_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
				float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

				// rotate to face input direction relative to camera position
				transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
			}


			Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

			// move the player
			 _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

			// update animator if using character
			if (_hasAnimator)
			{
				_animator.SetFloat(_animIDSpeed, _animationBlend);
				_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
			}
		}

		private void JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// update animator if using character
				_animator.SetBool(_animIDPickBow, false);
				if (_hasAnimator)
				{
					_animator.SetBool(_animIDDive, false);
					_animator.SetBool(_animIDCrouch, false);
					_animator.SetBool(_animIDArmedBow, false);
					_animator.SetBool(_animIDAiming, false);
					//_animator.SetBool(_animIDShoot, false);
					_animator.SetBool(_animIDJump, false);
					_animator.SetBool(_animIDFreeFall, false);
				}

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

					// update animator if using character
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDJump, true);
					}
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}
				else
				{
					// update animator if using character
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDFreeFall, true);
					}
				}

				// if we are not grounded, do not jump
				_input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		public void ReduceHitbox(bool reduce)
        {
            if (reduce)
            {
				_controller.height = _capsuleHeightDown;
				_controller.center = _capsuleCenterDown;
            }
            else
            {
				_controller.height = _capsuleHeight;
				_controller.center = _capsuleCenter;
			}
        }

		public void StartDive()
		{
			diving = true;
		}

		public void FinishDive()
        {
			diving = false;
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
				//if (doSpineRotation)
				//	bow.ShowCrosshair(hit.point);
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
            //if (_input.aiming)
            //{
            //	rotateSpineTimer = delayToRotateSpine;
            //	doSpineRotation = false;
            //}
            //if (rotateSpineTimer > 0)
            //{
            //	rotateSpineTimer -= Time.deltaTime;
            //}
            //else
            //{
            //	doSpineRotation = true;
            //}
            //if (doSpineRotation)
            //{
            //	spine.LookAt(ray.GetPoint(50));
            //	spine.Rotate(spineOffset);
            //}
            //print(rotateSpineTimer);
            //print(doSpineRotation);
            spine.LookAt(ray.GetPoint(50));
            spine.Rotate(spineOffset);
        }

		public void RotateCamera()
		{
            //cameraYrotation += Input.GetAxis(camInputSettings.MouseXAxis) * camSettings.mouseXSense;
            //cameraXrotation -= Input.GetAxis(camInputSettings.MouseYAxis) * camSettings.mouseYSense;
            cameraYrotation += _input.look.y;
            cameraXrotation -= _input.look.x;
            cameraXrotation = Mathf.Clamp(cameraXrotation, camSettings.minClampAngle, camSettings.maxClampAngle);
			cameraYrotation = Mathf.Repeat(cameraYrotation, 360);
			Vector3 rotatingAngle = new Vector3(cameraXrotation, cameraYrotation, 0);
			Quaternion rotation = Quaternion.Slerp(center.transform.localRotation, Quaternion.Euler(rotatingAngle), camSettings.rotationSpeed * Time.deltaTime);
			center.transform.localRotation = rotation;
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;
			
			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}
	}
}