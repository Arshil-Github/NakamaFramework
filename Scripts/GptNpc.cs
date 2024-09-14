using System;
using System.Collections;
using System.Text;
using LMNT;
using UnityEngine;
using UnityEngine.Networking;

public class GptNpc : MonoBehaviour
{
    public enum RequestType
    {
        GET = 0,
        POST = 1
    }

    public enum RequestState
    {
        HasResponse,
        Thinking
    }

    public RequestState requestState;
    
    [Serializable]
    public class PostData
    {
        public string userMessage;
    }
    
    public class PostResult
    {
        public string name;
        public string response;
        public string[] options;
        public string animationName;
        public string currentLocation;
    }
    [SerializeReference]protected Animator animator;
    protected LMNTSpeech speech;
    
    void Start()
    {
        SendPlayerResponse("Introduce Yourself");
        speech = GetComponent<LMNTSpeech>();
    }

    public void SendPlayerResponse(string response, RoomTriggers.Room location = RoomTriggers.Room.OrbRoom)
    {
        StartCoroutine(MakeRequests(response + $". The player is currently in the room {location.ToString()}"));
        ChangeRequestState(RequestState.Thinking);
    }

    protected void ChangeRequestState(RequestState state)
    {
        requestState = state;

        if (requestState == RequestState.Thinking)
        {
            animator.SetBool("isThinking", true);
            GameManager.instance.GetDialogueUI().SetNpcDialogue("Thinking...");
        }
    }
    
    protected IEnumerator MakeRequests(string userResponse)
    {
        //POST
        var dataToPost = new PostData() { userMessage = userResponse };
        var postRequest = CreateRequests("http://localhost:3000/getResponse", RequestType.POST, dataToPost);

        yield return postRequest.SendWebRequest();
        ChangeRequestState(RequestState.HasResponse);
        
        var deserializedPostData = JsonUtility.FromJson<PostResult>(postRequest.downloadHandler.text);

        GameManager.instance.GetDialogueUI().SetNpcName(deserializedPostData.name);
        GameManager.instance.GetDialogueUI().SetNpcDialogue(deserializedPostData.response);
        
        animator.SetBool("isThinking", false);
        PlayAnimation(deserializedPostData.animationName);
        
        GameManager.instance.GetOptionsUI().SetOptions(deserializedPostData.options);

        speech.dialogue = deserializedPostData.response;
        StartCoroutine(speech.Talk());

        if (this is TourNpc)
        {
            ((TourNpc) this).SetLocation(deserializedPostData.currentLocation);
        }
    }

    protected UnityWebRequest CreateRequests(string path, RequestType type = RequestType.GET, object data = null)
    {
        var request = new UnityWebRequest(path, type.ToString());

        if (data != null)
        {
            var bodyRaw = Encoding.UTF8.GetBytes((JsonUtility.ToJson(data)));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }

    protected void PlayAnimation(string animationName)
    {
        animator.CrossFadeInFixedTime(animationName, 0.5f);
        Debug.Log(animationName);
    }
    //
    // private void ChangeTTS(string text)
    // {
    //     //Make a new LMNTSpeech object and attach it to the gameobject
    //     LMNTSpeech newSpeech = gameObject.AddComponent<LMNTSpeech>();
    //     newSpeech.voice = speech.voice;
    //     
    //     //Delete the old LMNTSpeech object
    //     Destroy(speech);
    //     //Destroy audio source
    //     Destroy(speech.GetComponent<AudioSource>());
    //     
    //     //Play the new dialogue
    //     newSpeech.dialogue = text;
    //     StartCoroutine(newSpeech.Talk());
    // }
}
