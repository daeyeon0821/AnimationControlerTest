using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [Tooltip("Skill 1에서 미는 속도")]
    public float pushSpeed = 0f;
    [Header("Skill 1 발동을 위한 변수")]
    [Tooltip("Skill 1에서 손으로 밀어줄 콜라이더")]
    public PushColliderObj pushCollider = default;
    [Tooltip("Skill 1에서 콜라이더가 이동할 위치")]
    public GameObject pushPosition = default;

    private PushState pushState = default;
    private KickState kickState = default;

    private bool isRunSkill_1 = false;  /** 스킬1이 동작하는 상태 */
    private IPushable pushableObj = default;   /** 밀 수 있는 오브젝트 */
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();

        // { Skill 1 초기화
        pushState = animator.GetBehaviour<PushState>();
        pushState.enterRoutine = Start_Skill_1;
        pushState.exitRoutine = End_PushState;
        kickState = animator.GetBehaviour<KickState>();
        kickState.exitRoutine = End_Skill_1;

        pushDuration = new WaitForSeconds(pushDurationSeconds);
        pushCollider.gameObject.SetActive(false);
        pushCollider.transform.localPosition = Vector3.zero;
        pushPosition.SetActive(false);
        // } Skill 1 초기화
    }   // Awake()

    public void OnSkill_1(InputAction.CallbackContext context)
    {
        if (context.performed == false) { return; }
        // 스킬이 발동되는 동안 리턴한다.
        if (isRunSkill_1 == true) { return; }

        StartCoroutine(DoPushAni());

        // 밀기 콜라이더를 손으로 이동한다.
        // 밀 수 있는 오브젝트를 콜라이더로 감지한다.
        pushCollider.transform.position = pushPosition.transform.position;
        pushCollider.gameObject.SetActive(true);
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
        // Push 하는 동안 Move 애니메이션을 재생하지 않고 플레이어를 이동한다.
        playerMove.OnMove(pushSpeed);
    }   // Start_Skill_1()

    //! 밀기 동작이 끝났을 때 동작하는 함수
    private void End_PushState()
    {
        playerMove.Lock_PlayerMove(true);
        // 밀기 콜라이더를 제자리로 되돌린다.
        pushCollider.gameObject.SetActive(false);
        pushCollider.transform.localPosition = Vector3.zero;
    }   // End_PushState()

    //! Skill 1이 끝날 때 동작하는 함수
    private void End_Skill_1()
    {
        playerMove.Lock_PlayerMove(false);
        isRunSkill_1 = false;
        // 발차기가 끝난 후 Idle 애니메이션을 재생한다.
        playerMove.Play_MoveAni(false);
    }   // End_Skill_1()

    //! 밀 수 있는 오브젝트를 민다.
    public void OnPush(IPushable target)
    {
        pushableObj = target;
        if (pushableObj == default) { return; }

        //// DEBUG:
        //Debug.LogFormat("밀기 가능한 오브젝트 입니다. {0}", target.GetGameObject().name);

        pushableObj.OnPush(playerMove.MoveDirection, pushSpeed, pushDuration);
        pushableObj = default;
    }   // OnPush()
}
