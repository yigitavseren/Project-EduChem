using UnityEngine;

// Category determining which field the kid will take the exam in
public enum StudentField { Science, EqualWeight, Humanities }

[System.Serializable]
public class Student
{
    public string studentName;
    public StudentField field; // Science, Equal Weight, or Humanities?
    public int incomeContribution;

    // --- DYNAMIC SURVIVAL BARS (0 - 100) ---
    public int sanity;
    public int energy;
    public int loyalty;
    public int toxicity;

    // --- CORE RPG STATS (1 - 10) ---
    public int intelligence;
    public int physique;
    public int willpower;
    public int wealth;

    // --- COURSE SCORES (Current / Potential) ---
    public int mathScore, potentialMath;
    public int physicsScore, potentialPhysics;
    public int chemistryScore, potentialChemistry;
    public int bioScore, potentialBio;

    public int turkishScore, potentialTurkish;
    public int historyScore, potentialHistory;
    public int geoScore, potentialGeo;
    public int philosophyScore, potentialPhilosophy;

    public Student(int academySuccess)
    {
        // GIANT MALE & FEMALE FIRST NAME POOL
        string[] firstNames = {
            "James", "John", "Robert", "Michael", "William", "David", "Richard", "Charles", "Joseph", "Thomas",
            "Christopher", "Daniel", "Paul", "Mark", "Donald", "George", "Kenneth", "Steven", "Edward", "Brian",
            "Ronald", "Anthony", "Kevin", "Jason", "Matthew", "Gary", "Timothy", "Jose", "Larry", "Jeffrey",
            "Frank", "Scott", "Eric", "Stephen", "Andrew", "Raymond", "Gregory", "Joshua", "Jerry", "Dennis",
            "Walter", "Patrick", "Peter", "Harold", "Douglas", "Henry", "Carl", "Arthur", "Ryan", "Roger",
            "Mary", "Patricia", "Jennifer", "Linda", "Elizabeth", "Barbara", "Susan", "Jessica", "Sarah", "Karen",
            "Nancy", "Lisa", "Betty", "Margaret", "Sandra", "Ashley", "Kimberly", "Emily", "Donna", "Michelle",
            "Dorothy", "Carol", "Amanda", "Melissa", "Deborah", "Stephanie", "Rebecca", "Sharon", "Laura", "Cynthia",
            "Kathleen", "Amy", "Shirley", "Angela", "Helen", "Anna", "Brenda", "Pamela", "Nicole", "Emma",
            "Samantha", "Katherine", "Christine", "Debra", "Rachel", "Catherine", "Carolyn", "Janet", "Ruth", "Maria",
            "Heather", "Diane", "Virginia", "Julie", "Joyce", "Victoria", "Olivia", "Kelly", "Christina", "Lauren",
            "Joan", "Evelyn", "Judith", "Megan", "Cheryl", "Andrea", "Hannah", "Martha", "Jacqueline", "Frances",
            "Gloria", "Ann", "Teresa", "Kathryn", "Sara", "Janice", "Jean", "Alice", "Madison", "Doris",
            "Abigail", "Julia", "Judy", "Grace", "Denise", "Amber", "Marilyn", "Beverly", "Danielle", "Theresa",
            "Sophia", "Marie", "Diana", "Brittany", "Natalie", "Isabella", "Charlotte", "Rose", "Alexis", "Kayla",
            "Liam", "Noah", "Oliver", "Elijah", "Lucas", "Mason", "Logan", "Ethan"
        };

        // GIANT LAST NAME POOL
        string[] lastNames = {
            "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
            "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin",
            "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson",
            "Walker", "Young", "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill",
            "Flores", "Green", "Adams", "Nelson", "Baker", "Hall", "Rivera", "Campbell", "Mitchell", "Carter",
            "Roberts", "Gomez", "Phillips", "Evans", "Turner", "Diaz", "Parker", "Cruz", "Edwards", "Collins",
            "Reyes", "Stewart", "Morris", "Morales", "Murphy", "Cook", "Rogers", "Gutierrez", "Ortiz", "Morgan",
            "Cooper", "Peterson", "Bailey", "Reed", "Kelly", "Howard", "Ramos", "Kim", "Cox", "Ward",
            "Richardson", "Watson", "Brooks", "Chavez", "Wood", "James", "Bennett", "Gray", "Mendoza", "Ruiz",
            "Hughes", "Price", "Alvarez", "Castillo", "Sanders", "Patel", "Myers", "Long", "Ross", "Foster"
        };

        // Combine First and Last Name
        this.studentName = firstNames[Random.Range(0, firstNames.Length)] + " " + lastNames[Random.Range(0, lastNames.Length)];

        // 2. RANDOM FIELD SELECTION
        this.field = (StudentField)Random.Range(0, 3); // Returns 0, 1, or 2

        // 3. STAT DISTRIBUTION AND ECONOMY
        // Initialize Core RPG Stats (1 - 10)
        this.intelligence = Random.Range(1, 11);
        this.physique = Random.Range(1, 11);
        this.willpower = Random.Range(1, 11);
        this.wealth = Random.Range(1, 11);

        // Initialize Dynamic Survival Bars (0 - 100)
        this.sanity = 100;
        this.energy = 100;
        this.loyalty = 50;
        this.toxicity = 0;

        // Wealth replaces the strict manual income setting
        this.incomeContribution = this.wealth * Random.Range(40, 65);

        // 4. DETERMINE SCORES BASED ON FIELD
        switch (this.field)
        {
            case StudentField.Science:
                mathScore = GenerateScore(20, academySuccess, out potentialMath);
                physicsScore = GenerateScore(20, academySuccess, out potentialPhysics);
                chemistryScore = GenerateScore(20, academySuccess, out potentialChemistry);
                bioScore = GenerateScore(20, academySuccess, out potentialBio);
                break;

            case StudentField.EqualWeight:
                turkishScore = GenerateScore(40, academySuccess, out potentialTurkish);
                mathScore = GenerateScore(40, academySuccess, out potentialMath);
                break;

            case StudentField.Humanities:
                turkishScore = GenerateScore(20, academySuccess, out potentialTurkish);
                historyScore = GenerateScore(20, academySuccess, out potentialHistory);
                geoScore = GenerateScore(20, academySuccess, out potentialGeo);
                philosophyScore = GenerateScore(20, academySuccess, out potentialPhilosophy);
                break;
        }
    }

    // Engine that generates logical scores based on max questions and academy success
    private int GenerateScore(int maxQuestions, int academySuccess, out int potential)
    {
        // maxQuestions VALUE WILL NOW BE THE INCOMING VALUE (20 or 40)
        int currentScore;

        if (academySuccess < 30) // Lazy squad (0 - 40% score range)
        {
            currentScore = Random.Range(0, (int)(maxQuestions * 0.41f));
            potential = Random.Range(currentScore, (int)(maxQuestions * 0.71f));
        }
        else if (academySuccess < 70) // Mid-level (30% - 70% score range)
        {
            currentScore = Random.Range((int)(maxQuestions * 0.3f), (int)(maxQuestions * 0.71f));
            potential = Random.Range(currentScore, (int)(maxQuestions * 0.91f));
        }
        else // Top tier student (60% - Full score range)
        {
            currentScore = Random.Range((int)(maxQuestions * 0.6f), maxQuestions + 1);
            potential = maxQuestions;
        }

        return currentScore;
    }
}

