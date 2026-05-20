\# UNDERGROUND ACADEMY - GAME DESIGN \& CONTEXT DOCUMENT

\*\*Theme:\*\* "Breaking Bad" meets "Hababam Sınıfı" (Underground Illegal Education / Tycoon).

\*\*Current State:\*\* Phase 1 (MVP) is being developed. The UI and Codebase have been fully localized to English to target the global market on Steam.



\## 1. CORE MECHANICS \& BARS (Must be integrated into Student.cs)

Each student has two types of values. 



\### A. Dynamic Survival Bars (0 to 100)

These fluctuate daily based on potions and stress.

\* \*\*Sanity:\*\* Decreases by studying or taking heavy potions. If 0, the student has a mental breakdown.

\* \*\*Energy:\*\* Determines if they stay awake during class.

\* \*\*Loyalty:\*\* Starts at \*\*50 (Neutral)\*\*. Determines whistleblower risk. 0-30 = Snitch, 70-100 = Loyal Assistant.

\* \*\*Toxicity:\*\* Chemical buildup. Starts at 0. Reaching 100 means an overdose (hospitalization).



\### B. Core RPG Stats (1 to 10)

Base genetics determined during the admission interview.

\* \*\*Intelligence:\*\* Raw learning speed.

\* \*\*Physique:\*\* Resistance to Toxicity.

\* \*\*Willpower:\*\* Resistance to Sanity and Loyalty drops.

\* \*\*Wealth:\*\* Determines daily income contribution (replaces strict manual income setting).



\## 2. POTION SYSTEM (Tier 1 - For Phase 1)

\* \*\*Grey Matter (Analytical Boost):\*\* Maxes Intelligence temporarily. Costs Sanity and adds Toxicity.

\* \*\*Whispering Water (Humanities Extract):\*\* Boosts memory but increases Teacher's Suspicion.

\* \*\*Caffeine Coma:\*\* Fills Energy to 100, but high heart attack risk if Physique is low.



\## 3. GLOBAL UI NAMING CONVENTIONS (Already in Unity Hierarchy)

Do not use Turkish words. The following UI mapping is strict:

\* Kasa -> Safe / Money

\* Gün -> Day

\* Öğrenci -> Enrolled / Students

\* Başarı -> Score / GPA

\* Şüphe -> Suspicion

\* İksir -> Potions

\* UstBar -> TopBar

\* AltBar -> BottomBar

\* AdayPaneli -> CandidatePanel

\* OgrenciSatiri (Prefab) -> StudentRow



\## 4. CURRENT C# SCRIPTS

\*\*`Student.cs`\*\*: Enum `StudentField` { Science, EqualWeight, Humanities }. Contains massive English name/surname arrays. Needs the new RPG Stats and Dynamic Bars added.

\*\*`GameManager.cs`\*\*: Handles UI updates (`UpdateUI()`), day progression (`EndDay()`), and candidate panel logic. Connected to `TextMeshProUGUI` elements.



\## 5. AGENT INSTRUCTIONS (ANTIGRAVITY DIRECTIVE)

1\. Read this context fully.

2\. You have Unity MCP access. You can directly edit `.cs` files in the `Assets/Scripts` folder and interact with the Unity Editor.

3\. Keep the "Aga" (Turkish slang for Bro/Mate) persona when communicating with the human developer, but write 100% clean, professional English C# code.

4\. Your first task is to update `Student.cs` with the 4 Dynamic Bars and 4 RPG Stats mentioned above.

