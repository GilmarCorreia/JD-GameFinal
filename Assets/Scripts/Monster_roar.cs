using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;


public class Monster_roar : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Monster>().isInvulnerable = true;
        animator.GetComponent<Monster>().speed = 0.0f;
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Monster>().isInvulnerable = false;
        animator.GetComponent<Monster>().speed = 2.5f;
    }

}
