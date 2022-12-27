using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Npc : MonoBehaviour
{
    public GameObject m_SubCanvasObj = null;
    public GameObject m_SubCamera = null;

    public Button m_btn_Close = null;


    // Start is called before the first frame update
    void Start()
    {
        if (m_btn_Close != null)
            m_btn_Close.onClick.AddListener(OnClickClose);

    }


    private void OnClickClose()
    {
        m_SubCamera.SetActive(false);
        m_SubCanvasObj.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            //SubCamera On
            m_SubCamera.SetActive(true);
            m_SubCanvasObj.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            m_SubCamera.SetActive(false);
            m_SubCanvasObj.SetActive(false);
        }

    }
}
