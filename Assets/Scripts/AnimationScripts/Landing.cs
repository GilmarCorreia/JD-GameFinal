using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landing : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //ErikaArcher player = animator.GetComponent<ErikaArcher>();
        Movement moveScript = animator.GetComponent<Movement>();
        moveScript.landing = false ;
    }
}
