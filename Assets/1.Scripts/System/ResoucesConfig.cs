using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MongDo/Resource Config", fileName = "Resouces.asset")]
public class ResoucesConfig : ScriptableObject
{
    public Material[] PlayerMaterial;

    public Material GetMaterial(int _idx)
    {  
        return PlayerMaterial[_idx];      
    }

    public GameObject[] PlayerWeapon;
    public GameObject GetWeapon(int _idx)
    {
        return PlayerWeapon[_idx];
    }

    public Sprite[] QuestIcon;
    public Sprite GetQuestIconSprite(int _idx)
    {
        return QuestIcon[_idx];
    }


    public Sprite[] Skills;
    public Sprite GetSkillSprite(int _idx)
    {
        return Skills[_idx];
    }
}
