using UnityEngine;

public enum TeacherBranch
{
    Matematik,
    Fizik,
    Kimya,
    Biyoloji,
    Edebiyat,
    Tarih,
    Cografya,
    Felsefe
}

[System.Serializable]
public class Teacher
{
    public string teacherName = "New Teacher";
    public TeacherBranch branch;

    [Header("RPG Stats (1-10)")]
    public int educationEfficiency; // Eğitim Verimi
    public int authority;           // Otorite
    public int observation;         // Gözlem
    public int salaryCost;          // Maaş Maliyeti / Kaşe

    [Header("Dynamic Bars (0-100)")]
    public float energy = 100f;     // Başlangıç: 100
    public float burnout = 0f;      // Başlangıç: 0
    public float loyalty = 50f;     // Başlangıç: 50
    public float suspicion = 0f;    // Başlangıç: 0

    // Sadece isim ataması yapıyoruz, statlar artık TeacherGenerator tarafından atanacak
    public Teacher(string name)
    {
        this.teacherName = name;
    }
}
