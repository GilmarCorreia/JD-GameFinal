using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monster_walk : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Monster>().isInvulnerable = false;
        animator.GetComponent<Monster>().speed = 2.5f;
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Monster>().isInvulnerable = false;
        animator.GetComponent<Monster>().speed = 2.5f;
    }
}
