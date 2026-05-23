using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI; // NEW: Added to change the card's background color (Science/Humanities)

public class StudentDailyModifiers
{
    public float IncomeMultiplier = 1f;
    public int ExtraEnergyDecay = 0;
    public int ExtraSanityDecay = 0;
    public float ToxicityIncreaseMultiplier = 1f;
    public int EnergyDecayReduction = 0;
    public int LoyaltyBonus = 0;
    public int SanityBonus = 0;
    public bool PreventStatDecay = false;
    public bool IsIncomeZero = false;
}

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI suspicionText;
    public TextMeshProUGUI potionText;
    public TextMeshProUGUI studentText;
    public TextMeshProUGUI successText;

    [Header("Student Admission Panel (NEW ADDITIONS)")]
    public GameObject candidatePanel;           // The panel itself (to toggle on/off)
    public Image cardBackground;                // The card image that will change color
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cardFieldText;
    public TextMeshProUGUI cardScoresText;
    public TextMeshProUGUI cardIncomeText;

    [Header("Student List Panel")]
    public GameObject studentListPanel;         // Our main panel
    public Transform studentListContent;        // Content object inside Scroll View
    public GameObject studentRowPrefab;         // The Prefab we created earlier

    [Header("Laboratory and Potion System")]
    public GameObject laboratoryPanel;
    public TextMeshProUGUI focusSyrupText;      // We will link the "Owned..." text here

    private int focusSyrupCount = 0;            // Total potions we have
    private int potionCost = 100;               // Price of 1 potion

    [Header("Academy & Lab Statistics")]
    public int money = 5000;
    public int currentDay = 1;
    public int suspicionLevel = 0;
    public int secretLabExpense = 500;
    public int potionStock = 0;
    public int studentCount = 0;
    public int successRate = 10;

    [Header("Student Roster")]
    public List<Student> enrolledStudents = new List<Student>();
    public List<Student> waitingCandidates = new List<Student>();
    private Student currentlyDisplayedCandidate; // NEW: The kid whose card is currently on screen

    [Header("Production Settings")]
    public int productionCost = 300;
    public int productionRisk = 15;

    private bool isGameOver = false;

    void Start()
    {
        UpdateUI();
        // NEW: Hide the panel when the game starts so it doesn't stay on screen
        if (candidatePanel != null) candidatePanel.SetActive(false);
    }

    public void EndDay()
    {
        if (isGameOver) return;

        currentDay++;
        money -= secretLabExpense; // Sabit laboratuvar masrafı

        if (suspicionLevel > 0)
        {
            suspicionLevel -= 5;
            if (suspicionLevel < 0) suspicionLevel = 0;
        }

        // --- YENİ SİSTEM: HASAT VAKTİ VE EZİYET ---
        // Öğretmen branşlarına göre günlük modifiyeleri hesapla
        StudentDailyModifiers mods = CalculateBranchEffects();
        
        int totalIncomeForToday = 0;

        foreach (Student kid in enrolledStudents)
        {
            // 1. Kasa Kazancı: (Cüzdan + Zeka). Oyun ekonomisi anlamlı olsun diye 50 ile çarpılıyor.
            float dailyIncomeFloat = (kid.intelligence + kid.wealth) * 50f;
            
            // Branş etkisini uygula (Çarpanlar ve Felsefe sıfırlaması)
            dailyIncomeFloat *= mods.IncomeMultiplier;
            if (mods.IsIncomeZero) dailyIncomeFloat = 0;
            
            int dailyIncome = Mathf.RoundToInt(dailyIncomeFloat);
            totalIncomeForToday += dailyIncome;
            kid.incomeContribution = dailyIncome;

            // 2. Eziyet (Mentalite ve Enerji Düşüşü)
            int baseDecay = 20 - kid.willpower;
            if (baseDecay < 0) baseDecay = 0;

            // Branşlardan gelen ekstra yorgunluk ve hafifletmeleri hesapla
            int finalEnergyDecay = baseDecay + mods.ExtraEnergyDecay - mods.EnergyDecayReduction;
            if (finalEnergyDecay < 0) finalEnergyDecay = 0;
            
            int finalSanityDecay = baseDecay + mods.ExtraSanityDecay;
            if (finalSanityDecay < 0) finalSanityDecay = 0;

            // Cografya hocası varsa (PreventStatDecay) yorgunluk sıfırlanır
            if (mods.PreventStatDecay)
            {
                finalEnergyDecay = 0;
                finalSanityDecay = 0;
            }

            kid.energy -= finalEnergyDecay;
            kid.sanity -= finalSanityDecay;

            // Branşların pozitif bonusları (Felsefe ve Tarih vb.)
            kid.sanity += mods.SanityBonus;
            kid.loyalty += mods.LoyaltyBonus;

            // 3. Detoks (Toksisite Temizliği)
            if (kid.toxicity > 0)
            {
                kid.toxicity -= 10;
            }
            // (mods.ToxicityIncreaseMultiplier ileride laboratuvar toksisite artışı eklendiğinde kullanılacak)

            // 4. Matematiksel Sınırlar (Mathf.Clamp)
            kid.energy = Mathf.Clamp(kid.energy, 0, 100);
            kid.sanity = Mathf.Clamp(kid.sanity, 0, 100);
            kid.toxicity = Mathf.Clamp(kid.toxicity, 0, 100);
            kid.loyalty = Mathf.Clamp(kid.loyalty, 0, 100);
        }
        
        money += totalIncomeForToday;

        if (money <= 0)
        {
            if (moneyText != null) moneyText.text = "BANKRUPT!";
            isGameOver = true;
            return;
        }

        // --- YENİ EKLENEN: ÖĞRETMENLERİN GÜN SONU İŞLEMLERİ ---
        TeacherManager teacherManager = FindObjectOfType<TeacherManager>(true);
        if (teacherManager != null)
        {
            teacherManager.ProcessTeachersEndDay();
        }

        UpdateUI();

        // 5. Öğrenci Listesi UI'ını tetikle
        UpdateStudentList();
        
        // Ertesi gün için kapıya yeni adaylar geldiğinde paneli otomatik aç!
        if (candidatePanel != null)
        {
            candidatePanel.SetActive(true);
        }
    }

    // --- BRANŞ ETKİLERİNİ HESAPLAYAN YARDIMCI FONKSİYON ---
    private StudentDailyModifiers CalculateBranchEffects()
    {
        StudentDailyModifiers mods = new StudentDailyModifiers();
        TeacherManager teacherManager = FindObjectOfType<TeacherManager>(true);
        if (teacherManager == null) return mods;

        // Aynı branşın etkisinin 1'den fazla kez uygulanmasını engellemek için HashSet kullanıyoruz
        HashSet<TeacherBranch> appliedBranches = new HashSet<TeacherBranch>();

        foreach (Teacher t in teacherManager.activeTeachers)
        {
            if (appliedBranches.Contains(t.branch)) continue; // Zaten bu branşın etkisini uyguladıysak atla
            appliedBranches.Add(t.branch);

            switch (t.branch)
            {
                case TeacherBranch.Matematik:
                    mods.IncomeMultiplier *= 2.0f;
                    mods.ExtraEnergyDecay += 30;
                    break;
                case TeacherBranch.Fizik:
                    mods.IncomeMultiplier *= 2.5f;
                    mods.ExtraSanityDecay += 35;
                    break;
                case TeacherBranch.Kimya:
                    mods.IncomeMultiplier *= 1.2f;
                    mods.ToxicityIncreaseMultiplier *= 0.7f;
                    break;
                case TeacherBranch.Biyoloji:
                    mods.IncomeMultiplier *= 1.5f;
                    mods.EnergyDecayReduction += 10;
                    // TODO: İleride Bünye (Physique) statı için pasif bir buff (hastalık direnci vb.) buraya eklenecek.
                    break;
                case TeacherBranch.Edebiyat:
                    mods.IncomeMultiplier *= 1.5f;
                    mods.ExtraSanityDecay += 15;
                    break;
                case TeacherBranch.Tarih:
                    mods.IncomeMultiplier *= 1.2f;
                    mods.LoyaltyBonus += 15;
                    break;
                case TeacherBranch.Cografya:
                    mods.IncomeMultiplier *= 1.0f;
                    mods.PreventStatDecay = true;
                    break;
                case TeacherBranch.Felsefe:
                    mods.IsIncomeZero = true;
                    mods.SanityBonus += 40;
                    break;
            }
        }
        return mods;
    }

    // --- NEWLY ADDED PANEL FUNCTIONS START HERE ---
    public void ShowNextCandidate()
    {
        if (waitingCandidates.Count > 0)
        {
            currentlyDisplayedCandidate = waitingCandidates[0]; // Get the first kid from the list

            if (cardNameText != null) cardNameText.text = currentlyDisplayedCandidate.studentName;
            if (cardIncomeText != null) cardIncomeText.text = "Expected Income: " + currentlyDisplayedCandidate.incomeContribution + " TL/Day";

            if (cardBackground != null && cardFieldText != null && cardScoresText != null)
            {
                // Set Color and Scores based on the kid's field
                if (currentlyDisplayedCandidate.field == StudentField.Science)
                {
                    cardBackground.color = new Color(0.2f, 0.4f, 0.8f); // Science Blue
                    cardFieldText.text = "Field: Science";
                    cardScoresText.text = $"Math: {currentlyDisplayedCandidate.mathScore}/20 (Max: {currentlyDisplayedCandidate.potentialMath})\n" +
                                          $"Physics: {currentlyDisplayedCandidate.physicsScore}/20 (Max: {currentlyDisplayedCandidate.potentialPhysics})\n" +
                                          $"Chem: {currentlyDisplayedCandidate.chemistryScore}/20 (Max: {currentlyDisplayedCandidate.potentialChemistry})\n" +
                                          $"Bio: {currentlyDisplayedCandidate.bioScore}/20 (Max: {currentlyDisplayedCandidate.potentialBio})";
                }
                else if (currentlyDisplayedCandidate.field == StudentField.EqualWeight)
                {
                    cardBackground.color = new Color(0.8f, 0.6f, 0.2f); // Equal Weight Yellow
                    cardFieldText.text = "Field: Equal Weight";
                    cardScoresText.text = $"Turkish: {currentlyDisplayedCandidate.turkishScore}/40 (Max: {currentlyDisplayedCandidate.potentialTurkish})\n" +
                                          $"Math: {currentlyDisplayedCandidate.mathScore}/40 (Max: {currentlyDisplayedCandidate.potentialMath})";
                }
                else if (currentlyDisplayedCandidate.field == StudentField.Humanities)
                {
                    cardBackground.color = new Color(0.8f, 0.3f, 0.3f); // Humanities Red
                    cardFieldText.text = "Field: Humanities";
                    cardScoresText.text = $"Turkish: {currentlyDisplayedCandidate.turkishScore}/20 (Max: {currentlyDisplayedCandidate.potentialTurkish})\n" +
                                          $"History: {currentlyDisplayedCandidate.historyScore}/20 (Max: {currentlyDisplayedCandidate.potentialHistory})\n" +
                                          $"Geo: {currentlyDisplayedCandidate.geoScore}/20 (Max: {currentlyDisplayedCandidate.potentialGeo})\n" +
                                          $"Philosophy: {currentlyDisplayedCandidate.philosophyScore}/20 (Max: {currentlyDisplayedCandidate.potentialPhilosophy})";
                }
            }
        }
        else
        {
            // If no candidates left at the door, close the panel and return to normal game
            candidatePanel.SetActive(false);
        }
    }

    public void AcceptCandidate()
    {
        enrolledStudents.Add(currentlyDisplayedCandidate); // Register to academy
        studentCount++;
        waitingCandidates.RemoveAt(0); // Remove from the waiting line
        ShowNextCandidate();           // Show the next student if available
        UpdateUI();
    }

    public void RejectCandidate()
    {
        waitingCandidates.RemoveAt(0); // Kick the kid out
        ShowNextCandidate();           // Show the next student if available
    }
    // --- NEWLY ADDED PANEL FUNCTIONS END HERE ---

    // YOUR EXISTING FUNCTIONS REMAIN EXACTLY THE SAME (Translated)
    public void EnrollStudent()
    {
        if (isGameOver) return;

        money += 1000;
        studentCount++;
        UpdateUI();
    }

    public void ProduceInLaboratory()
    {
        if (isGameOver) return;

        if (money >= productionCost)
        {
            money -= productionCost;
            potionStock++;
            suspicionLevel += productionRisk;

            if (suspicionLevel >= 100)
            {
                suspicionText.text = "SUSPICION: 100%";
                moneyText.text = "POLICE RAID!";
                dayText.text = "BUSTED!";
                isGameOver = true;
                return;
            }

            UpdateUI();
        }
    }

    public void GivePotion()
    {
        if (isGameOver || potionStock <= 0 || studentCount <= 0) return;

        potionStock--;
        successRate += 5;
        suspicionLevel += 2;

        UpdateUI();
    }

    public void UpdateStudentList()
    {
        // Eski manuel TextMeshPro arama sistemi İPTAL. 
        // Artık otonom StudentsListManager bu işi devraldı.
        // true parametresi panel kapalıyken bile (örneğin End Day'e basıldığında) scripti bulmasını sağlar
        StudentsListManager listManager = FindObjectOfType<StudentsListManager>(true);
        if (listManager != null)
        {
            listManager.RefreshList();
        }
        else
        {
            Debug.LogWarning("[GameManager] StudentsListManager bulunamadı! Lütfen listeyi yönetecek objeye scripti ekleyin.");
        }
    }

    public void ToggleStudentListPanel()
    {
        // If the interview panel is open, don't allow opening the student list (so they don't overlap)
        if (candidatePanel.activeSelf) return;

        // Invert the panel's state (Close if open, open if closed)
        bool currentState = studentListPanel.activeSelf;
        studentListPanel.SetActive(!currentState);

        // If we are opening the panel right now, let's redraw the list from scratch so new kids are added
        if (!currentState)
        {
            UpdateStudentList();
        }
    }

    public void ProduceFocusSyrup()
    {
        // Check if there is enough money in the safe
        if (money >= potionCost)
        {
            money -= potionCost;         // Deduct money
            focusSyrupCount++;           // Add potion

            // Update on-screen texts instantly
            UpdateUI(); 
            UpdateLaboratoryUI();

            Debug.Log("[LAB] Chemical successfully boiled! New Potion produced.");
        }
        else
        {
            Debug.Log("[LAB] Safe is empty! Not enough money to produce potion.");
            // We can pop up an "Out of Money!" warning window here later
        }
    }

    public void UpdateLaboratoryUI()
    {
        focusSyrupText.text = "Owned Focus\nSyrup: " + focusSyrupCount;
    }

    public void ToggleLaboratoryPanel()
    {
        if (candidatePanel.activeSelf) return; // Don't open while interviewing

        bool currentState = laboratoryPanel.activeSelf;
        laboratoryPanel.SetActive(!currentState);

        if (!currentState)
        {
            UpdateLaboratoryUI(); // Write the current amount when the panel opens
        }
    }

    void UpdateUI()
    {
        moneyText.text = money + " TL";
        dayText.text = "Day: " + currentDay;
        suspicionText.text = "Suspicion: %" + suspicionLevel;

        if (potionText != null) potionText.text = "Potion: " + potionStock;
        if (studentText != null) studentText.text = "Student: " + studentCount;
        if (successText != null) successText.text = "Success: %" + successRate;
    }
}