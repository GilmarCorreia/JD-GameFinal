using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster_idle : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Monster>().isInvulnerable = true;
        animator.GetComponent<Monster>().speed = 0.0f;
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Monster>().isInvulnerable = false;
        
    }
}
