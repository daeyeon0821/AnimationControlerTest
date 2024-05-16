using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushColliderObj : MonoBehaviour
{
    public PlayerSkill playerSkill = default;   /** 플레이어 스킬과 상호작용하기 위한 변수 */
    private IPushable pushableObj = default;

    private void OnTriggerStay(Collider other)
    {
        pushableObj = other.GetComponent<IPushable>();
        if (pushableObj == default) { return; }

       
        playerSkill.OnPush(pushableObj);
        this.gameObject.SetActive(false);
        pushableObj = default;
    }   // OnTriggerStay()
}
