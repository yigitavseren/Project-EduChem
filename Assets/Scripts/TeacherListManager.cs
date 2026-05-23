using UnityEngine;

public class TeacherListManager : MonoBehaviour
{
    [Header("UI Links")]
    public Transform contentContainer;      // ScrollView'un içindeki Content objesi
    public GameObject teacherRowPrefab;     // Üzerinde TeacherRowUI scripti olan gri kutu prefab'ı

    private TeacherManager teacherManager;

    void OnEnable()
    {
        if (teacherManager == null)
            teacherManager = FindObjectOfType<TeacherManager>(true);
            
        RefreshList();
    }

    /// <summary>
    /// Ekrandaki listeyi temizler ve aktif öğretmenleri yeniden çizer.
    /// GameManager EndDay sonrası veya Panel açıldığında çağrılır.
    /// </summary>
    public void RefreshList()
    {
        if (teacherManager == null || contentContainer == null || teacherRowPrefab == null)
        {
            Debug.LogWarning("[TeacherListManager] Eksik referanslar var. Listeyi çizemiyorum.");
            return;
        }

        // Önce ekrandaki eski öğretmen satırlarını yok et (Temizlik)
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        // Güncel aktif öğretmenleri (activeTeachers) baştan çiz
        foreach (Teacher t in teacherManager.activeTeachers)
        {
            GameObject newRow = Instantiate(teacherRowPrefab, contentContainer);
            
            TeacherRowUI rowUI = newRow.GetComponent<TeacherRowUI>();
            if (rowUI != null)
            {
                rowUI.SetData(t); // Datayı UI'a otonom olarak bas
            }
        }
    }
}
