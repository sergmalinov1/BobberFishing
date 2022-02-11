using UnityEngine;

[CreateAssetMenu(menuName = "Language/Add word", fileName = "New Word")]
public class LanguageData : ScriptableObject
{
    [Tooltip("English")]
    [SerializeField] private string _English;

    [Tooltip("Russian")]
    [SerializeField] private string _Russian;

    [Tooltip("Ukrain")]
    [SerializeField] private string _Ukraine;

    public string English
    {
        get
        {
            return _English;
        }
    }

    public string Russian
    {
        get
        {
            return _Russian;
        }
    }


    public string Ukraine
    {
        get
        {
            return _Ukraine;
        }
    }

}
