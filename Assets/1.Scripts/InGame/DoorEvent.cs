using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEvent : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Door = null;
    [SerializeField]
    private GameObject m_EventCamera = null;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;
        Debug.Log("BossRoom Open" + other.name);
        m_EventCamera.SetActive(true);
        MainUIManager.GetInstance().OffMainCanvas();
        GameManager.GetInstance().StartEvent();

        this.GetComponent<BoxCollider>().enabled=false;
        StartCoroutine(DrawDoorCo());
    }

    // z -1.8f
    IEnumerator DrawDoorCo() {
        yield return new WaitForSeconds(0.5f);
        Camera.main.GetComponent<FollowCamVer2>().ShakeCam(0.5f, 0.3f);
        yield return new WaitForSeconds(0.3f);

        while (m_Door.transform.localPosition.z < 1.9f) {
            m_Door.transform.localPosition += Vector3.forward * 0.02f;
            yield return new WaitForFixedUpdate(); //60Frame
        }

        m_EventCamera.SetActive(false);
        MainUIManager.GetInstance().OnMainCanvas();
        GameManager.GetInstance().EndEvent();
        yield return null;
    }
}
