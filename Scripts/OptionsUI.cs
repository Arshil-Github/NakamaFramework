using TMPro;
using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] options;
    
    public void SetOptions(string[] options)
    {
        for (int i = 0; i < options.Length; i++)
        {
            this.options[i].text = options[i];
        }
    }
}
