using UnityEngine;

public class PlayerPrefsController : MonoBehaviour
{

    #region Add
    public void AddTestStrings()
    {
        PlayerPrefs.SetString("String", "boing");
        PlayerPrefs.SetString("String2", "foo");
        //PlayerPrefs.Save();
    }

    public void AddTestInt()
    {
        PlayerPrefs.SetInt("Int", 1234);
        //PlayerPrefs.Save();
    }

    public void AddTestFloat()
    {
        PlayerPrefs.SetFloat("Float", 3.14f);
        //PlayerPrefs.Save();
    }
    #endregion

    #region Remove
    public void RemoveTestStrings()
    {
        PlayerPrefs.DeleteKey("String");
        PlayerPrefs.DeleteKey("String2");
        //PlayerPrefs.Save();
    }

    public void RemoveTestInt()
    {
        PlayerPrefs.DeleteKey("Int");
        //PlayerPrefs.Save();
    }

    public void RemoveTestFloat()
    {
        PlayerPrefs.DeleteKey("Float");
        //PlayerPrefs.Save();
    }

    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
    }
    #endregion
}