using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBoxCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {//�÷��̾�� �浹�� ��� ���� ������ �ִϸ��̼� �� �ΰ��� �Ŵ����� ����
            this.GetComponent<Collider>().enabled = false;

            this.GetComponent<Animator>().SetTrigger("isOpen");
            InGameManager.GetInstance().GetDropBox();
        }      
    }

    public void EndOfOpenMotion() {
        this.gameObject.SetActive(false);
    }


    public void Drop() {
        this.gameObject.SetActive(true);
        this.GetComponent<Collider>().enabled = true;
    }
}
