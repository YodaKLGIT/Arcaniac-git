using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellWheel : MonoBehaviour
{
    public GameObject spellWheel; // Reference to the spell wheel (the parent object that holds all icons)
    public GameObject spellIconPrefab; // Assign a UI Button prefab
    public Transform centerPoint; // Center of the spell wheel (this should be a UI object inside the canvas)
    public float radius = 150f; // The distance from the center to the buttons
    public List<SpellData> spells; // List of spells assigned in the inspector

    private List<GameObject> spellIcons = new List<GameObject>();
    private List<SpellData> selectedSpells = new List<SpellData>();

    private bool isWheelOpen = false; // Track the open/close state of the spell wheel

    void Start()
    {
        // Make sure the spell wheel is closed at the start
        spellWheel.SetActive(false);

        // Create the spell wheel UI
        CreateSpellWheel();
    }

    void Update()
    {
        // Check for right mouse button click to open or close the spell wheel
        if (Input.GetKeyDown(KeyCode.E)) // Right mouse button (RMB) pressed
        {
            ToggleSpellWheel();
        }
    }

    void CreateSpellWheel()
    {
        // Clear previous spell icons
        foreach (var icon in spellIcons)
        {
            Destroy(icon);
        }
        spellIcons.Clear();

        RectTransform spellWheelRect = spellWheel.GetComponent<RectTransform>(); // Canvas UI object
        float canvasScaleFactor = spellWheelRect.lossyScale.x; // Fix scaling issues

        for (int i = 0; i < spells.Count; i++)
        {
            float angle = i * (360f / spells.Count);
            float angleRad = angle * Mathf.Deg2Rad;

            // Calculate local UI position (keeping it within the 1920x1080 space)
            Vector2 localPosition = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * (radius / canvasScaleFactor);

            // Instantiate UI button
            GameObject spellIcon = Instantiate(spellIconPrefab, spellWheel.transform);
            spellIcon.name = spells[i].spellName;

            // Set UI element position
            RectTransform iconRect = spellIcon.GetComponent<RectTransform>();
            iconRect.anchoredPosition = localPosition; // Correct position in UI space
            iconRect.localScale = Vector3.one; // Reset scale

            // Assign spell data
            spellIcon.GetComponent<Image>().sprite = spells[i].spellIcon;
            spellIcon.GetComponent<Button>().onClick.AddListener(() => SelectSpell(spells[i], spellIcon));

            spellIcons.Add(spellIcon);
        }
    }


    void ToggleSpellWheel()
    {
        // Toggle the visibility of the spell wheel
        isWheelOpen = !isWheelOpen;
        spellWheel.SetActive(isWheelOpen); // Activate or deactivate spell wheel

        // Pause or resume the game
        if (isWheelOpen)
        {
            Time.timeScale = 0f; // Pause the game (optional)
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Make cursor visible
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor back
            Cursor.visible = false; // Hide cursor
        }
    }


    void SelectSpell(SpellData spell, GameObject spellIcon)
    {
        if (selectedSpells.Contains(spell))
        {
            selectedSpells.Remove(spell);
            spellIcon.GetComponent<Image>().color = Color.white; // Deselect
        }
        else if (selectedSpells.Count < 4)
        {
            selectedSpells.Add(spell);
            spellIcon.GetComponent<Image>().color = Color.green; // Highlight selection
        }
    }

    public void ConfirmSelection()
    {
        if (selectedSpells.Count >= 2)
        {
            Debug.Log("Spells selected: " + string.Join(", ", selectedSpells.ConvertAll(s => s.spellName)));
            // Send selectedSpells to the spell casting system
        }
    }
}
