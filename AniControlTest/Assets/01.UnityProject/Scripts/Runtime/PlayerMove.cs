using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private enum InputDirection
    {
        NONE, LEFT, RIGHT, FORWARD, BACK
    }
    private InputDirection inputDirection = InputDirection.NONE;    /** 플레이어가 입력받은 방향 */

    [Header("플레이어의 움직임 설정")]
    [Tooltip("플레이어의 이동속도를 조절하는 옵션")]
    [Range(0f, 10f)]
    public float moveSpeed = 0f;
    //private Coroutine moveHorizontal = default;
    //private Coroutine moveVertical = default;

    private Coroutine moveRoutine = default;    /** 플레이어를 움직이는 함수 */

    private Rigidbody rgbody = default;
    private Animator animator = default;

    private bool isMove = false;    /** 플레이어의 이동 상태 */
    private Vector3 moveDirection = Vector3.zero;   /** 플레이어가 이동할 방향 */
    private void Awake()
    {
        rgbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    #region 플레이어의 이동
    //! Horizontal axis 입력을 받기 위한 함수
    public void OnMoveHorizontal(InputAction.CallbackContext context)
    {
        Move_Player(context);
        //Cancel_HorizontalMove(context);
        Cancel_Moving(context, true);
    }       // OnMoveHorizontal()

    //! Vertical axis 입력을 받기 위한 함수
    public void OnMoveVertical(InputAction.CallbackContext context)
    {
        Move_Player(context);
        //Cancel_VerticalMove(context);
        Cancel_Moving(context, false);
    }       // OnMoveHorizontal()

    //! Horizontal, Vertical axis로 플레이어의 인게임 움직임을 결정하는 함수
    private void Move_Player(InputAction.CallbackContext context)
    {
        if (context.started == false) { return; }
        
        // 입력이 확인되면 플레이어를 움직인다.

        // 기존에 달리던 방향을 캔슬하고 새로운 방향으로 달린다.
        StopMove(ref moveRoutine);
        isMove = false;

        // 컨트롤러의 방향을 받아서 캐싱한다.
        Vector2 inputAxis = default;
        inputAxis = context.ReadValue<Vector2>();

        // 이동하려는 방향으로 회전한다.
        Rotate_Player(inputAxis, out moveDirection);

        // 플레이어를 이동한다.
        moveRoutine = StartCoroutine(DoMove());

        // 이동하는 애니메이션을 재생한다.
        isMove = true;
        animator.SetBool("IsMove", isMove);
    }   // OnMove()

    private void Cancel_Moving(InputAction.CallbackContext context, bool isHorizontal)
    {
        // 캔슬 이벤트가 아닌 경우 리턴한다.
        if (context.canceled == false) { return; }

        if(isHorizontal)
        {
            // 움직이는 방향이 Vertical인 경우 리턴한다.
            if (inputDirection.Equals(InputDirection.FORWARD) || inputDirection.Equals(InputDirection.BACK)) { return; }
        }   // if: 가로 입력을 받은 경우
        else
        {
            // 움직이는 방향이 Horizontal인 경우 리턴한다.
            if (inputDirection.Equals(InputDirection.LEFT) || inputDirection.Equals(InputDirection.RIGHT)) { return; }
        }   // else: 세로 입력을 받은 경우

        // 움직이는 방향과 캔슬하려는 방향이 일치하면 플레이어를 멈춘다.
        StopMove(ref moveRoutine);

        // 멈추는 애니메이션을 재생한다.
        isMove = false;
        animator.SetBool("IsMove", isMove);
    }   // Cancel_Moving()

    private IEnumerator DoMove()
    {
        Vector3 moveVelo = default;

        while (true)
        {
            // 키를 누르고 있는 동안 움직인다.
            // 자연스럽게 움직이기 위해서 임의의 보정값 100을 곱해준다.
            moveVelo = moveDirection * moveSpeed * 100f * Time.fixedDeltaTime;
            rgbody.velocity = moveVelo;

            yield return null;
        }   // loop: 움직이는 키 입력받는 동안 반복하는 루프
    }       // DoMove()

    private void StopMove(ref Coroutine moveRoutine)
    {
        if (moveRoutine == null) { return; }

        StopCoroutine(moveRoutine);
        // 자연스럽게 멈추기 위해서 움직이는 속도를 임의로 절반 감속한다.
        rgbody.velocity = rgbody.velocity * 0.5f;

        // 움직였던 방향을 정리한다.
        inputDirection = InputDirection.NONE;
        moveDirection = Vector3.zero;

        moveRoutine = null;
    }   // StopMove()
    #endregion  // 플레이어의 이동

    private void Rotate_Player(Vector2 inputAxis, out Vector3 moveDirection)
    {
        inputDirection = InputDirection.NONE;
        moveDirection = default;
        // 플레이어가 회전할 각도를 캐싱한다.
        float rotationAngleY = 0f;

        if (inputAxis.x < 0f)
        {
            inputDirection = InputDirection.LEFT;
            moveDirection = Vector3.left;
            rotationAngleY += 270f;
        }   // if: 왼쪽 방향인 경우
        else if (0f < inputAxis.x)
        {
            inputDirection = InputDirection.RIGHT;
            moveDirection = Vector3.right;
            rotationAngleY += 90f;
        }   // if: 오른쪽 방향인 경우
        else if (0f < inputAxis.y)
        {
            inputDirection = InputDirection.FORWARD;
            moveDirection = Vector3.forward;
            rotationAngleY = 0f;
        }   // if: 앞쪽 방향인 경우
        else if (inputAxis.y < 0f)
        {
            inputDirection = InputDirection.BACK;
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
