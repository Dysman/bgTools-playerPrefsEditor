using UnityEngine;

public class PlayerPrefsController : MonoBehaviour
{

    #region Add
    public void AddTestStrings()
    {
        PlayerPrefs.SetString("Runtime_String", "boing");
        PlayerPrefs.SetString("Runtime_String2", "foo");
    }

    public void AddTestInt()
    {
        PlayerPrefs.SetInt("Runtime_Int", 1234);
    }

    public void AddTestFloat()
    {
        PlayerPrefs.SetFloat("Runtime_Float", 3.14f);
    }
    #endregion

    #region Remove
    public void RemoveTestStrings()
    {
        PlayerPrefs.DeleteKey("Runtime_String");
        PlayerPrefs.DeleteKey("Runtime_String2");
    }

    public void RemoveTestInt()
    {
        PlayerPrefs.DeleteKey("Runtime_Int");
    }

    public void RemoveTestFloat()
    {
        PlayerPrefs.DeleteKey("Runtime_Float");
    }

    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }
    #endregion
}