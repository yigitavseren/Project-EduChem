using UnityEngine;

[System.Serializable]
public class Student
{
    public string isim;
    public int baslangicNeti;
    public int mevcutNet;
    public int potansiyelNet; // Ýksirle çýkabileceði maksimum sýnýr
    public int gelirKatkisi;  // Bu öðrencinin her gün kasaya býrakacaðý para

    // Yeni rastgele öðrenci yaratma motoru
    public Student(int dershaneBasarisi)
    {
        // Rastgele isim havuzu
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
            "Berna", "Asena", "Banu", "Cansel", "Cansu", "Deniz", "Derya", "Elmas", "Filiz", "Funda"
        };

        string[] soyadlar = {
            "Yýlmaz", "Kaya", "Demir", "Çelik", "Þahin", "Yýldýz", "Yýldýrým", "Öztürk", "Aydýn", "Özdemir",
            "Arslan", "Doðan", "Kýlýç", "Aslan", "Çetin", "Kara", "Koç", "Kurt", "Özkan", "Þimþek",
            "Acar", "Avcý", "Yaman", "Bulut", "Köse", "Aksoy", "Yalçýn", "Turan", "Güler", "Yaser",
            "Korkmaz", "Erdoðan", "Polat", "Güneþ", "Eser", "Aslan", " can", "Tekin", "Uysal", "Gök",
            "Okan", "Budak", "Sarý", "Aktaþ", "Uzun", "Kýsa", "Yüksek", "Alkan", "Þen", "Gül",
            "Akýn", "Bozkurt", "Özcan", "Gündüz", "Ünal", "Yiðit", "Güngör", "Çakýr", "Koçak", "Özer",
            "Duran", "Akkuþ", "Sarýkaya", "Yavuz", "Karaca", "Güven", "Coþkun", "Deniz", "Solmaz", "Ay",
            "Karakaya", "Erten", "Tüfekçi", "Sönmez", "Öz", "Gencer", "Baþtürk", "Yurt", "Savaþ", "Barýþ",
            "Umut", "Duman", "Köksal", "Tuncer", "Büyük", "Küçük", "Akyol", "Iþýk", "Sarýoðlu", "Avseren",
            "Dað", "Taþ", "Kütük", "Kalýp", "Dengiz", "Pekcan", "Uçar", "Kaçan", "Yazar", "Çizer"
        };

        // Ýsmi ve Soyadý Kombinle
        this.isim = isimler[Random.Range(0, isimler.Length)] + " " + soyadlar[Random.Range(0, soyadlar.Length)];

        // Dershane baþarýsý düþükse (%10-%30), gelen çocuklarýn netleri de düþük olur
        if (dershaneBasarisi < 30)
        {
            this.baslangicNeti = Random.Range(5, 20);      // Tembel tayfa (5-20 net arasý)
            this.potansiyelNet = Random.Range(30, 50);     // Ýksirle bile max 50 nete çýkabilir
            this.gelirKatkisi = Random.Range(50, 100);     // Az para býrakýrlar
        }
        // Dershane baþarý yüzdesi arttýkça elit öðrenciler gelmeye baþlar
        else if (dershaneBasarisi >= 30 && dershaneBasarisi < 70)
        {
            this.baslangicNeti = Random.Range(25, 55);     // Orta seviye
            this.potansiyelNet = Random.Range(60, 85);
            this.gelirKatkisi = Random.Range(150, 250);
        }
        else
        {
            this.baslangicNeti = Random.Range(60, 95);     // Derece öðrencileri (Zehir gibi)
            this.potansiyelNet = Random.Range(95, 100);
            this.gelirKatkisi = Random.Range(400, 600);    // Parayý basýp gelirler
        }

        this.mevcutNet = this.baslangicNeti;
    }
}
