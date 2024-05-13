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

    #region 플레이어의 이동
    //! Horizontal axis 입력을 받기 위한 함수
    public void OnMoveHorizontal(InputAction.CallbackContext context)
    {
        Move_Player(context, ref moveHorizontal);
    }       // OnMoveHorizontal()

    //! Vertical axis 입력을 받기 위한 함수
    public void OnMoveVertical(InputAction.CallbackContext context)
    {
        Move_Player(context, ref moveVertical);
    }       // OnMoveHorizontal()

    //! Horizontal, Vertical axis로 플레이어의 인게임 움직임을 결정하는 함수
    private void Move_Player(InputAction.CallbackContext context, ref Coroutine moveRoutine)
    {
        if (context.started == true)
        {
            StopMove(moveRoutine);

            // 컨트롤러의 방향을 받아서 캐싱한다.
            Vector2 inputAxis = default;
            inputAxis = context.ReadValue<Vector2>();

            // 인게임에서 어느 방향으로 이동할지 캐싱한다.
            Vector3 moveDirection = default;

            // 이동하려는 방향으로 회전한다.
            Rotate_Player(inputAxis, out moveDirection);

            // 플레이어를 이동한다.
            moveRoutine = StartCoroutine(DoMove(moveDirection));
        }   // if: 입력이 확인되면 움직이기 시작한다.
        else if (context.canceled == true)
        {
            StopMove(moveRoutine);
        }   // if: 입력이 종료되면 멈춘다.
    }   // OnMove()

    private IEnumerator DoMove(Vector3 moveDirection)
    {
        Vector3 moveVelo = default;

        while (true)
        {
            // 키를 누르고 있는 동안 움직인다.
            // 자연스럽게 움직이기 위해서 임의의 보정값 100을 곱해준다.
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
    #endregion  // 플레이어의 이동

    private void Rotate_Player(Vector2 inputAxis, out Vector3 moveDirection)
    {
        moveDirection = default;
        // 플레이어가 회전할 각도를 캐싱한다.
        float rotationAngleY = 0f;

        if (inputAxis.x < 0f)
        {
            moveDirection = Vector3.left;
            rotationAngleY += 270f;
        }   // if: 왼쪽 방향인 경우
        else if (0f < inputAxis.x)
        {
            moveDirection = Vector3.right;
            rotationAngleY += 90f;
        }   // if: 오른쪽 방향인 경우
        else if (0f < inputAxis.y)
        {
            moveDirection = Vector3.forward;
            rotationAngleY = 0f;
        }   // if: 앞쪽 방향인 경우
        else if (inputAxis.y < 0f)
        {
            moveDirection = Vector3.back;
            rotationAngleY += 180f;
        }   // if: 뒤쪽 방향인 경우

        // 플레이어가 회전할 각에서 현재 바라보는 각을 뺀다.
        // 인게임에서 바라볼 방향을 연산한다.
        Vector3 rotationEuler = Vector3.zero;
        rotationEuler.y = rotationAngleY - transform.rotation.eulerAngles.y;

        //// DEBUG:
        //Debug.LogFormat("플레이어가 바라볼 방향, 회전 각도: {0}, {1}", moveDirection, rotationAngleY);
        //Debug.LogFormat("플레이어의 Rotation, 회전해야 할 각도: {0}, {1}",
        //    transform.rotation.eulerAngles, rotationEuler);

        transform.Rotate(rotationEuler);
    }   // Rotate_Player()

    public void OnJump(InputAction.CallbackContext context)
    {
        // TODO: performed 상태에서만 동작하도록 구현할 것.
        Debug.Log("Call Jump !!!");
    }
}
