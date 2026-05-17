using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Arayüz (UI) Elementleri")]
    public TextMeshProUGUI kasaText;
    public TextMeshProUGUI gunText;
    public TextMeshProUGUI supheText;
    public TextMeshProUGUI iksirText;
    public TextMeshProUGUI ogrenciText; // YENÝLER BURADA
    public TextMeshProUGUI basariText;

    [Header("Dershane & Laboratuvar Ýstatistikleri")]
    public int kasa = 5000;
    public int kacinciGun = 1;
    public int supheSeviyesi = 0;
    public int gizliLaboratuvarGideri = 500;
    public int iksirStogu = 0;
    public int ogrenciSayisi = 0;
    public int basariOrani = 10;

    [Header("Öðrenci Listesi")]
    public List<Student> dershanedekiOgrenciler = new List<Student>(); // Kaydettiðimiz öðrenciler burada birikecek
    public List<Student> kapidakiAdaylar = new List<Student>();      // Kapýda bekleyen kýsýtlý adaylar

    [Header("Üretim Ayarlarý")]
    public int uretimMaliyeti = 300;
    public int uretimRiski = 15;

    private bool oyunBittiMi = false;

    void Start()
    {
        ArayuzuGuncelle();
    }

    public void GunuBitir()
    {
        if (oyunBittiMi) return;

        kacinciGun++;

        // Gelir ve Giderler hesaplanýyor
        kasa += (ogrenciSayisi * 100);
        kasa -= gizliLaboratuvarGideri;

        // Baþarýya göre yeni öðrenciler geliyor
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

        // Her gün bittiðinde kapýya yeni, kýsýtlý sayýda (örneðin 3 tane) rastgele aday gelsin
        kapidakiAdaylar.Clear();
        for (int i = 0; i < 3; i++)
        {
            kapidakiAdaylar.Add(new Student(basariOrani));
        }

        // EKONOMÝ GÜNCELLEMESÝ: Artýk sabit para deðil, her öðrencinin kendi gelir katkýsý kasaya eklensin
        foreach (Student ogr in dershanedekiOgrenciler)
        {
            kasa += ogr.gelirKatkisi;
        }

        ArayuzuGuncelle();
    }

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