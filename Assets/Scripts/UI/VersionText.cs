using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class VersionText : MonoBehaviour
    {
        void Awake()
        {
            GetComponent<TMP_Text>().text = "v. " + Application.version;
        }
    }
}
