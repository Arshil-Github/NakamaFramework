using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    //Function to pass 1 to 4 into the GptNpc
    [SerializeField] private GptNpc currentNpc;

    public enum ConversationState
    {
        Listen,
        Speak
    }

    public ConversationState conversationState = ConversationState.Listen;
    public RoomTriggers.Room currentRoom = RoomTriggers.Room.OrbRoom;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }

    private void Update()
    {
        switch (conversationState)
        {
            case ConversationState.Listen:
                ListenUpdate();
                break;
            case ConversationState.Speak:
                SpeakUpdate();
                break;
        }   
    }

    private void ListenUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Switch to speak state
            GameManager.instance.SwitchConversationStateUI(ConversationState.Speak);
            conversationState = ConversationState.Speak;
        }
    }

    private void SpeakUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentNpc.SendPlayerResponse("1");
            GameManager.instance.SwitchConversationStateUI(ConversationState.Listen);
            conversationState = ConversationState.Listen;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentNpc.SendPlayerResponse("2");
            GameManager.instance.SwitchConversationStateUI(ConversationState.Listen);
            conversationState = ConversationState.Listen;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentNpc.SendPlayerResponse("3");
            GameManager.instance.SwitchConversationStateUI(ConversationState.Listen);
            conversationState = ConversationState.Listen;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentNpc.SendPlayerResponse("4");
            GameManager.instance.SwitchConversationStateUI(ConversationState.Listen);
            conversationState = ConversationState.Listen;
        }
    }
    
    public void SetCurrentRoom(RoomTriggers.Room room)
    {
        currentRoom = room;
    }
    
    /*private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if(other.TryGetComponent(out GptNpc npc))
        {
            currentNpc = npc;
            GameManager.instance.SwitchConversationStateUI(ConversationState.Listen);
            conversationState = ConversationState.Listen;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out GptNpc npc))
        {
            currentNpc = null;
            GameManager.instance.SwitchConversationStateUI(ConversationState.Idle);
            conversationState = ConversationState.Idle;
        }
    }*/
}