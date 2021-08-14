using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Monster_run : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 3f;

    Transform player;
    Rigidbody rb;
    Monster monster;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody>();
        monster = animator.GetComponent<Monster>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monster.lookAtPlayer();
        UnityEngine.Vector3 target = new UnityEngine.Vector3(player.position.x, player.position.y, player.position.z);
        UnityEngine.Vector3 newPos = UnityEngine.Vector3.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (UnityEngine.Vector3.Distance(player.position, rb.position) <= attackRange)
        {
            animator.SetTrigger("rig|attack"); 
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("rig|attack");   
    }
}
