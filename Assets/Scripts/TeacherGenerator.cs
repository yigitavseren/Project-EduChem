using System.Collections.Generic;
using UnityEngine;

public class TeacherGenerator : MonoBehaviour
{
    [Header("Generator Data")]
    private string[] firstNames = { "Walter", "Jesse", "Skyler", "Hank", "Marie", "Saul", "Gus", "Mike", "Charles", "Jane", "Hector", "Todd" };
    private string[] lastNames = { "White", "Pinkman", "Lambert", "Schrader", "Goodman", "Fring", "Ehrmantraut", "McGill", "Margolis", "Salamanca", "Alquist" };

    /// <summary>
    /// Çağrıldığında 3 adet rastgele öğretmen adayı üretir ve listeye döndürür.
    /// </summary>
    /// <param name="currentReputation">Dershanenin güncel repütasyonu (0-100 arası)</param>
    /// <param name="forceGoodTeacher">Eğer true ise, üretilen hocalardan en az 1 tanesi kesinlikle üst düzey (8-10) statlarla gelir.</param>
    public List<Teacher> GenerateTeacherCandidates(int currentReputation, bool forceGoodTeacher)
    {
        List<Teacher> candidates = new List<Teacher>();
        
        // Eğer kıyak geçilecekse, 3 hocadan hangisinin (0, 1 veya 2) şanslı olduğunu seç
        int luckyTeacherIndex = -1;
        if (forceGoodTeacher)
        {
            luckyTeacherIndex = Random.Range(0, 3);
        }

        for (int i = 0; i < 3; i++)
        {
            // Rastgele bir isim üret (Örn: "Walter Pinkman")
            string randomName = firstNames[Random.Range(0, firstNames.Length)] + " " + lastNames[Random.Range(0, lastNames.Length)];
            Teacher newTeacher = new Teacher(randomName);

            // Rastgele bir branş ata (0'dan 8'e kadar, Enum uzunluğu)
            newTeacher.branch = (TeacherBranch)Random.Range(0, 8);

            // Eğer bu hoca bizim şanslı (forceGoodTeacher) hocamızsa statları zirveden ver (8-10)
            if (i == luckyTeacherIndex)
            {
                newTeacher.educationEfficiency = Random.Range(8, 11);
                newTeacher.authority = Random.Range(8, 11);
                newTeacher.observation = Random.Range(8, 11);
            }
            else
            {
                // Değilse, Repütasyona göre Ağırlıklı (Weighted) Olasılık uygula
                newTeacher.educationEfficiency = GetWeightedStat(currentReputation);
                newTeacher.authority = GetWeightedStat(currentReputation);
                newTeacher.observation = GetWeightedStat(currentReputation);
            }

            // MAAŞ DENGESİ: Hocanın statları ne kadar yüksekse maaşı (SalaryCost) da o kadar fırlar
            float averageStat = (newTeacher.educationEfficiency + newTeacher.authority + newTeacher.observation) / 3f;
            
            // Maaş genel statların ortalamasına yakındır, araya minik bir rastgelelik (-1, +1) katıyoruz.
            int baseSalary = Mathf.RoundToInt(averageStat);
            newTeacher.salaryCost = Mathf.Clamp(baseSalary + Random.Range(-1, 2), 1, 10);

            candidates.Add(newTeacher);
        }

        return candidates;
    }

    /// <summary>
    /// Repütasyona göre ağırlıklı (Weighted) zar atma fonksiyonu.
    /// Dershanenin ünü (Reputation) arttıkça kaliteli hoca gelme ihtimali katlanarak artar.
    /// </summary>
    private int GetWeightedStat(int reputation)
    {
        int roll = Random.Range(1, 101); // 1-100 arası zar at

        if (reputation < 30) 
        {
            // DÜŞÜK REP: Merdiven altı dershaneye genelde vasıfsız hoca gelir
            if (roll <= 70) return Random.Range(1, 5);       // %70 İhtimalle Çöp (1-4)
            else if (roll <= 95) return Random.Range(5, 8);  // %25 İhtimalle Ortalama (5-7)
            else return Random.Range(8, 11);                 // %5 İhtimalle Gizli Cevher (8-10)
        }
        else if (reputation < 70) 
        {
            // ORTA REP: Standart bir kuruma ortalama hocalar başvurur
            if (roll <= 30) return Random.Range(1, 5);       // %30 İhtimalle Çöp (1-4)
            else if (roll <= 80) return Random.Range(5, 8);  // %50 İhtimalle Ortalama (5-7)
            else return Random.Range(8, 11);                 // %20 İhtimalle Kaliteli (8-10)
        }
        else 
        {
            // YÜKSEK REP: Kaliteli kuruma elit hocalar akar
            if (roll <= 10) return Random.Range(1, 5);       // %10 İhtimalle Torpilli vasıfsız (1-4)
            else if (roll <= 50) return Random.Range(5, 8);  // %40 İhtimalle Ortalama (5-7)
            else return Random.Range(8, 11);                 // %50 İhtimalle Elit Hoca (8-10)
        }
    }
}
