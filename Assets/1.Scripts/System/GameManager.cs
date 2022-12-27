using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public partial class GameManager : MonoBehaviour
{
    private static GameManager m_instance = null;

    public static GameManager GetInstance()
    {
        return m_instance;
    }


    private readonly string TITLE = "1.Title";
    private readonly string VILLAGE = "2.Village";
    private readonly string UI = "3.UI";
    private readonly string INGAME = "4.InGame";

    private string m_ActiveScene = "";
    private void Awake()
    {
        if (m_instance != null)
            Destroy(m_instance);

        m_instance = this;

        DontDestroyOnLoad(this);
    }




    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        m_ActiveScene = TITLE;
 
    }



    public void ChangeScene(string _scene)
    {
        if (_scene != TITLE && _scene != INGAME && _scene != UI && _scene != VILLAGE)
            return;

        if (m_isChangeScene)
            return;

        if (m_ActiveScene == _scene) //현재 신과 같으면
            return;

        Debug.Log("ChangeScene " + _scene);
   
        StartCoroutine(ChangeSceneCo(_scene));


    }

    private bool m_isChangeScene = false; //씬 변경중 재요청 들어올 경우 대비
    private IEnumerator ChangeSceneCo(string _scene)
    {
        LoadingObjectActive(true);
        m_isChangeScene = true;
        yield return new WaitForSeconds(0.2f);      
        AsyncOperation async;
        var _progressGage = 0.0f;
        

        if (m_ActiveScene == TITLE) { //Title에서 넘어올경우 
            async = SceneManager.LoadSceneAsync(_scene,LoadSceneMode.Single);
            UpdateGage(_progressGage);
            async.allowSceneActivation = false;

            while (!async.isDone)
            {
                _progressGage = async.progress / 2;
                UpdateGage(_progressGage);

                if (async.progress >= 0.9f)
                    async.allowSceneActivation = true;

                yield return new WaitForFixedUpdate();
            }

            while (_progressGage < 0.5f)
            {
                UpdateGage(_progressGage);
                yield return new WaitForFixedUpdate();
                _progressGage += 0.01f;
            }
            UpdateGage(_progressGage);
            yield return new WaitForSeconds(0.2f);

            async = SceneManager.LoadSceneAsync(UI, LoadSceneMode.Additive); //추가
            async.allowSceneActivation = false;

            while (!async.isDone)
            {
                _progressGage =0.5f+ async.progress / 2;
                UpdateGage(_progressGage);

                if (async.progress >= 0.9f)
                    async.allowSceneActivation = true;

                yield return new WaitForFixedUpdate();
            }
      
            while (_progressGage < 1.0f) {
                UpdateGage(_progressGage);
                yield return new WaitForFixedUpdate();
                _progressGage+= 0.01f;
            }
            
            yield return new WaitForSeconds(0.2f);
            UpdateGage(1);
            LoadingObjectActive(false);
            m_ActiveScene = _scene;
            m_isChangeScene = false;
            yield break;
        }

        //-------Next Step

        //처음 이후 
     
        SceneManager.UnloadSceneAsync(m_ActiveScene);//기존씬 제거              
        
        async = SceneManager.LoadSceneAsync(_scene, LoadSceneMode.Additive); //추가

        async.allowSceneActivation = false;

        while (!async.isDone)
        {         
            UpdateGage(async.progress);

            if (async.progress >= 0.9f)
                async.allowSceneActivation = true;

            yield return new WaitForFixedUpdate();
        }
        _progressGage = 0.9f;
        while (_progressGage < 1.0f)
        {
            UpdateGage(_progressGage);
            yield return new WaitForFixedUpdate();
            _progressGage += 0.01f;
        }
        UpdateGage(1.0f);

        yield return new WaitForSeconds(0.2f);
        m_isChangeScene = false;
        m_ActiveScene = _scene;
        LoadingObjectActive(false);
    }


    [Header("Loading"), SerializeField]
    private GameObject m_LoadingObj = null;
    [SerializeField]
    private Image m_img_Gage = null;
    [SerializeField]
    private Text m_txt_Gage = null;
    //<--- --->
    public void UpdateGage(float _gage)
    {
        m_img_Gage.fillAmount = _gage;
        m_txt_Gage.text = string.Format("{0}%", (_gage * 100).ToString("N2"));
    }

    private void LoadingObjectActive(bool _isOn)
    {
        m_LoadingObj.SetActive(_isOn);
    }

    [SerializeField]
    private ToastMsg[] m_ToastMsgs = null;
    private const int MAX_MSG = 10;


    private int m_ToastMsgIndex = 0;
    public void SetMessage(string _msg, bool _accent = false)
    {

        if (m_ToastMsgs[m_ToastMsgIndex].gameObject.activeSelf)
        {
            for (int i = 0; i < MAX_MSG; i++)
            {
                if (!m_ToastMsgs[i].gameObject.activeSelf)
                {
                    m_ToastMsgIndex = i;
                    break;
                }
            }
        }        

        m_ToastMsgs[m_ToastMsgIndex].SetMessage(_msg, _accent);
        m_ToastMsgIndex++;
        if (m_ToastMsgIndex < MAX_MSG)
            m_ToastMsgIndex = 0;
    }
         

}
