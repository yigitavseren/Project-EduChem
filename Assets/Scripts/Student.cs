using UnityEngine;

// Çocuðun hangi alandan sýnava gireceðini belirleyen kategori
public enum OgrenciAlani { Sayisal, EsitAgirlik, Sozel }

[System.Serializable]
public class Student
{
    public string isim;
    public OgrenciAlani alan; // Sayýsal mý, EA mý, Sözel mi?
    public int gelirKatkisi;

    // --- DERS NETLERÝ (Mevcut / Potansiyel) ---
    public int matNet, potansiyelMat;
    public int fizikNet, potansiyelFizik;
    public int kimyaNet, potansiyelKimya;
    public int biyoNet, potansiyelBiyo;

    public int turkceNet, potansiyelTurkce;
    public int tarihNet, potansiyelTarih;
    public int cogNet, potansiyelCog;
    public int felsefeNet, potansiyelFelsefe;

    public Student(int dershaneBasarisi)
    {
        // DEV ERKEK & KADIN ÝSÝM HAVUZU
        string[] isimler = {
            "Yiðit", "Ahmet", "Mehmet", "Can", "Murat", "Selin", "Zeynep", "Buse", "Elif", "Arda",
            "Burak", "Emre", "Furkan", "Oðuzhan", "Mert", "Volkan", "Gökhan", "Hakan", "Serkan", "Kaan",
            "Deniz", "Ege", "Barýþ", "Umut", "Güneþ", "Doruk", "Görkem", "Batuhan", "Mete", "Alper",
            "Yaðýz", "Emir", "Kerem", "Tarýk", "Utku", "Anýl", "Tuna", "Onur", "Cem", "Alp",
            "Tolga", "Ozan", "Berk", "Eren", "Uður", "Okan", "Cihan", "Ýlker", "Soner", "Özgür",
            "Ayþe", "Fatma", "Hayriye", "Emine", "Hatice", "Merve", "Gamze", "Gizem", "Seda", "Ebru",
            "Tuðba", "Kübra", "Büþra", "Rabia", "Beyza", "Hilal", "Sena", "Aslý", "Ezgi", "Özge",
            "Ceren", "Dilek", "Pýnar", "Irmak", "Damla", "Yaðmur", "Belen", "Nisa", "Melisa", "Aleyna",
            "Didem", "Sinem", "Bahar", "Hazal", "Dilan", "Rojda", "Ece", "Melis", "Ýrem", "Berfin",
            "Gözde", "Derya", "Asya", "Defne", "Derin", "Doða", "Bade", "Simge", "Hande", "Bengü",
            "Gül", "Lale", "Karanfil", "Menekþe", "Narin", "Naz", "Eda", "Sude", "Eylül", "Öykü",
            "Ali", "Veli", "Hasan", "Hüseyin", "Osman", "Mustafa", "Kemal", "Yusuf", "Ömer", "Hamza",
            "Berat", "Efe", "Metehan", "Alparslan", "Oðuz", "Bugra", "Taha", "Yasin", "Bilal", "Fatih",
            "Süleyman", "Ibrahim", "Halil", "Sadýk", "Salih", "Asým", "Metin", "Tekin", "Cetin", "Semih",
            "Melisa", "Esen", "Duygu", "Sibel", "Yeþim", "Nihal", "Handan", "Jale", "Hale", "Leman",
            "Berna", "Asena", "Banu", "Cansel", "Cansu", "Elmas", "Filiz", "Funda"
        };

        // DEV SOYADI HAVUZU
        string[] soyadlar = {
            "Yýlmaz", "Kaya", "Demir", "Çelik", "Þahin", "Yýldýz", "Yýldýrým", "Öztürk", "Aydýn", "Özdemir",
            "Arslan", "Doðan", "Kýlýç", "Aslan", "Çetin", "Kara", "Koç", "Kurt", "Özkan", "Þimþek",
            "Acar", "Avcý", "Yaman", "Bulut", "Köse", "Aksoy", "Yalçýn", "Turan", "Güler", "Yaser",
            "Korkmaz", "Erdoðan", "Polat", "Güneþ", "Eser", "Candan", "Tekin", "Uysal", "Gök",
            "Okan", "Budak", "Sarý", "Aktaþ", "Uzun", "Kýsa", "Yüksek", "Alkan", "Þen", "Gül",
            "Akýn", "Bozkurt", "Özcan", "Gündüz", "Ünal", "Yiðit", "Güngör", "Çakýr", "Koçak", "Özer",
            "Duran", "Akkuþ", "Sarýkaya", "Yavuz", "Karaca", "Güven", "Coþkun", "Deniz", "Solmaz", "Ay",
            "Karakaya", "Erten", "Tüfekçi", "Sönmez", "Öz", "Gencer", "Baþtürk", "Yurt", "Savaþ", "Barýþ",
            "Umut", "Duman", "Köksal", "Tuncer", "Büyük", "Küçük", "Akyol", "Iþýk", "Sarýoðlu", "Avseren",
            "Dað", "Taþ", "Kütük", "Kalýp", "Dengiz", "Pekcan", "Uçar", "Kaçan", "Yazar", "Çizer"
        };

        // Ýsmi ve Soyadý Kombinle
        this.isim = isimler[Random.Range(0, isimler.Length)] + " " + soyadlar[Random.Range(0, soyadlar.Length)];

        // 2. RASTGELE ALAN SEÇÝMÝ
        this.alan = (OgrenciAlani)Random.Range(0, 3); // 0, 1 veya 2 döner

        // 3. STAT DAÐITIMI VE EKONOMÝ
        if (dershaneBasarisi < 30) this.gelirKatkisi = Random.Range(50, 100);
        else if (dershaneBasarisi < 70) this.gelirKatkisi = Random.Range(150, 250);
        else this.gelirKatkisi = Random.Range(400, 600);

        // 4. ALANINA GÖRE NETLERÝ BELÝRLE
        switch (this.alan)
        {
            case OgrenciAlani.Sayisal:
                matNet = NetUret(20, dershaneBasarisi, out potansiyelMat);
                fizikNet = NetUret(20, dershaneBasarisi, out potansiyelFizik);
                kimyaNet = NetUret(20, dershaneBasarisi, out potansiyelKimya);
                biyoNet = NetUret(20, dershaneBasarisi, out potansiyelBiyo);
                break;

            case OgrenciAlani.EsitAgirlik:
                turkceNet = NetUret(40, dershaneBasarisi, out potansiyelTurkce);
                matNet = NetUret(40, dershaneBasarisi, out potansiyelMat);
                break;

            case OgrenciAlani.Sozel:
                turkceNet = NetUret(20, dershaneBasarisi, out potansiyelTurkce);
                tarihNet = NetUret(20, dershaneBasarisi, out potansiyelTarih);
                cogNet = NetUret(20, dershaneBasarisi, out potansiyelCog);
                felsefeNet = NetUret(20, dershaneBasarisi, out potansiyelFelsefe);
                break;
        }
    }

    // Maksimum soru sayýsýna ve dershane baþarýsýna göre mantýklý net üreten motor
    private int NetUret(int maxSoru, int dershaneBasarisi, out int potansiyel)
    {
        // ARTIK maxSoru DEÐERÝ DIÞARIDAN GELEN (20 veya 40) DEÐER OLACAK
        int mevcutNet;

        if (dershaneBasarisi < 30) // Tembel tayfa (0 - %40 net arasý)
        {
            mevcutNet = Random.Range(0, (int)(maxSoru * 0.41f));
            potansiyel = Random.Range(mevcutNet, (int)(maxSoru * 0.71f));
        }
        else if (dershaneBasarisi < 70) // Orta seviye (%30 - %70 net arasý)
        {
            mevcutNet = Random.Range((int)(maxSoru * 0.3f), (int)(maxSoru * 0.71f));
            potansiyel = Random.Range(mevcutNet, (int)(maxSoru * 0.91f));
        }
        else // Derece öðrencisi (%60 - Full net arasý)
        {
            mevcutNet = Random.Range((int)(maxSoru * 0.6f), maxSoru + 1);
            potansiyel = maxSoru;
        }

        return mevcutNet;
    }
}
