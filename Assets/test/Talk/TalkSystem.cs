using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TalkSystem : MonoBehaviour
{
    private Dictionary<int, string> m_Talks = new Dictionary<int, string>(); // 리스트

    [SerializeField,Header("대화 내용")]
    private string[] m_TalkData = null; //대화 내용

    [SerializeField]
    private Text m_txt_Talk = null;
    [SerializeField]
    private GameObject m_TalkObj = null;

    [SerializeField, Header("글자 팝업 시간")]
    private float m_termTime = 0.2f;

    private bool m_isSkip = false;

    [SerializeField]
    private Button m_btn_Skip = null;
    // Start is called before the first frame update
    void Awake()
    {
        GenerateTalk();

        if (m_btn_Skip != null)
            m_btn_Skip.onClick.AddListener(OnClickSkip);
    }
    private void OnClickSkip() {
        m_isSkip = true;
    }

    private void GenerateTalk() {
        var count = 0;
        foreach(string _data in m_TalkData){
            m_Talks.Add(count, _data);
            count++;
        }      
    }

    public char GetTalk(int _code, int _talkIndex) {
        return m_Talks[_code][_talkIndex];
    }

    private int m_perviousCode = 0;
    public void StartTalk(int _code) {
        StopCoroutine(TalkCo(m_perviousCode));
        StartCoroutine(TalkCo(_code));
        m_perviousCode = _code;
    }

    private int m_talkIndex = 0; //
    IEnumerator TalkCo(int _code) {
        m_talkIndex = 0;
        m_txt_Talk.text = "";
        while (!m_isSkip) {

            m_txt_Talk.text += GetTalk(_code,m_talkIndex);

            m_talkIndex++;
            if (m_talkIndex >= m_Talks[_code].Length)
                m_isSkip = true;

            yield return new WaitForSeconds(m_termTime);
        }

        m_txt_Talk.text = m_Talks[_code].ToString();

         yield return null;
        m_isSkip = false;        m_talkIndex = 0;
    }
}
