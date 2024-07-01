using UnityEngine;

public class DebugShortcuts : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            FindObjectOfType<Generator>().ScatterActiveObjects();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            FindObjectOfType<HerdManager>().ForceUpdateClusters();
        }
    }
}
