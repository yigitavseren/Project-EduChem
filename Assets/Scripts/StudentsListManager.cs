using UnityEngine;

public class StudentsListManager : MonoBehaviour
{
    [Header("Core Links")]
    public GameManager gameManager;
    
    [Header("UI Links")]
    public Transform contentContainer;      // Scroll View içindeki 'Content' objesi
    public GameObject studentRowPrefab;     // Çoğaltacağımız Öğrenci Satırı (Üzerinde StudentRowUI scripti olmalı)

    // Panel her açıldığında (örneğin butonla) listeyi tazeleyelim
    private void OnEnable()
    {
        RefreshList();
    }

    public void RefreshList()
    {
        // GameManager atanmamışsa bulmaya çalış
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null || contentContainer == null || studentRowPrefab == null)
        {
            Debug.LogWarning("[StudentsListManager] Eksik referanslar var. Lütfen Inspector'dan GameManager, Content ve Prefab atamalarını yapın.");
            return;
        }

        // 1. Önce ekrandaki eski listeyi tamamen temizle (Çift yazmayı önlemek için)
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        // 2. Dershaneye kayıtlı olan (enrolledStudents) her bir öğrenci için yeni satır oluştur
        foreach (Student s in gameManager.enrolledStudents)
        {
            // Şablonu Content'in içine klonla
            GameObject newRow = Instantiate(studentRowPrefab, contentContainer);
            
            // Klonun üzerindeki StudentRowUI scriptini al ve verileri bas
            StudentRowUI rowUI = newRow.GetComponent<StudentRowUI>();
            if (rowUI != null)
            {
                rowUI.Setup(s);
            }
            else
            {
                Debug.LogWarning("[StudentsListManager] StudentRowPrefab üzerinde 'StudentRowUI' scripti bulunamadı!");
            }
        }
    }
}
