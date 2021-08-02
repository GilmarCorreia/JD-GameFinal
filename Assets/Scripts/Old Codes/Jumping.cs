using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ErikaArcher player = animator.GetComponent<ErikaArcher>();
        player.jumping = false;
    }
}
