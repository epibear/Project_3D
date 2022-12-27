using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{  
    public GameObject m_obj_CriDam = null;
    public GameObject m_obj_NormalDam = null;

    public TextMeshProUGUI m_txt_Critical = null;
    public TextMeshProUGUI m_txt_Base = null;

    // Start is called before the first frame update

    public void SetOnEffect(string a_dam, Vector3 a_pos, bool a_Cri = false)
    {
        this.transform.position = a_pos;
        this.gameObject.SetActive(true);
        if (a_Cri)
        {
            m_obj_NormalDam.SetActive(false);
            m_obj_CriDam.SetActive(true);

            m_txt_Critical.text = string.Format("Critical!\n-{0}", a_dam);
        }
        else
        {
            m_obj_CriDam.SetActive(false);
            m_obj_NormalDam.SetActive(true);
            m_txt_Base.text = string.Format("-{0}", a_dam);
        }
        Invoke("SetOffEffect", 0.35f);
    }

    private void SetOffEffect()
    {
        this.gameObject.SetActive(false);
    }


}
