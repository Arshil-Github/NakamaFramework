using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private TextMeshProUGUI npcDialogue;
    
    public void SetNpcName(string name)
    {
        npcName.text = name;
    }
    public void SetNpcDialogue(string dialogue)
    {
        npcDialogue.text = dialogue;
    }
}