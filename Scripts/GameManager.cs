using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private DialogueUI _dialogueUI;
    [SerializeField] private OptionsUI _optionsUI;

    private void Start()
    {
        ShowDialogueUI(true);
        ShowOptionsUI(false);
    }
    private void Awake()
    {
        instance = this;
    }
    
    public DialogueUI GetDialogueUI()
    {
        return _dialogueUI;
        
    }
    public void SwitchConversationStateUI(Player.ConversationState toState)
    {
        switch (toState)
        {
            case Player.ConversationState.Listen:
                ShowDialogueUI(true);
                ShowOptionsUI(false);
                break;
            case Player.ConversationState.Speak:
                ShowDialogueUI(false);
                ShowOptionsUI(true);
                break;
            /*case Player.ConversationState.Idle:
                ShowDialogueUI(false);
                ShowOptionsUI(false);
                break;*/
                
        }
    }
    public void ShowDialogueUI(bool show)
    {
        _dialogueUI.gameObject.SetActive(show);
    }
    public void ShowOptionsUI(bool show)
    {
        _optionsUI.gameObject.SetActive(show);
    }
    public OptionsUI GetOptionsUI()
    {
        return _optionsUI;
    }

}