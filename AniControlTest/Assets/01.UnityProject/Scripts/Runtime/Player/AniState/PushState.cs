using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushState : StateMachineBehaviour
{
    public delegate void StateRoutine();
    public StateRoutine enterRoutine = default;
    public StateRoutine exitRoutine = default;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        enterRoutine?.Invoke();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        exitRoutine?.Invoke();
    }
}
