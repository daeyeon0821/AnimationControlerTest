using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("플레이어의 움직임 설정")]
    [Tooltip("플레이어의 이동속도를 조절하는 옵션")]
    [Range(0f, 10f)]
    public float moveSpeed = 0f;
    private Coroutine moveHorizontal = default;
    private Coroutine moveVertical = default;

    private Rigidbody rigidbody = default;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void OnMoveHorizontal(InputAction.CallbackContext context)
    {
        if(context.started == true)
        {
            StopMove(moveHorizontal);
            moveHorizontal = StartCoroutine(DoMove(context));
        }   // if: 입력이 확인되면 움직이기 시작한다.
        else if (context.canceled == true)
        {
            StopMove(moveHorizontal);
        }   // if: 입력이 종료되면 멈춘다.
    }       // OnMoveHorizontal()

    public void OnMoveVertical(InputAction.CallbackContext context)
    {
        if (context.started == true)
        {
            StopMove(moveVertical);
            moveVertical = StartCoroutine(DoMove(context));
        }   // if: 입력이 확인되면 움직이기 시작한다.
        else if (context.canceled == true)
        {
            StopMove(moveVertical);
        }   // if: 입력이 종료되면 멈춘다.
    }       // OnMoveHorizontal()

    private IEnumerator DoMove(InputAction.CallbackContext context)
    {
        Vector2 inputAxis = default;

        Vector3 moveDirection = default;
        Vector3 moveVelo = default;

        // 컨트롤러의 방향을 받아서 캐싱한다.
        inputAxis = context.ReadValue<Vector2>();
        if (inputAxis.x < 0f)
        {
            moveDirection = Vector3.left;
        }   // if: 왼쪽 방향인 경우
        else if (0f < inputAxis.x)
        {
            moveDirection = Vector3.right;
        }   // if: 오른쪽 방향인 경우
        else if (0f < inputAxis.y)
        {
            moveDirection = Vector3.forward;
        }   // if: 앞쪽 방향인 경우
        else if (inputAxis.y < 0f)
        {
            moveDirection = Vector3.back;
        }   // if: 뒤쪽 방향인 경우

        while (true)
        {
            // 키를 누르고 있는 동안 움직인다.
            moveVelo = moveDirection * moveSpeed * 100f * Time.fixedDeltaTime;
            rigidbody.velocity = moveVelo;

            yield return null;
        }   // loop: 움직이는 키 입력받는 동안 반복하는 루프
    }       // DoMove()

    private void StopMove(Coroutine moveRoutine)
    {
        if (moveRoutine == null) { return; }

        StopCoroutine(moveRoutine);
        // 자연스럽게 멈추기 위해서 움직이는 속도를 임의로 절반 감속한다.
        rigidbody.velocity = rigidbody.velocity * 0.5f;
        moveRoutine = null;
    }   // StopMove()

    public void OnJump(InputAction.CallbackContext context)
    {
        // TODO: performed 상태에서만 동작하도록 구현할 것.
        Debug.Log("Call Jump !!!");
    }
}
