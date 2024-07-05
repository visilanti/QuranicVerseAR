using UnityEngine;
using TMPro;

public class PengamalanDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text pengamalanText; // Assign in Inspector

    void Start()
    {
        string generatedPengamalan = PlayerPrefs.GetString("GeneratedPengamalan", "No Pengamalan Found");
        pengamalanText.text = generatedPengamalan;
    }
}
