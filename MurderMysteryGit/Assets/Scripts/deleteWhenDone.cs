using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class deleteWhenDone : MonoBehaviour
{
    public TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onSubmit.AddListener(Kill);
    }
    private void OnDestroy()
    {
        inputField.onSubmit.RemoveListener(Kill);
    }

    public void Kill(string s)
    {
        Destroy(gameObject);
    }

}
