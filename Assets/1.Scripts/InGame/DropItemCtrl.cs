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
        {//플레이어와 충돌할 경우 상자 열리는 애니메이션 및 인게임 매니저에 전달
         

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
