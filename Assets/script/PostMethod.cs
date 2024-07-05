using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
 
public class PostMethod : MonoBehaviour
{
    TMP_Text outputArea;
 
    void Start()
    {
        outputArea = GameObject.Find("OutputArea").GetComponent<TMP_Text>();
        GameObject.Find("PostButton").GetComponent<Button>().onClick.AddListener(PostData);
    }
 
    void PostData() => StartCoroutine(PostData_Coroutine());
 
    IEnumerator PostData_Coroutine()
    {
        outputArea.text = "Loading...";
        string uri = "http://135.181.26.148:7025/api/list-ayah-by-page?page=1";
        WWWForm form = new WWWForm();
        form.AddField("title", "test data");
        using(UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                outputArea.text = request.error;
            else
                outputArea.text = request.downloadHandler.text;
        }
    }
}