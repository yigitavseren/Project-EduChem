using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI; // YENÝ: Kartýn rengini (Sayýsal/Sözel) deðiþtirmek için bunu ekledik

public class GameManager : MonoBehaviour
{
    [Header("Arayüz (UI) Elementleri")]
    public TextMeshProUGUI kasaText;
    public TextMeshProUGUI gunText;
    public TextMeshProUGUI supheText;
    public TextMeshProUGUI iksirText;
    public TextMeshProUGUI ogrenciText;
    public TextMeshProUGUI basariText;

    [Header("Öðrenci Kabul Paneli (YENÝ EKLENENLER)")]
    public GameObject adayPaneli;           // Panelin kendisi (açýp kapatmak için)
    public Image kartArkaplan;              // Renk deðiþtirecek olan kartýn resmi
    public TextMeshProUGUI kartIsimText;
    public TextMeshProUGUI kartAlanText;
    public TextMeshProUGUI kartNetlerText;
    public TextMeshProUGUI kartGelirText;

    [Header("Dershane & Laboratuvar Ýstatistikleri")]
    public int kasa = 5000;
    public int kacinciGun = 1;
    public int supheSeviyesi = 0;
    public int gizliLaboratuvarGideri = 500;
    public int iksirStogu = 0;
    public int ogrenciSayisi = 0;
    public int basariOrani = 10;

    [Header("Öðrenci Listesi")]
    public List<Student> dershanedekiOgrenciler = new List<Student>();
    public List<Student> kapidakiAdaylar = new List<Student>();
    private Student suAnGosterilenAday; // YENÝ: O an ekranda kartý olan çocuk

    [Header("Üretim Ayarlarý")]
    public int uretimMaliyeti = 300;
    public int uretimRiski = 15;

    private bool oyunBittiMi = false;

    void Start()
    {
        ArayuzuGuncelle();
        // YENÝ: Oyun baþlarken panel ekranda kalmasýn, gizlensin
        if (adayPaneli != null) adayPaneli.SetActive(false);
    }

    public void GunuBitir()
    {
        if (oyunBittiMi) return;

        kacinciGun++;

        kasa += (ogrenciSayisi * 100);
        kasa -= gizliLaboratuvarGideri;

        ogrenciSayisi += (basariOrani / 10);

        if (supheSeviyesi > 0)
        {
            supheSeviyesi -= 5;
            if (supheSeviyesi < 0) supheSeviyesi = 0;
        }

        if (kasa <= 0)
        {
            kasaText.text = "ÝFLAS ETTÝN!";
            oyunBittiMi = true;
            return;
        }

        kapidakiAdaylar.Clear();
        for (int i = 0; i < 3; i++)
        {
            kapidakiAdaylar.Add(new Student(basariOrani));
        }

        foreach (Student ogr in dershanedekiOgrenciler)
        {
            kasa += ogr.gelirKatkisi;
        }

        // YENÝ: Gün bitince kapýdaki adaylarý göstermek için paneli açýyoruz
        if (adayPaneli != null)
        {
            adayPaneli.SetActive(true);
            SiradakiAdayiGoster();
        }

        ArayuzuGuncelle();
    }

    // --- YENÝ EKLENEN PANEL FONKSÝYONLARI BURADAN BAÞLIYOR ---
    public void SiradakiAdayiGoster()
    {
        if (kapidakiAdaylar.Count > 0)
        {
            suAnGosterilenAday = kapidakiAdaylar[0]; // Listeden ilk çocuðu al

            kartIsimText.text = suAnGosterilenAday.isim;
            kartGelirText.text = "Beklenen Gelir: " + suAnGosterilenAday.gelirKatkisi + " TL/Gün";

            // Çocuðun alanýna göre Rengi ve Netleri ayarla
            // Çocuðun alanýna göre Rengi ve Netleri ayarla
            if (suAnGosterilenAday.alan == OgrenciAlani.Sayisal)
            {
                kartArkaplan.color = new Color(0.2f, 0.4f, 0.8f); // Sayýsalcý Mavi
                kartAlanText.text = "Alan: Sayýsal";
                kartNetlerText.text = $"Mat: {suAnGosterilenAday.matNet}/20 (Max: {suAnGosterilenAday.potansiyelMat})\n" +
                                      $"Fizik: {suAnGosterilenAday.fizikNet}/20 (Max: {suAnGosterilenAday.potansiyelFizik})\n" +
                                      $"Kimya: {suAnGosterilenAday.kimyaNet}/20 (Max: {suAnGosterilenAday.potansiyelKimya})\n" +
                                      $"Biyo: {suAnGosterilenAday.biyoNet}/20 (Max: {suAnGosterilenAday.potansiyelBiyo})";
            }
            else if (suAnGosterilenAday.alan == OgrenciAlani.EsitAgirlik)
            {
                kartArkaplan.color = new Color(0.8f, 0.6f, 0.2f); // EA Sarý
                kartAlanText.text = "Alan: Eþit Aðýrlýk";
                kartNetlerText.text = $"Türkçe: {suAnGosterilenAday.turkceNet}/40 (Max: {suAnGosterilenAday.potansiyelTurkce})\n" +
                                      $"Mat: {suAnGosterilenAday.matNet}/40 (Max: {suAnGosterilenAday.potansiyelMat})";
            }
            else if (suAnGosterilenAday.alan == OgrenciAlani.Sozel)
            {
                kartArkaplan.color = new Color(0.8f, 0.3f, 0.3f); // Sözel Kýrmýzý
                kartAlanText.text = "Alan: Sözel";
                kartNetlerText.text = $"Türkçe: {suAnGosterilenAday.turkceNet}/20 (Max: {suAnGosterilenAday.potansiyelTurkce})\n" +
                                      $"Tarih: {suAnGosterilenAday.tarihNet}/20 (Max: {suAnGosterilenAday.potansiyelTarih})\n" +
                                      $"Coðrafya: {suAnGosterilenAday.cogNet}/20 (Max: {suAnGosterilenAday.potansiyelCog})\n" +
                                      $"Felsefe: {suAnGosterilenAday.felsefeNet}/20 (Max: {suAnGosterilenAday.potansiyelFelsefe})";
            }
        }
        else
        {
            // Kapýda aday kalmadýysa paneli kapat ve normal oyuna dön
            adayPaneli.SetActive(false);
        }
    }

    public void AdayiKabulEt()
    {
        dershanedekiOgrenciler.Add(suAnGosterilenAday); // Dershaneye kaydet
        ogrenciSayisi++;
        kapidakiAdaylar.RemoveAt(0); // Kapýdaki sýradan sil
        SiradakiAdayiGoster();       // Varsa sýradaki öðrenciyi ekrana getir
        ArayuzuGuncelle();
    }

    public void AdayiReddet()
    {
        kapidakiAdaylar.RemoveAt(0); // Çocuðu kov
        SiradakiAdayiGoster();       // Varsa sýradaki öðrenciyi ekrana getir
    }
    // --- YENÝ EKLENEN PANEL FONKSÝYONLARI BURADA BÝTÝYOR ---

    // SENÝN MEVCUT FONKSÝYONLARIN AYNLEN DURUYOR
    public void OgrenciKaydet()
    {
        if (oyunBittiMi) return;

        kasa += 1000;
        ogrenciSayisi++;
        ArayuzuGuncelle();
    }

    public void LaboratuvardaUret()
    {
        if (oyunBittiMi) return;

        if (kasa >= uretimMaliyeti)
        {
            kasa -= uretimMaliyeti;
            iksirStogu++;
            supheSeviyesi += uretimRiski;

            if (supheSeviyesi >= 100)
            {
                supheText.text = "ÞÜPHE: %100";
                kasaText.text = "POLÝS BASKINI!";
                gunText.text = "TUTUKLANDIN!";
                oyunBittiMi = true;
                return;
            }

            ArayuzuGuncelle();
        }
    }

    public void IksirIciri()
    {
        if (oyunBittiMi || iksirStogu <= 0 || ogrenciSayisi <= 0) return;

        iksirStogu--;
        basariOrani += 5;
        supheSeviyesi += 2;

        ArayuzuGuncelle();
    }

    void ArayuzuGuncelle()
    {
        kasaText.text = kasa + " TL";
        gunText.text = "Gün: " + kacinciGun;
        supheText.text = "Þüphe: %" + supheSeviyesi;

        if (iksirText != null) iksirText.text = "Ýksir: " + iksirStogu;
        if (ogrenciText != null) ogrenciText.text = "Öðrenci: " + ogrenciSayisi;
        if (basariText != null) basariText.text = "Baþarý: %" + basariOrani;
    }
}