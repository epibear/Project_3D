using UnityEngine;
using UnityEngine.SceneManagement;


public class ForcedStart : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void FirstLoad()
    {
        if (SceneManager.GetActiveScene().name.CompareTo("1.Title") != 0)
            SceneManager.LoadScene("1.Title");
    }
}