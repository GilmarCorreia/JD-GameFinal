using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster_Attack : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Monster>().isAttacking = true;
        animator.GetComponent<Monster>().speed = 0.0f;
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Monster>().isAttacking = false;
        animator.GetComponent<Monster>().speed = 2.5f;
    }
}
