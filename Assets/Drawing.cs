using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ErikaArcher player = animator.GetComponent<ErikaArcher>();
        //player.DrawArrow();
    }

}
