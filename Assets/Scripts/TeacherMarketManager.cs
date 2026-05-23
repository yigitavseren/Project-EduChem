using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TeacherMarketManager : MonoBehaviour
{
    [Header("Sistem Bağlantıları")]
    public TeacherGenerator teacherGenerator;
    public TeacherManager teacherManager;

    [Header("Aday Arayüzü (UI)")]
    public TextMeshProUGUI Txt_Name;
    public TextMeshProUGUI Txt_SalaryCost;

    [Header("RPG Statları")]
    public TextMeshProUGUI Txt_Education;
    public TextMeshProUGUI Txt_Authority;
    public TextMeshProUGUI Txt_Observation;

    [Header("Kontroller")]
    public TextMeshProUGUI Txt_CandidatesLeft;

    private List<Teacher> currentMarketCandidates = new List<Teacher>();

    void OnEnable()
    {
        if (teacherGenerator == null) teacherGenerator = FindObjectOfType<TeacherGenerator>();
        if (teacherManager == null) teacherManager = FindObjectOfType<TeacherManager>();

        RefreshMarket();
    }

    /// <summary>
    /// Piyasadan 3 yeni hoca bulur.
    /// </summary>
    public void RefreshMarket()
    {
        // GameManager'daki Başarı Oranını şimdilik Repütasyon (Ün) olarak kullanıyoruz.
        GameManager gm = FindObjectOfType<GameManager>();
        int currentRep = gm != null ? gm.successRate : 50; 

        // Beyin avcısına (TeacherGenerator) 3 aday ürettir
        currentMarketCandidates = teacherGenerator.GenerateTeacherCandidates(currentRep, false);
        
        ShowNextCandidate();
    }

    private void ShowNextCandidate()
    {
        if (currentMarketCandidates.Count > 0)
        {
            Teacher t = currentMarketCandidates[0]; // Kuyruktaki ilk adayı getir

            if (Txt_Name != null) Txt_Name.text = t.teacherName + " (" + t.branch.ToString() + ")";
            if (Txt_SalaryCost != null) Txt_SalaryCost.text = "Kaşe (Maaş): " + t.salaryCost + " /Gün";
            if (Txt_Education != null) Txt_Education.text = "Eğitim Verimi: " + t.educationEfficiency;
            if (Txt_Authority != null) Txt_Authority.text = "Otorite: " + t.authority;
            if (Txt_Observation != null) Txt_Observation.text = "Gözlem: " + t.observation;

            if (Txt_CandidatesLeft != null) Txt_CandidatesLeft.text = "Kalan Aday: " + currentMarketCandidates.Count;
        }
        else
        {
            // İncelenecek hoca kalmadıysa paneli kapat
            gameObject.SetActive(false);
        }
    }

    // "İşe Al" butonuna tıklanınca çağrılır
    public void OnHireClicked()
    {
        if (currentMarketCandidates.Count > 0)
        {
            Teacher hiredTeacher = currentMarketCandidates[0];
            
            // Hocayı aktif kadroya ekle
            teacherManager.activeTeachers.Add(hiredTeacher);
            
            // Sıradan çıkar
            currentMarketCandidates.RemoveAt(0);

            // Eğer Personel Listesi o an ekranda açıksa anında güncellensin
            TeacherListManager listManager = FindObjectOfType<TeacherListManager>(true);
            if (listManager != null && listManager.gameObject.activeInHierarchy)
            {
                listManager.RefreshList();
            }

            ShowNextCandidate(); // Sıradaki hocayı ekrana getir
        }
    }

    // "Reddet" butonuna tıklanınca çağrılır
    public void OnRejectClicked()
    {
        if (currentMarketCandidates.Count > 0)
        {
            currentMarketCandidates.RemoveAt(0);
            ShowNextCandidate();
        }
    }
}
