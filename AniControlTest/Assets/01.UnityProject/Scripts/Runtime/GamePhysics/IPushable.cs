using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPushable
{
    //! 밀기 가능한 오브젝트를 미는 함수
    public void OnPush(Vector3 moveDirection, float moveSpeed, WaitForSeconds moveDuration);

    //! 밀기 가능한 오브젝트에 임팩트를 주는 함수
    public void OnImpact(Vector3 moveDirection, float power);

    //! GameObject를 리턴하는 함수
    public GameObject GetGameObject();
}
