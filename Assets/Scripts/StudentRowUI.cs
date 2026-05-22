using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StudentRowUI : MonoBehaviour
{
    [Header("General Info")]
    public TextMeshProUGUI Txt_Name;
    public TextMeshProUGUI Txt_Field;

    [Header("Stats (Texts)")]
    public TextMeshProUGUI Txt_Intelligence;
    public TextMeshProUGUI Txt_Physique;
    public TextMeshProUGUI Txt_Willpower;
    public TextMeshProUGUI Txt_Wealth;

    [Header("Survival Bars (Sliders)")]
    public Slider Slider_Energy;
    public Slider Slider_Sanity;
    public Slider Slider_Loyalty;
    public Slider Slider_Toxicity;

    // Bu fonksiyon StudentsListManager tarafından çağrılacak
    public void Setup(Student s)
    {
        if (s == null) return;

        // İsim ve Alan
        if (Txt_Name != null) Txt_Name.text = s.studentName;
        if (Txt_Field != null) Txt_Field.text = s.field.ToString();

        // Statlar (Örn: "INT: 8" gibi görünmesi için kısa başlıklar ekledim, istersen değiştirebilirsin)
        if (Txt_Intelligence != null) Txt_Intelligence.text = "INT: " + s.intelligence;
        if (Txt_Physique != null) Txt_Physique.text = "PHY: " + s.physique;
        if (Txt_Willpower != null) Txt_Willpower.text = "WIL: " + s.willpower;
        if (Txt_Wealth != null) Txt_Wealth.text = "WEA: " + s.wealth;

        // Barlar (Max değerleri 100 yapıyoruz ki direkt dolsun)
        if (Slider_Energy != null) { Slider_Energy.maxValue = 100; Slider_Energy.value = s.energy; }
        if (Slider_Sanity != null) { Slider_Sanity.maxValue = 100; Slider_Sanity.value = s.sanity; }
        if (Slider_Loyalty != null) { Slider_Loyalty.maxValue = 100; Slider_Loyalty.value = s.loyalty; }
        if (Slider_Toxicity != null) { Slider_Toxicity.maxValue = 100; Slider_Toxicity.value = s.toxicity; }
    }
}
