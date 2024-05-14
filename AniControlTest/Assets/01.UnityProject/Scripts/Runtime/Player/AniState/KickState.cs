using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickState : StateMachineBehaviour
{
    public delegate void StateRoutine();
    public StateRoutine exitRoutine = default;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        exitRoutine?.Invoke();
    }
}
