using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class QuranAPIManager : MonoBehaviour
{
    private string apiUrl = "http://135.181.26.148:7025/api/"; // Ganti dengan URL API Anda

    public IEnumerator GetSurahData(int surahNumber)
    {
        string url = $"{apiUrl}/list-ayah-by-page?page=/{surahNumber}";
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse JSON dan tampilkan data
            string jsonResponse = request.downloadHandler.text;
            // Parse dan gunakan data
        }
        else
        {
            Debug.LogError(request.error);
        }
    }
}


// using UnityEngine;
// using UnityEngine.Networking;
// using UnityEngine.UI;
// using System.Collections;

// public class QuranAPIManager : MonoBehaviour
// {
//     public InputField OutputArea;

//     void Start()
//     {
//         OutputArea = GameObject.Find("OutputArea").GetComponent<InputField>();

//         if (OutputArea == null)
//         {
//             Debug.LogError("OutputArea not found or does not have InputField component.");
//             return;
//         }

//         int receivedValue = PlayerPrefs.GetInt("PassedValue", -1);
//         if (receivedValue != -1)
//         {
//             StartCoroutine(GetDataFromAPI(receivedValue));
//         }
//     }

//     IEnumerator GetDataFromAPI(int page_id)
//     {
//         OutputArea.text = "Loading....";
//         string url = "http://135.181.26.148:7025/api/list-ayah-by-page?page=" + page_id;
//         UnityWebRequest request = UnityWebRequest.Get(url);
//         yield return request.SendWebRequest();

//         if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
//         {
//             Debug.LogError(request.error);
//             OutputArea.text = "Error: " + request.error;
//         }
//         else
//         {
//             OutputArea.text = request.downloadHandler.text;
//         }
//     }
// }
