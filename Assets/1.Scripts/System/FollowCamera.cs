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
        var wantedRotationAngle = target.eulerAngles.y; //Ÿ���� ȸ�� ��
        var wantedHeight = target.position.y + height; //Ÿ�� ���� + ���ϴ� ����

        var currentRotationAngle = transform.eulerAngles.y; //���� �� ȸ�� ��
        var currentHeight = transform.position.y; //ī�޶� ��ġ

        // Damp the rotation around the y-axis //�� ȸ����--> Ÿ�� ȸ���� �ǵ��� 
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Damp the height //ī�޶� ����-> ��ǥ ����
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        transform.position = target.position; //�������� Ÿ�� ��ġ�� ����
        transform.position -= currentRotation * Vector3.forward * distance; //�׸��� ȸ�� �ϰ�, �� ȸ���Ŀ� �Ÿ��� �γ� 

        // Set the height of the camera
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z); //���̰� 

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
