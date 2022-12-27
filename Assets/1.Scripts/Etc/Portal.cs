using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private string Destination = "";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.GetInstance().ChangeScene(Destination);
            this.GetComponent<SphereCollider>().enabled = false;
        }
    }
}
