using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CandidateManager : MonoBehaviour
{
    [Header("Core Links")]
    public GameManager gameManager;

    [Header("Candidate Info UI")]
    public TextMeshProUGUI Txt_Name;
    public TextMeshProUGUI Txt_Field;

    [Header("RPG Stats Texts (1-10)")]
    public TextMeshProUGUI Txt_Intelligence;
    public TextMeshProUGUI Txt_Physique;
    public TextMeshProUGUI Txt_Willpower;
    public TextMeshProUGUI Txt_Wealth;

    [Header("Survival Bars (Sliders 0-100)")]
    public Slider Slider_Energy;
    public Slider Slider_Sanity;
    public Slider Slider_Loyalty;
    public Slider Slider_Toxicity;

    private Student currentCandidate;

    public int candidatesLeftToday = 3;

    private void OnEnable()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
            
        // Panel her açıldığında otomatik 3 yeni aday hakkımız olsun
        candidatesLeftToday = 3;
        GenerateNewCandidate();
    }

    // Aday oluşturma ve değerleri (1-10 ve barlar) Student.cs üzerinden çekme
    public void GenerateNewCandidate()
    {
        int currentAcademySuccess = gameManager != null ? gameManager.successRate : 50;
        
        // Student scripti zaten constructor'ında statları ve barları istediğin gibi belirliyor.
        // (Loyalty 50, Sanity 100, Energy 100, Toxicity 0, Statlar 1-10)
        currentCandidate = new Student(currentAcademySuccess);

        // Verileri senin hazırladığın UI elemanlarına tıkır tıkır basalım
        UpdatePanel(currentCandidate);
    }

    // UI'a verileri basan fonksiyon
    public void UpdatePanel(Student s)
    {
        if (s == null) return;

        // İsim ve Alan (Eğer UI'da bunları göstereceğin Text'ler varsa atayabilirsin)
        if (Txt_Name != null) Txt_Name.text = s.studentName;
        if (Txt_Field != null) Txt_Field.text = "Field: " + s.field.ToString();

        // --- STATLAR (1-10) ---
        if (Txt_Intelligence != null) Txt_Intelligence.text = "Intelligence: " + s.intelligence.ToString();
        if (Txt_Physique != null) Txt_Physique.text = "Physique: " + s.physique.ToString();
        if (Txt_Willpower != null) Txt_Willpower.text = "Willpower: " + s.willpower.ToString();
        if (Txt_Wealth != null) Txt_Wealth.text = "Wealth: " + s.wealth.ToString();

        // --- BARLAR (0-100) ---
        if (Slider_Energy != null) 
        { 
            Slider_Energy.maxValue = 100; Slider_Energy.value = s.energy; 
            SetSliderColor(Slider_Energy, new Color(1f, 0.8f, 0f), new Color(0.2f, 0.2f, 0.2f)); // Sarı
        }
        if (Slider_Sanity != null) 
        { 
            Slider_Sanity.maxValue = 100; Slider_Sanity.value = s.sanity; 
            SetSliderColor(Slider_Sanity, new Color(0.2f, 0.6f, 1f), new Color(0.2f, 0.2f, 0.2f)); // Mavi
        }
        if (Slider_Loyalty != null) 
        { 
            Slider_Loyalty.maxValue = 100; Slider_Loyalty.value = s.loyalty; 
            SetSliderColor(Slider_Loyalty, Color.white, new Color(0.2f, 0.2f, 0.2f)); // Beyaz
        }
        if (Slider_Toxicity != null) 
        { 
            Slider_Toxicity.maxValue = 100; Slider_Toxicity.value = s.toxicity; 
            SetSliderColor(Slider_Toxicity, new Color(0.1f, 0.8f, 0.1f), new Color(0.2f, 0.2f, 0.2f)); // Yeşil
        }
    }

    // Slider renklerini otonom olarak ayarlayan yardımcı fonksiyon
    private void SetSliderColor(Slider slider, Color fillColor, Color bgColor)
    {
        if (slider == null) return;

        // Fill (Dolgu) Rengi
        if (slider.fillRect != null)
        {
            Image fillImage = slider.fillRect.GetComponent<Image>();
            if (fillImage != null) fillImage.color = fillColor;
        }

        // Background (Arka Plan) Rengi
        Transform bgTransform = slider.transform.Find("Background");
        if (bgTransform != null)
        {
            Image bgImage = bgTransform.GetComponent<Image>();
            if (bgImage != null) bgImage.color = bgColor;
        }
    }

    // Accept Butonuna tıklandığında çalışacak fonksiyon
    public void OnAcceptClicked()
    {
        if (currentCandidate != null && gameManager != null)
        {
            // Öğrenciyi dershaneye kaydet
            gameManager.enrolledStudents.Add(currentCandidate);
            gameManager.studentCount++;
            
            // Eğer öğrenci listesi açıksa onu da güncelle (Şimdilik GameManager üzerinden, daha sonra ListManager kullanacak)
            gameManager.UpdateStudentList(); 
        }
        
        candidatesLeftToday--;

        if (candidatesLeftToday > 0)
        {
            GenerateNewCandidate();
        }
        else
        {
            // Hak bittiyse paneli kapat
            gameObject.SetActive(false);
        }
    }

    // Reject Butonuna tıklandığında çalışacak fonksiyon
    public void OnRejectClicked()
    {
        candidatesLeftToday--;

        if (candidatesLeftToday > 0)
        {
            GenerateNewCandidate();
        }
        else
        {
            // Hak bittiyse paneli kapat
            gameObject.SetActive(false);
        }
    }
}
