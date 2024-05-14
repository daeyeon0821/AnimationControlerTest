using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkill : MonoBehaviour
{
    private Animator animator = default;
    private PlayerMove playerMove = default;

    // Skill 1의 옵션
    [Header("Skill 1의 옵션")]
    [Range(0f, 5f)]
    [Tooltip("Skill 1에서 밀기 애니메이션을 재생할 시간 (단위: 초)")]
    public float pushDurationSeconds = 0f;
    private WaitForSeconds pushDuration = default;
    [Range(0f, 1f)]
    [Tooltip("Skill 1에서 미는 힘")]
    public float pushPower = 0f;

    private bool isRunSkill_1 = false;

    private KickState kickState = default;
    private PushState pushState = default;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();

        // { Skill 1 초기화
        pushState = animator.GetBehaviour<PushState>();
        pushState.enterRoutine = Start_Skill_1;
        pushState.exitRoutine = () => { playerMove.Lock_PlayerMove(true); };
        kickState = animator.GetBehaviour<KickState>();
        kickState.exitRoutine = End_Skill_1;

        pushDuration = new WaitForSeconds(pushDurationSeconds);
        // } Skill 1 초기화
    }   // Awake()

    public void OnSkill_1(InputAction.CallbackContext context)
    {
        if (context.performed == false) { return; }
        // 스킬이 발동되는 동안 리턴한다.
        if (isRunSkill_1 == true) { return; }

        StartCoroutine(DoPushAni());
    }   // OnSkill_1()

    private IEnumerator DoPushAni()
    {
        animator.SetTrigger("Push");
        yield return pushDuration;
        animator.SetTrigger("Kick");
    }   // DoPushAni()

    //! Skill 1이 시작할 때 동작하는 함수
    private void Start_Skill_1()
    {
        isRunSkill_1 = true;
        playerMove.Lock_PlayerMove(true);
        playerMove.OnMove(pushPower);
    }   // Start_Skill_1()

    //! Skill 1이 끝날 때 동작하는 함수
    private void End_Skill_1()
    {
        playerMove.Lock_PlayerMove(false);
        isRunSkill_1 = false;
    }   // End_Skill_1()
}
