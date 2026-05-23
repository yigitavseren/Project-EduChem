using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class TeacherRowUI : MonoBehaviour
{
    [Header("UI Elemanlari")]
    public TextMeshProUGUI Txt_Name;
    
    [Header("RPG Statlari (Text)")]
    public TextMeshProUGUI Txt_Education;
    public TextMeshProUGUI Txt_Authority;
    public TextMeshProUGUI Txt_Observation;
    public TextMeshProUGUI Txt_Salary;

    [Header("Bar Elemanlari (Slider)")]
    public Slider Slider_Energy;
    public Slider Slider_Burnout;
    public Slider Slider_Loyalty;
    public Slider Slider_Suspicion;

    private RectTransform rectTransform;
    
    // Unity'nin UI hatalarını önlemek için tasarlanan zorunlu ölçüler
    private float targetWidth = 1000f;
    private float targetHeight = 150f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        FixLayout();
    }

    // Otonom Hata Giderici (UI Kaymalarını Önler)
    private void FixLayout()
    {
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.pivot = new Vector2(0.5f, 1f);
        rectTransform.sizeDelta = new Vector2(targetWidth, targetHeight);
        rectTransform.localScale = Vector3.one;
    }

    /// <summary>
    /// Bu satıra bir öğretmenin verilerini yansıtmak için kullanılır.
    /// </summary>
    public void SetData(Teacher t)
    {
        if (t == null) return;

        if (Txt_Name != null) Txt_Name.text = t.teacherName + " (" + t.branch.ToString() + ")";

        if (Txt_Education != null) Txt_Education.text = "Edu: " + t.educationEfficiency;
        if (Txt_Authority != null) Txt_Authority.text = "Auth: " + t.authority;
        if (Txt_Observation != null) Txt_Observation.text = "Obs: " + t.observation;
        if (Txt_Salary != null) Txt_Salary.text = "Salary: " + t.salaryCost + "$/Day";

        if (Slider_Energy != null)
        {
            Slider_Energy.maxValue = 100f;
            Slider_Energy.value = t.energy;
            SetSliderColor(Slider_Energy, new Color(1f, 0.8f, 0f)); // Sarı (Enerji)
            CreateSliderLabel(Slider_Energy, "Energy");
        }

        if (Slider_Burnout != null)
        {
            Slider_Burnout.maxValue = 100f;
            Slider_Burnout.value = t.burnout;
            SetSliderColor(Slider_Burnout, new Color(0.8f, 0.2f, 0.2f)); // Kırmızı (Tükenmişlik)
            CreateSliderLabel(Slider_Burnout, "Burnout");
        }

        if (Slider_Loyalty != null)
        {
            Slider_Loyalty.maxValue = 100f;
            Slider_Loyalty.value = t.loyalty;
            SetSliderColor(Slider_Loyalty, Color.white); // Beyaz (Sadakat)
            CreateSliderLabel(Slider_Loyalty, "Loyalty");
        }

        if (Slider_Suspicion != null)
        {
            Slider_Suspicion.maxValue = 100f;
            Slider_Suspicion.value = t.suspicion;
            SetSliderColor(Slider_Suspicion, new Color(0.6f, 0.2f, 0.8f)); // Mor (Şüphe)
            CreateSliderLabel(Slider_Suspicion, "Suspicion");
        }
    }

    private void SetSliderColor(Slider slider, Color fillColor)
    {
        if (slider == null || slider.fillRect == null) return;
        Image fillImage = slider.fillRect.GetComponent<Image>();
        if (fillImage != null) fillImage.color = fillColor;
    }

    private void CreateSliderLabel(Slider slider, string labelText)
    {
        if (slider == null) return;

        // Label zaten varsa tekrar yaratma
        if (slider.transform.Find(labelText + "_Label") != null) return;

        GameObject labelObj = new GameObject(labelText + "_Label");
        labelObj.transform.SetParent(slider.transform, false);

        TextMeshProUGUI tmp = labelObj.AddComponent<TextMeshProUGUI>();
        tmp.text = labelText;
        tmp.fontSize = 18;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Bottom;
        tmp.fontStyle = FontStyles.Bold;

        RectTransform rt = labelObj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 0);
        rt.sizeDelta = new Vector2(0, 30);
        rt.anchoredPosition = new Vector2(0, -10); // Slider'ın hafif üstüne
    }
}
