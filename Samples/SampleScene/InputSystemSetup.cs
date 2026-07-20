using UnityEngine;

public class InputSystemSetup : MonoBehaviour
{
    [SerializeField] private GameObject LegacyEventSystemGameObject;
    
    private void Awake()
    {
#if !ENABLE_INPUT_SYSTEM
        // GameObject go = GameObject.Find("EventSystem(Legacy)");
        gameObject.SetActive(false);
        if (LegacyEventSystemGameObject != null)
        {
            LegacyEventSystemGameObject.SetActive(true);
        }
#endif
    }
}
