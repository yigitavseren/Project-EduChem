using System.Collections.Generic;
using UnityEngine;

public class TeacherManager : MonoBehaviour
{
    [Header("Staff")]
    public List<Teacher> activeTeachers = new List<Teacher>();

    // Bu fonksiyon her gün sonunda GameManager tarafından veya bağımsız olarak çağrılabilir.
    public void ProcessTeachersEndDay()
    {
        foreach (Teacher teacher in activeTeachers)
        {
            // Otoritesi yüksek olan hocalar disiplini sağlamakta zorlanmaz, bu yüzden daha az yorulurlar.
            // Otorite 1-10 arası bir değer. Formül: 20 puanlık sabit yorulmadan otoriteyi çıkarıyoruz.
            // Örn: Otorite 10 ise -> 20 - 10 = 10 enerji düşer. Otorite 1 ise -> 20 - 1 = 19 enerji düşer.
            float energyLoss = 20f - teacher.authority;
            if (energyLoss < 0) energyLoss = 0; // Negatif enerji kaybını engelle (can dolmasın)

            teacher.energy -= energyLoss;
            
            // Tükenmişlik sendromu (Burnout) her gün 10 puan artar. (Öğretmenlik zor meslek...)
            teacher.burnout += 10f;

            // Değerleri 0 ile 100 arasında sınırla (Clamp)
            teacher.energy = Mathf.Clamp(teacher.energy, 0f, 100f);
            teacher.burnout = Mathf.Clamp(teacher.burnout, 0f, 100f);
            teacher.loyalty = Mathf.Clamp(teacher.loyalty, 0f, 100f);
            teacher.suspicion = Mathf.Clamp(teacher.suspicion, 0f, 100f);
        }
        
        Debug.Log("[TeacherManager] Gün sonu işlemleri tüm aktif öğretmenler için tamamlandı.");
    }
}
