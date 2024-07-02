using UnityEngine;

public class DebugShortcuts : MonoBehaviour
{
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GlobalEvents.Rescatter?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            GlobalEvents.RunKmeans?.Invoke();
        }
    }
}
