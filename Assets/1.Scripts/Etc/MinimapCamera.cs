using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField]
    private Transform m_targetTr = null;

    private Camera m_Camera = null;
    // Start is called before the first frame update
    void Start()
    {
        if (m_targetTr == null)
            m_targetTr = GameObject.Find("Player").transform;

        m_Camera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (m_targetTr == null)
        {
            FindTarget();
            return;
        }
        this.transform.position = new Vector3(m_targetTr.position.x, 10, m_targetTr.position.z);
    }
    
    private void FindTarget()
    {
        var player = GameObject.Find("Player");
        if (player == null)
        {
            Invoke("FindTarget", 1.0f);
            return;
        }
        m_targetTr = player.transform;
    }

    //Max Size
    private const int MAXSIZE = 14;
    private const int MINSIZE = 6;

    private int m_CurSize = 10;
    public void ChangSize(int _size) {
        m_CurSize += _size;
        if (m_CurSize > MAXSIZE) {

            m_CurSize = MAXSIZE;
            GameManager.GetInstance().SetMessage("최대 크기입니다");
        }
           

        if (m_CurSize < MINSIZE)
        {
            m_CurSize = MINSIZE;
            GameManager.GetInstance().SetMessage("최소 크기입니다");
        }
      

        m_Camera.orthographicSize = m_CurSize;
    }
}
