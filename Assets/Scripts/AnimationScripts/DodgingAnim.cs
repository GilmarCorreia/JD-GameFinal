using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgingAnim : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Movement moveScript = animator.GetComponent<Movement>();
        moveScript.dodging = false;
    }
}
