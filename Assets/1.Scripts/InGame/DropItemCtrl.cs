using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject m_MainEffectObj = null;
    [SerializeField]
    private GameObject m_SubEffectObj = null;

  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {//�÷��̾�� �浹�� ��� ���� ������ �ִϸ��̼� �� �ΰ��� �Ŵ����� ����
         

            if (m_SubEffectObj != null)
            {
                this.GetComponent<Collider>().enabled = false;
                m_MainEffectObj.SetActive(false);
                m_SubEffectObj.SetActive(true);
                Debug.Log("Get Gold");
                SoundManager.GetInstance().PlayEffect("GetCoin");
                //InGameManager.GetInstance().GetGold();
                Destroy(this.gameObject, 1.0f);

                m_SubEffectObj = null;
            }
        }
       
           

    }
}
