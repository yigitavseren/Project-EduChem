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

        // Bütün UI elemanlarını kod ile zorla hizala (Bozuk tasarımı ezer)
        FixLayout();

        // İsim ve Alan
        if (Txt_Name != null) Txt_Name.text = s.studentName;
        if (Txt_Field != null) Txt_Field.text = s.field.ToString();

        // Statlar
        if (Txt_Intelligence != null) Txt_Intelligence.text = "INT: " + s.intelligence;
        if (Txt_Physique != null) Txt_Physique.text = "PHY: " + s.physique;
        if (Txt_Willpower != null) Txt_Willpower.text = "WIL: " + s.willpower;
        if (Txt_Wealth != null) Txt_Wealth.text = "WEA: " + s.wealth;

        // Barlar, Renkler ve Üst Etiketleri
        if (Slider_Energy != null) 
        { 
            Slider_Energy.maxValue = 100; Slider_Energy.value = s.energy; 
            SetSliderColor(Slider_Energy, new Color(1f, 0.8f, 0f), new Color(0.2f, 0.2f, 0.2f)); // Sarı
            CreateSliderLabel("Energy", 450, 75);
        }
        if (Slider_Sanity != null) 
        { 
            Slider_Sanity.maxValue = 100; Slider_Sanity.value = s.sanity; 
            SetSliderColor(Slider_Sanity, new Color(0.2f, 0.6f, 1f), new Color(0.2f, 0.2f, 0.2f)); // Mavi
            CreateSliderLabel("Sanity", 450, 35);
        }
        if (Slider_Loyalty != null) 
        { 
            Slider_Loyalty.maxValue = 100; Slider_Loyalty.value = s.loyalty; 
            SetSliderColor(Slider_Loyalty, Color.white, new Color(0.2f, 0.2f, 0.2f)); // Beyaz
            CreateSliderLabel("Loyalty", 450, -5);
        }
        if (Slider_Toxicity != null) 
        { 
            Slider_Toxicity.maxValue = 100; Slider_Toxicity.value = s.toxicity; 
            SetSliderColor(Slider_Toxicity, new Color(0.1f, 0.8f, 0.1f), new Color(0.2f, 0.2f, 0.2f)); // Yeşil
            CreateSliderLabel("Toxicity", 450, -45);
        }
    }

    // Slider renklerini ayarlayan yardımcı fonksiyon
    private void SetSliderColor(Slider slider, Color fillColor, Color bgColor)
    {
        if (slider == null) return;

        if (slider.fillRect != null)
        {
            Image fillImage = slider.fillRect.GetComponent<Image>();
            if (fillImage != null) fillImage.color = fillColor;
        }

        Transform bgTransform = slider.transform.Find("Background");
        if (bgTransform != null)
        {
            Image bgImage = bgTransform.GetComponent<Image>();
            if (bgImage != null) bgImage.color = bgColor;
        }
    }

    // Otonom olarak sliderların üstüne isim yazdıran fonksiyon
    private void CreateSliderLabel(string labelText, float x, float y)
    {
        string objName = labelText + "_AutoLabel";
        if (transform.Find(objName) != null) return; // Zaten varsa bir daha ekleme

        GameObject go = new GameObject(objName);
        go.transform.SetParent(this.transform, false);
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = labelText;
        tmp.color = Color.white;
        SetRect(tmp, x, y, 200, 20, 14);
    }

    // Unity UI'daki anchor ve pivot hatalarını hiçe sayıp objeleri otonom olarak ızgaraya oturtur
    private void FixLayout()
    {
        RectTransform rowRect = GetComponent<RectTransform>();
        if (rowRect != null)
        {
            // Satırın yüksekliğini sabitle (İçine sığsınlar diye)
            rowRect.sizeDelta = new Vector2(rowRect.sizeDelta.x, 180f);
            
            // LayoutGroup'un bu satırı ezmesini engellemek için LayoutElement ekle/güncelle
            LayoutElement le = GetComponent<LayoutElement>();
            if (le == null) le = gameObject.AddComponent<LayoutElement>();
            le.minHeight = 180f;
            le.preferredHeight = 180f;
        }

        // Sol Sütun (İsim ve Alan)
        SetRect(Txt_Name, 20, 40, 250, 40, 24);
        SetRect(Txt_Field, 20, -40, 250, 40, 20);

        // Orta Sütun (Statlar)
        SetRect(Txt_Intelligence, 280, 60, 150, 30, 20);
        SetRect(Txt_Physique, 280, 20, 150, 30, 20);
        SetRect(Txt_Willpower, 280, -20, 150, 30, 20);
        SetRect(Txt_Wealth, 280, -60, 150, 30, 20);

        // Sağ Sütun (Slider Barları)
        SetRect(Slider_Energy, 450, 60, 200, 20, 0);
        SetRect(Slider_Sanity, 450, 20, 200, 20, 0);
        SetRect(Slider_Loyalty, 450, -20, 200, 20, 0);
        SetRect(Slider_Toxicity, 450, -60, 200, 20, 0);
    }

    private void SetRect(Component comp, float x, float y, float w, float h, float fontSize)
    {
        if (comp == null) return;
        RectTransform rt = comp.GetComponent<RectTransform>();
        if (rt == null) return;

        // Herhangi bir yanlış Scale ayarını sıfırla
        rt.localScale = Vector3.one;

        // Orta-Sol köşeye demirle (Y=0 tam ortasıdır)
        rt.anchorMin = new Vector2(0, 0.5f);
        rt.anchorMax = new Vector2(0, 0.5f);
        rt.pivot = new Vector2(0, 0.5f);
        
        // Pozisyon ve boyutu zorla
        rt.anchoredPosition = new Vector2(x, y);
        rt.sizeDelta = new Vector2(w, h);

        // Eğer bu bir Text ise, taşmasını ve fontunu zorla
        TextMeshProUGUI tmp = comp as TextMeshProUGUI;
        if (tmp != null && fontSize > 0)
        {
            tmp.fontSize = fontSize;
            tmp.enableWordWrapping = false;
            tmp.alignment = TextAlignmentOptions.Left | TextAlignmentOptions.Midline;
            tmp.overflowMode = TextOverflowModes.Overflow;
        }
    }
}
