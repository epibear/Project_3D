using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class InGameManager : MonoBehaviour
{
    private static InGameManager m_instance = null;

    public static InGameManager GetInstance() {
        return m_instance;
    }

    private void Awake()
    {
        if (m_instance != null)
            Destroy(m_instance);

        m_instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.GetInstance().PlayBgm("Battle");
    }


    public void ChangeCharacterUI() { 
    
    }

    public void ChangeWeaponUI()
    {

    }
    public void GetGoldByMonster(int _gold) { 
    
    }
    public void GetDropBox() {

        Debug.Log("Drop Box is Get");
    }
    public GameObject m_DropBoxObj = null;
    public GameObject m_DropGoldObj = null;
    public void DropItem(Vector3 _pos) {
        var _dirvec = _pos - PlayerCtrl.GetInstance().transform.position;
        var inst = Instantiate(m_DropGoldObj, _pos, Quaternion.identity);
       // inst.GetComponent<DropBoxCtrl>().Drop();
        //inst.transform.LookAt(_dirvec);
    }
}

