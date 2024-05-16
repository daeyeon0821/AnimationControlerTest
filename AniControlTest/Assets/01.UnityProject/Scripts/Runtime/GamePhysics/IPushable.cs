using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPushable
{
    //! 밀기 가능한 오브젝트를 미는 함수
    public void OnPush(Vector3 moveDirection, float moveSpeed, WaitForSeconds moveDuration);

    //! GameObject를 리턴하는 함수
    public GameObject GetGameObject();
}
