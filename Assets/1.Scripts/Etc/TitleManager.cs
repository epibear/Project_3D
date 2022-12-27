using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TitleManager : MonoBehaviour
{

    public Button m_btn_Start = null;

    // Start is called before the first frame update
    void Start()
    {
        m_btn_Start.onClick.AddListener(() =>
        {
            GameManager.GetInstance().ChangeScene("2.Village");

        });
    }

    
}
