using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool dive;
		public bool flashlight;
		private bool flashMode = false;

		public bool pickObject;

		public bool equipBow;
		private bool equipMode = false;

		public bool aiming;
		public bool shoot;

		[Header("Movement Settings")]
		public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnDive(InputValue value)
		{
			DiveInput(value.isPressed);
		}

		public void OnFlashlight(InputValue value)
        {
			if (value.isPressed)
			{
				flashMode = !flashMode;
				FlashlightInput(flashMode);
			}
        }

		public void OnPickObject(InputValue value)
		{
			PickObjectInput(value.isPressed);
		}

		public void OnEquipBow(InputValue value)
		{
			if(value.isPressed)
			{
				equipMode = !equipMode;
				EquipBowInput(equipMode);
			}
		}

		public void OnAiming(InputValue value)
		{
			AimingInput(value.isPressed);
		}

		public void OnShoot(InputValue value)
		{
			ShootInput(value.isPressed);
		}

#else
		// old input sys if we do decide to have it (most likely wont)...
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void DiveInput(bool newDiveState)
		{
			dive = newDiveState;
		}

		public void FlashlightInput(bool newFlashlightState)
		{
			flashlight = newFlashlightState;
		}

		public void PickObjectInput(bool newPickObjectState)
		{
			pickObject = newPickObjectState;
		}

		public void EquipBowInput(bool newEquipBowState)
		{
			equipBow = newEquipBowState;
		}

		public void AimingInput(bool newAimingState)
		{
			aiming = newAimingState;
		}

		public void ShootInput(bool newShootState)
		{
			shoot = newShootState;
		}

#if !UNITY_IOS || !UNITY_ANDROID

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

#endif

	}
	
}