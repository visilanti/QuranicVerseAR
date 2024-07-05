using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class Pengamalan : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ayahText;
    [SerializeField] private TextMeshProUGUI translationText;
    [SerializeField] private TextMeshProUGUI tafsirText;
    [SerializeField] private TextMeshProUGUI pengamalanText; // UI Text for displaying generated pengamalan
    [SerializeField] private Button pengamalanButton; // Button to navigate to PengamalanDetail
    [SerializeField] private Button backButton; // Button to navigate back to MainMenu

    void Start()
    {
        int selectedAyahId = PlayerPrefs.GetInt("SelectedAyahId");
        string selectedSurah = PlayerPrefs.GetString("SelectedSurah");
        string selectedVerseNumber = PlayerPrefs.GetString("SelectedVerseNumber");

        Debug.Log("Selected Ayah ID: " + selectedAyahId);
        Debug.Log("Selected Surah: " + selectedSurah);
        Debug.Log("Selected Verse Number: " + selectedVerseNumber);

        StartCoroutine(GetAyahDetail(selectedAyahId));

        // Add listener to pengamalanButton
        pengamalanButton.onClick.AddListener(NavigateToPengamalanDetail);

        // Add listener to backButton
        backButton.onClick.AddListener(NavigateToMainMenu);
    }

    IEnumerator GetAyahDetail(int ayahId)
    {
        string url = "http://135.181.26.148:7025/api/detail-ayah?id=" + ayahId;
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            AyahResponse ayahResponse = JsonUtility.FromJson<AyahResponse>(jsonResponse);

            if (ayahResponse != null && ayahResponse.message == "Success")
            {
                ayahText.text = ayahResponse.ayah.ayah;
                translationText.text = ayahResponse.ayah.translation;
                tafsirText.text = ayahResponse.ayah.tafsir;

                // Generate pengamalan automatically
                StartCoroutine(GeneratePengamalan(ayahResponse.ayah.translation, ayahResponse.ayah.tafsir));
            }
            else
            {
                Debug.LogError("Failed to fetch ayah details or unexpected response format.");
            }
        }
    }

    IEnumerator GeneratePengamalan(string translation, string tafsir)
    {
        string url = "http://135.181.26.148:7025/api/generate-pengamalan";

        // Prepare the data to be sent
        PengamalanRequest requestData = new PengamalanRequest
        {
            translation = translation,
            tafsir = tafsir
        };

        string jsonData = JsonUtility.ToJson(requestData);

        // Send the request
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            PengamalanResponse response = JsonUtility.FromJson<PengamalanResponse>(jsonResponse);

            if (response != null && response.message == "Success")
            {
                // Save the generated pengamalan to PlayerPrefs
                PlayerPrefs.SetString("GeneratedPengamalan", response.generated_pengamalan);
                PlayerPrefs.Save();
            }
            else
            {
                Debug.LogError("Failed to generate pengamalan or unexpected response format.");
            }
        }
    }

    void NavigateToPengamalanDetail()
    {
        SceneManager.LoadScene("Pengamalan");
    }

    void NavigateToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    [System.Serializable]
    public class Ayah
    {
        public string ayah;
        public int id;
        public string tafsir;
        public string translation;
    }

    [System.Serializable]
    public class AyahResponse
    {
        public Ayah ayah;
        public string message;
    }

    [System.Serializable]
    public class PengamalanRequest
    {
        public string translation;
        public string tafsir;
    }

    [System.Serializable]
    public class PengamalanResponse
    {
        public string generated_pengamalan;
        public string message;
    }
}


// using UnityEngine;
// using UnityEngine.Networking;
// using UnityEngine.UI;
// using TMPro;
// using System.Collections;
// using UnityEngine.SceneManagement;

// public class Pengamalan : MonoBehaviour
// {
//     [SerializeField] private TextMeshProUGUI ayahText;
//     [SerializeField] private TextMeshProUGUI translationText;
//     [SerializeField] private TextMeshProUGUI tafsirText;
//     [SerializeField] private TextMeshProUGUI pengamalanText; // UI Text for displaying generated pengamalan

//     void Start()
//     {
//         int selectedAyahId = PlayerPrefs.GetInt("SelectedAyahId");
//         string selectedSurah = PlayerPrefs.GetString("SelectedSurah");
//         string selectedVerseNumber = PlayerPrefs.GetString("SelectedVerseNumber");

//         Debug.Log("Selected Ayah ID: " + selectedAyahId);
//         Debug.Log("Selected Surah: " + selectedSurah);
//         Debug.Log("Selected Verse Number: " + selectedVerseNumber);

//         StartCoroutine(GetAyahDetail(selectedAyahId));
//     }

//     IEnumerator GetAyahDetail(int ayahId)
//     {
//         string url = "http://135.181.26.148:7025/api/detail-ayah?id=" + ayahId;
//         UnityWebRequest request = UnityWebRequest.Get(url);
//         yield return request.SendWebRequest();

//         if (request.result != UnityWebRequest.Result.Success)
//         {
//             Debug.LogError("Error: " + request.error);
//         }
//         else
//         {
//             string jsonResponse = request.downloadHandler.text;
//             AyahResponse ayahResponse = JsonUtility.FromJson<AyahResponse>(jsonResponse);

//             if (ayahResponse != null && ayahResponse.message == "Success")
//             {
//                 ayahText.text = ayahResponse.ayah.ayah;
//                 translationText.text = ayahResponse.ayah.translation;
//                 tafsirText.text = ayahResponse.ayah.tafsir;

//                 // Generate pengamalan automatically
//                 StartCoroutine(GeneratePengamalan(ayahResponse.ayah.translation, ayahResponse.ayah.tafsir));
//             }
//             else
//             {
//                 Debug.LogError("Failed to fetch ayah details or unexpected response format.");
//             }
//         }
//     }

//     IEnumerator GeneratePengamalan(string translation, string tafsir)
//     {
//         string url = "http://135.181.26.148:7025/api/generate-pengamalan";

//         // Prepare the data to be sent
//         PengamalanRequest requestData = new PengamalanRequest
//         {
//             translation = translation,
//             tafsir = tafsir
//         };

//         string jsonData = JsonUtility.ToJson(requestData);

//         // Send the request
//         UnityWebRequest request = new UnityWebRequest(url, "POST");
//         byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
//         request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//         request.downloadHandler = new DownloadHandlerBuffer();
//         request.SetRequestHeader("Content-Type", "application/json");

//         yield return request.SendWebRequest();

//         if (request.result != UnityWebRequest.Result.Success)
//         {
//             Debug.LogError("Error: " + request.error);
//         }
//         else
//         {
//             string jsonResponse = request.downloadHandler.text;
//             PengamalanResponse response = JsonUtility.FromJson<PengamalanResponse>(jsonResponse);

//             if (response != null && response.message == "Success")
//             {
//                 // Save the generated pengamalan to PlayerPrefs
//                 PlayerPrefs.SetString("GeneratedPengamalan", response.generated_pengamalan);
//                 PlayerPrefs.Save();

//                 // Load the new scene
//                 SceneManager.LoadScene("PengamalanDetail"); // Ensure this scene is added in the Build Settings
//             }
//             else
//             {
//                 Debug.LogError("Failed to generate pengamalan or unexpected response format.");
//             }
//         }
//     }

//     [System.Serializable]
//     public class Ayah
//     {
//         public string ayah;
//         public int id;
//         public string tafsir;
//         public string translation;
//     }

//     [System.Serializable]
//     public class AyahResponse
//     {
//         public Ayah ayah;
//         public string message;
//     }

//     [System.Serializable]
//     public class PengamalanRequest
//     {
//         public string translation;
//         public string tafsir;
//     }

//     [System.Serializable]
//     public class PengamalanResponse
//     {
//         public string generated_pengamalan;
//         public string message;
//     }
// }


// using UnityEngine;
// using UnityEngine.Networking;
// using UnityEngine.UI;
// using TMPro;
// using System.Collections;

// public class Pengamalan : MonoBehaviour
// {
//     [SerializeField] private TextMeshProUGUI ayahText;
//     [SerializeField] private TextMeshProUGUI translationText;
//     [SerializeField] private TextMeshProUGUI tafsirText;
//     [SerializeField] private TextMeshProUGUI pengamalanText; // UI Text for displaying generated pengamalan

//     void Start()
//     {
//         int selectedAyahId = PlayerPrefs.GetInt("SelectedAyahId");
//         string selectedSurah = PlayerPrefs.GetString("SelectedSurah");
//         string selectedVerseNumber = PlayerPrefs.GetString("SelectedVerseNumber");

//         Debug.Log("Selected Ayah ID: " + selectedAyahId);
//         Debug.Log("Selected Surah: " + selectedSurah);
//         Debug.Log("Selected Verse Number: " + selectedVerseNumber);

//         StartCoroutine(GetAyahDetail(selectedAyahId));
//     }

//     IEnumerator GetAyahDetail(int ayahId)
//     {
//         string url = "http://135.181.26.148:7025/api/detail-ayah?id=" + ayahId;
//         UnityWebRequest request = UnityWebRequest.Get(url);
//         yield return request.SendWebRequest();

//         if (request.result != UnityWebRequest.Result.Success)
//         {
//             Debug.LogError("Error: " + request.error);
//         }
//         else
//         {
//             string jsonResponse = request.downloadHandler.text;
//             AyahResponse ayahResponse = JsonUtility.FromJson<AyahResponse>(jsonResponse);

//             if (ayahResponse != null && ayahResponse.message == "Success")
//             {
//                 ayahText.text = ayahResponse.ayah.ayah;
//                 translationText.text = ayahResponse.ayah.translation;
//                 tafsirText.text = ayahResponse.ayah.tafsir;

//                 // Generate pengamalan automatically
//                 StartCoroutine(GeneratePengamalan(ayahResponse.ayah.translation, ayahResponse.ayah.tafsir));
//             }
//             else
//             {
//                 Debug.LogError("Failed to fetch ayah details or unexpected response format.");
//             }
//         }
//     }

//     IEnumerator GeneratePengamalan(string translation, string tafsir)
//     {
//         string url = "http://135.181.26.148:7025/api/generate-pengamalan";

//         // Prepare the data to be sent
//         PengamalanRequest requestData = new PengamalanRequest
//         {
//             translation = translation,
//             tafsir = tafsir
//         };

//         string jsonData = JsonUtility.ToJson(requestData);

//         // Send the request
//         UnityWebRequest request = new UnityWebRequest(url, "POST");
//         byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
//         request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//         request.downloadHandler = new DownloadHandlerBuffer();
//         request.SetRequestHeader("Content-Type", "application/json");

//         yield return request.SendWebRequest();

//         if (request.result != UnityWebRequest.Result.Success)
//         {
//             Debug.LogError("Error: " + request.error);
//         }
//         else
//         {
//             string jsonResponse = request.downloadHandler.text;
//             PengamalanResponse response = JsonUtility.FromJson<PengamalanResponse>(jsonResponse);

//             if (response != null && response.message == "Success")
//             {
//                 pengamalanText.text = response.generated_pengamalan;
//             }
//             else
//             {
//                 Debug.LogError("Failed to generate pengamalan or unexpected response format.");
//             }
//         }
//     }

//     [System.Serializable]
//     public class Ayah
//     {
//         public string ayah;
//         public int id;
//         public string tafsir;
//         public string translation;
//     }

//     [System.Serializable]
//     public class AyahResponse
//     {
//         public Ayah ayah;
//         public string message;
//     }

//     [System.Serializable]
//     public class PengamalanRequest
//     {
//         public string translation;
//         public string tafsir;
//     }

//     [System.Serializable]
//     public class PengamalanResponse
//     {
//         public string generated_pengamalan;
//         public string message;
//     }
// }

