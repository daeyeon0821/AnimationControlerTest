using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushColliderObj : MonoBehaviour
{
    public enum ForceType
    {
        NONE, PUSH, KICK
    }

    public PlayerSkill playerSkill = default;   /** 플레이어 스킬과 상호작용하기 위한 변수 */
    private ForceType forceType = ForceType.NONE;   /** 오브젝트에 가할 힘의 종류를 결정하는 변수 */
    private IPushable pushableObj = default;

    private void OnTriggerStay(Collider other)
    {
        pushableObj = other.GetComponent<IPushable>();
        if (pushableObj == default) { return; }

        if (forceType.Equals(ForceType.PUSH))
        {
            playerSkill.OnPush(pushableObj);
        }
        else if (forceType.Equals(ForceType.KICK))
        {
            playerSkill.OnKick(pushableObj);
        }
        this.gameObject.SetActive(false);
        pushableObj = default;
    }   // OnTriggerStay()

    public void SetForceType(ForceType forceType_)
    {
        forceType = forceType_;
    }   // SetForceType()

    public void ChangeColliderActive(bool isActive, Vector3 pushPosition)
    {
        if(isActive == true)
        {
            this.transform.position = pushPosition;
            this.gameObject.SetActive(true);
        }   // if: Collider 사용할 때는 World Position을 사용한다.
        else
        {
            this.gameObject.SetActive(false);
            this.transform.localPosition = pushPosition;
            SetForceType(ForceType.NONE);
        }   // else: 기존 위치로 되돌릴 때는 Local Position을 사용한다.
    }   // ChangeColliderActive()
}
