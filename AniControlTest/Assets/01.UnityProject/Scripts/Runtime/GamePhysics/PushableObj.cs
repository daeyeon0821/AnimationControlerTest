using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObj : MonoBehaviour, IPushable
{
    private Rigidbody rgbody = default;
    private Coroutine moveRoutine = default;

    private void Awake()
    {
        rgbody = GetComponent<Rigidbody>();
    }

    public void OnPush(Vector3 moveDirection, float moveSpeed, WaitForSeconds moveDuration)
    {
        moveRoutine = StartCoroutine(DoMove(moveDirection, moveSpeed));
        StartCoroutine(StopMove(moveDuration));
    }   // OnPush()

    public void OnImpact(Vector3 moveDirection, float power)
    {
        Vector3 forceVector = moveDirection * power;
        // 움직이기 전에 Kinematic 옵션을 끈다.
        rgbody.isKinematic = false;
        rgbody.AddForce(forceVector, ForceMode.Impulse);

        // 일정 시간 이후에 Kinematic 옵션을 끈다.
        StartCoroutine(StopMove(new WaitForSeconds(0.5f)));
    }   // OnImpact()

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    private IEnumerator DoMove(Vector3 moveDirection_, float moveSpeed_)
    {
        Vector3 moveVelo = default;

        // 움직이기 전에 Kinematic 옵션을 끈다.
        rgbody.isKinematic = false;

        while (true)
        {
            // 키를 누르고 있는 동안 움직인다.
            // 자연스럽게 움직이기 위해서 임의의 보정값 100을 곱해준다.
            moveVelo = moveDirection_ * moveSpeed_ * 100f * Time.fixedDeltaTime;
            rgbody.velocity = moveVelo;

            yield return null;
        }   // loop: 움직이는 키 입력받는 동안 반복하는 루프
    }       // DoMove()

    private IEnumerator StopMove(WaitForSeconds stopDelay)
    {
        yield return stopDelay;
        if (moveRoutine != default)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = default;

        }
        rgbody.isKinematic = true;
    }   // StopMove()
}
