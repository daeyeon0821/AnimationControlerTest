using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickState : StateMachineBehaviour
{
    public delegate void StateRoutine();
    public StateRoutine exitRoutine = default;

    public StateRoutine kickRoutine = default;
    public bool isKickRoutine = false;
    private int targetFrame = default;
    private int currentFrame = default;
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (isKickRoutine == false) { return; }

        targetFrame = Mathf.RoundToInt(14f / 47f * 100);
        currentFrame = Mathf.RoundToInt(stateInfo.normalizedTime * 100);

        if (targetFrame.Equals(currentFrame) == false) { return; }

        //// DEBUG:
        //Debug.LogFormat("Call Kick updated | target: {0}, current: {1}", targetFrame, currentFrame);

        // 원하는 프레임에 함수를 실행한다.
        kickRoutine?.Invoke();
        isKickRoutine = false;
    }   // OnStateUpdate()

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        exitRoutine?.Invoke();
    }
}
