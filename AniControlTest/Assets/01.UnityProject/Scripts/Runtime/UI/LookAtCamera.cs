using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Vector3 rotationVector3 = default;

    private void Update()
    {
        // { 화면이 고정된 상태에서 카메라 바라보기
        transform.LookAt(Camera.main.transform);
        rotationVector3 = transform.eulerAngles;
        rotationVector3.x = rotationVector3.x + 105f;
        rotationVector3.y = 0;
        rotationVector3.z = 0;
        transform.eulerAngles = rotationVector3;
        // } 화면이 고정된 상태에서 카메라 바라보기
    }
}
