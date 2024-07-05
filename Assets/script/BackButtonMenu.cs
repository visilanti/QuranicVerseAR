using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackToDetailScriptMenu : MonoBehaviour
{
    void Start()
    {
        // Mengambil referensi dari button di UI
        Button backButton = GetComponent<Button>();

        // Menambahkan event listener untuk onClick
        backButton.onClick.AddListener(BackToDetail);
    }

    void BackToDetail()
    {
        // Menggunakan SceneManager untuk kembali ke scene 'DetailAyah'
        SceneManager.LoadScene("MainMenu");
    }
}
