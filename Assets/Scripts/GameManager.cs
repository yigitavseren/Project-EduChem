using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI; // NEW: Added to change the card's background color (Science/Humanities)

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

        money += (studentCount * 100);
        money -= secretLabExpense;

        studentCount += (successRate / 10);

        if (suspicionLevel > 0)
        {
            suspicionLevel -= 5;
            if (suspicionLevel < 0) suspicionLevel = 0;
        }

        if (money <= 0)
        {
            moneyText.text = "BANKRUPT!";
            isGameOver = true;
            return;
        }

        waitingCandidates.Clear();
        for (int i = 0; i < 3; i++)
        {
            waitingCandidates.Add(new Student(successRate));
        }

        foreach (Student student in enrolledStudents)
        {
            money += student.incomeContribution;
        }

        // NEW: Open the panel to show candidates at the door when the day ends
        if (candidatePanel != null)
        {
            candidatePanel.SetActive(true);
            ShowNextCandidate();
        }

        UpdateUI();
    }

    // --- NEWLY ADDED PANEL FUNCTIONS START HERE ---
    public void ShowNextCandidate()
    {
        if (waitingCandidates.Count > 0)
        {
            currentlyDisplayedCandidate = waitingCandidates[0]; // Get the first kid from the list

            cardNameText.text = currentlyDisplayedCandidate.studentName;
            cardIncomeText.text = "Expected Income: " + currentlyDisplayedCandidate.incomeContribution + " TL/Day";

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
        // First, let's clear the old rows in the list
        foreach (Transform child in studentListContent)
        {
            Destroy(child.gameObject);
        }

        // Generate a new row for each student in the academy
        foreach (Student student in enrolledStudents)
        {
            // Create a new row from the prefab and put it inside the Content
            GameObject newRow = Instantiate(studentRowPrefab, studentListContent);

            // Find the Texts inside it sequentially
            TextMeshProUGUI[] texts = newRow.GetComponentsInChildren<TextMeshProUGUI>();

            texts[0].text = student.studentName;
            texts[1].text = "Field: " + student.field.ToString();
            texts[2].text = "Income: " + student.incomeContribution + " TL/Day";

            // Determine the text to be written in the 4th Text (Scores) based on the kid's field
            string scoreText = "";
            if (student.field == StudentField.Science)
            {
                scoreText = $"Math: {student.mathScore} | Phys: {student.physicsScore} | Chem: {student.chemistryScore} | Bio: {student.bioScore}";
            }
            else if (student.field == StudentField.EqualWeight)
            {
                scoreText = $"Turk: {student.turkishScore} | Math: {student.mathScore}";
            }
            else if (student.field == StudentField.Humanities)
            {
                scoreText = $"Turk: {student.turkishScore} | Hist: {student.historyScore} | Geo: {student.geoScore} | Phil: {student.philosophyScore}";
            }

            texts[3].text = scoreText; // Print the scores to the 4th Text
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