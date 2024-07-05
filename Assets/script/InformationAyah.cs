using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class InformationAyah : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    [Serializable]
    public struct Game
    {
        public int id; // Assuming you also need id from API
        public string verse_number;
    }

    [SerializeField] string apiUrlTemplate = "http://135.181.26.148:7025/api/list-ayah-by-page?page=:page_id";
    [SerializeField] GameObject buttonTemplate; // Assign in Inspector

    void Start()
    {
        int pageId = PlayerPrefs.GetInt("PassedValue", 1); // Default to 1 if not set
        string apiUrl = apiUrlTemplate.Replace(":page_id", pageId.ToString());

        StartCoroutine(FetchDataAndPopulate(apiUrl));
    }

    IEnumerator FetchDataAndPopulate(string apiUrl)
    {
        // Fetch data from API
        using (var www = new WWW(apiUrl))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("Failed to fetch data: " + www.error);
                yield break;
            }

            // Parse JSON response
            string json = www.text;
            var responseData = JsonUtility.FromJson<ResponseData>(json);

            // Instantiate buttons
            foreach (var item in responseData.ayah)
            {
                GameObject g = Instantiate(buttonTemplate, transform); // Instantiate buttonTemplate
                g.transform.GetChild(0).GetComponent<TMP_Text>().text = responseData.surah;
                g.transform.GetChild(1).GetComponent<TMP_Text>().text = item.verse_number;

                int index = item.id - 1; // Adjust index if necessary
                g.GetComponent<Button>().onClick.AddListener(() => ItemClicked(item.id, responseData.surah, item.verse_number));
            }

            Destroy(buttonTemplate); // Destroy the template after instantiating buttons
        }
    }

    void ItemClicked(int id, string surah, string verseNumber)
    {
        Debug.Log("------------item " + id + " clicked---------------");
        Debug.Log("Surah: " + surah);
        Debug.Log("Ayah: " + verseNumber);

        PlayerPrefs.SetInt("SelectedAyahId", id);
        PlayerPrefs.SetString("SelectedSurah", surah);
        PlayerPrefs.SetString("SelectedVerseNumber", verseNumber);

        SceneManager.LoadScene("DetailAyah"); // Replace with your scene name
    }

    [Serializable]
    public class Ayah
    {
        public int id;
        public string verse_number;
    }

    [Serializable]
    public class ResponseData
    {
        public List<Ayah> ayah;
        public string surah;
    }
}



// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using TMPro;

// public class InformationAyah : MonoBehaviour
// {
//     [Serializable]
//     public struct Game
//     {
//         public int id; // Assuming you also need id from API
//         public string verse_number;
//     }

//     [SerializeField] string apiUrl = "http://135.181.26.148:7025/api/list-ayah-by-page?page=:page_id";
//     [SerializeField] GameObject buttonTemplate; // Assign in Inspector

//     void Start()
//     {
//         StartCoroutine(FetchDataAndPopulate());
//     }

//     IEnumerator FetchDataAndPopulate()
//     {
//         // Fetch data from API
//         using (var www = new WWW(apiUrl))
//         {
//             yield return www;
//             if (!string.IsNullOrEmpty(www.error))
//             {
//                 Debug.LogError("Failed to fetch data: " + www.error);
//                 yield break;
//             }

//             // Parse JSON response
//             string json = www.text;
//             var responseData = JsonUtility.FromJson<ResponseData>(json);

//             // Instantiate buttons
//             foreach (var item in responseData.ayah)
//             {
//                 GameObject g = Instantiate(buttonTemplate, transform); // Instantiate buttonTemplate
//                 g.transform.GetChild(0).GetComponent<TMP_Text>().text = responseData.surah;
//                 g.transform.GetChild(1).GetComponent<TMP_Text>().text = item.verse_number;

//                 int index = item.id - 1; // Adjust index if necessary
//                 g.GetComponent<Button>().onClick.AddListener(() => ItemClicked(index));
//             }

//             Destroy(buttonTemplate); // Destroy the template after instantiating buttons
//         }
//     }

//     void ItemClicked(int itemIndex)
//     {
//         Debug.Log("------------item " + itemIndex + " clicked---------------");
//         Debug.Log("Surah " + transform.GetChild(itemIndex).GetChild(0).GetComponent<TMP_Text>().text);
//         Debug.Log("Ayah " + transform.GetChild(itemIndex).GetChild(1).GetComponent<TMP_Text>().text);

//         PlayerPrefs.SetString("SelectedSurah", transform.GetChild(itemIndex).GetChild(0).GetComponent<TMP_Text>().text);
//         PlayerPrefs.SetString("SelectedVerseNumber", transform.GetChild(itemIndex).GetChild(1).GetComponent<TMP_Text>().text);

//         SceneManager.LoadScene("Pengamalan"); // Replace with your scene name
//     }

//     [Serializable]
//     public class Ayah
//     {
//         public int id;
//         public string verse_number;
//     }

//     [Serializable]
//     public class ResponseData
//     {
//         public List<Ayah> ayah;
//         public string surah;
//     }
// }
