using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    public Transform target;
    // The distance in the x-z plane to the target
    [SerializeField]
    private float distance = 10.0f;
    // the height we want the camera to be above the target
    [SerializeField]
    private float height = 5.0f;

    [SerializeField]
    private float rotationDamping = 15.0f;
    [SerializeField]
    private float heightDamping = 2.0f;


    // Update is called once per frame
    void LateUpdate()
    {
        // Early out if we don't have a target
        if (!target)
            return;

        // Calculate the current rotation angles
        var wantedRotationAngle = target.eulerAngles.y; //타겟의 회전 값
        var wantedHeight = target.position.y + height; //타겟 높이 + 원하는 높이

        var currentRotationAngle = transform.eulerAngles.y; //현재 내 회전 값
        var currentHeight = transform.position.y; //카메라 위치

        // Damp the rotation around the y-axis //내 회전값--> 타겟 회전값 되도록 
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Damp the height //카메라 높이-> 목표 높이
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        transform.position = target.position; //포지션을 타겟 위치로 변경
        transform.position -= currentRotation * Vector3.forward * distance; //그리고 회전 하고, 아 회전후에 거리를 두네 

        // Set the height of the camera
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z); //높이값 

        // Always look at the target
        transform.LookAt(target);

        if (isShake)
        {
            if (m_shakeTime > 0)
            {
                transform.position = (Vector3)Random.insideUnitCircle * m_shakePower + this.transform.position;
                m_shakeTime -= Time.deltaTime;
            }
            else
                isShake = false;
        }
    }

    private float m_shakeTime = 2f;
    private float m_shakePower = 0.15f;
    private bool isShake = false;
    public void ShakeCam(float _time, float _power=0.15f)
        {
            m_shakeTime = _time;
            m_shakePower = _power;
            isShake = true;
        }
   



}
