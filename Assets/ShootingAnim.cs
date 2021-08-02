using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAnim : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ErikaArcher player = animator.GetComponent<ErikaArcher>();
        player.shoot = false;
    }
}
