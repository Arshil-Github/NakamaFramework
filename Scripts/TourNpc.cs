using System;
using System.Collections;
using LMNT;
using UnityEngine;

public class TourNpc : GptNpc
{
    [Serializable]
    public class SetupData
    {
        public string targetLocation;
    }


    private string currentLocation = "";

    public Transform[] locationTransforms;
    void Awake()
    {
        speech = GetComponent<LMNTSpeech>();
        
        SetupData setupData = new SetupData()
        {
            targetLocation = "OrbRoom"
        };
        StartCoroutine(CustomSetup(setupData));
    }
    protected IEnumerator CustomSetup(SetupData setupToPost)
    {
        //POST
        var postRequest = CreateRequests("http://localhost:3000/setLocationInfo", RequestType.POST, setupToPost);
        yield return postRequest.SendWebRequest();
        
        Debug.Log("AI Setup  Complete");
    }
    
    public void SetLocation(string location)
    {
        if (currentLocation != location)
        {
            Debug.Log("Setting location to " + location);

            foreach (Transform locTransform in locationTransforms)
            {
                if (locTransform.gameObject.name == location)
                {
                    transform.position = locTransform.position;
                    transform.rotation = locTransform.rotation;
                    break;
                }
            }
            
            currentLocation = location;
        }
    }
}