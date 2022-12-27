using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextsCtrl : MonoBehaviour
{
    private const int MAX_INDEX = 50;

    private static DamageTextsCtrl instance;
    public static DamageTextsCtrl GetInstance()
    {
        if (instance == null)
            return null;
        return instance;
    }

    public GameObject DamageTextIns;    
    DamageText[] Texts;

    private int curIndex = 0;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Texts = new DamageText[MAX_INDEX];
        for (int i = 0; i < MAX_INDEX; i++)
        {
            GameObject temp = Instantiate(DamageTextIns);
            temp.transform.SetParent(this.gameObject.transform);
            Texts[i] = temp.transform.GetComponent<DamageText>();
        }

        //  Effects = this.transform.GetComponentsInChildren<DamageEffect>();
        //GameManager.GetInstance().ac_SceneChange += TextReset;
    }
    public void TextReset()
    {
        Debug.Log("DamageText Reset");
        for (int i = 0; i < MAX_INDEX; i++)
        {
            if (Texts[i].gameObject.activeSelf == true)
            {
                Texts[i].gameObject.SetActive(false);
            }
        }
    }


    public void OnDamageText(string a_dam, Vector3 a_pos, bool a_cri = false)
    {
        if (curIndex >= MAX_INDEX)
        {
            for (int i = 0; i < MAX_INDEX; i++)
            {
                if (Texts[i].gameObject.activeSelf == false)
                {
                    curIndex = i;
                    break;
                }
            }
        }
        Texts[curIndex].SetOnEffect(a_dam, a_pos, a_cri);
        curIndex++;
    }
}
