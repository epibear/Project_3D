using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamVer2 : MonoBehaviour
{
    public Transform m_targetTr = null;

    [SerializeField]
    private float extra_Y = 7f;
    [SerializeField]
    private float extra_Z = 4f;

    [SerializeField]
    private float RotateX = 60.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.rotation = Quaternion.Euler(RotateX, 0, 0);

        if (m_targetTr == null)
            m_targetTr = GameObject.Find("Player").transform;
  
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (m_targetTr == null)
        {
            FindTarget();
            return;
        }


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
        else
            this.transform.position = new Vector3(m_targetTr.position.x, extra_Y, m_targetTr.position.z - extra_Z);
    }

    private float m_shakeTime = 2f;
    private float m_shakePower = 0.15f;
    private bool isShake = false;
    public void ShakeCam(float _time, float _power = 0.15f)
    {
        m_shakeTime = _time;
        m_shakePower = _power;
        isShake = true;
    }


    private void FindTarget()
    {
        var player = GameObject.Find("Player");
        if (player == null) {
            Invoke("FindTarget", 1.0f);
            return;
        }         
        m_targetTr = player.transform;
    }

}
