using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingAnim : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Movement moveScript = animator.GetComponent<Movement>();
        moveScript.falling = false;
    }
}

