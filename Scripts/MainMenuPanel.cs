using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Button _startButton;
    
    public class CharacterDefinePostData
    {
        public string character;
    }
    
    public void StartGame()
    {
        string characterName = _inputField.text;
        //check if inputfield is empty
        if (characterName == "")
        {
            characterName = "Tony Stark";
        }


        StartCoroutine(MakeRequests(characterName));
        _startButton.interactable = false;
        
    }
    
    protected IEnumerator MakeRequests(string characterName)
    {
        //POST
        var dataToPost = new CharacterDefinePostData() { character = characterName};
        var postRequest = CreateRequests("http://localhost:3000/defineCharacter", GptNpc.RequestType.POST, dataToPost);

        yield return postRequest.SendWebRequest();
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        
        _startButton.interactable = true;
    }

    protected UnityWebRequest CreateRequests(string path, GptNpc.RequestType type = GptNpc.RequestType.GET, object data = null)
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
}
