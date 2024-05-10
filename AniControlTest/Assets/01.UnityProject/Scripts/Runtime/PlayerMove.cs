using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("플레이어의 움직임 설정")]
    [Range(0f, 10f)]
    public float moveSpeed = 0f;
    private Vector3 tempPosition = default;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            tempPosition = this.transform.position;
            tempPosition.x = tempPosition.x + (-1 * moveSpeed * Time.deltaTime);
            this.transform.position = tempPosition;
        }       // if: 왼쪽 키를 누른 경우
        else if (Input.GetKey(KeyCode.D))
        {
            tempPosition = this.transform.position;
            tempPosition.x = tempPosition.x + (moveSpeed * Time.deltaTime);
            this.transform.position = tempPosition;
        }       // if: 오른쪽 키를 누른 경우
    }       // Update()
}
