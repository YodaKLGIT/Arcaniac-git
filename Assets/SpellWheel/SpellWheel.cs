using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellWheel : MonoBehaviour
{
    public GameObject spellWheel;
    public GameObject spellIconPrefab;
    public Transform centerPoint;
    public float radius = 150f;
    public List<SpellData> spells;

    private List<GameObject> spellIcons = new List<GameObject>();
    private List<SpellData> selectedSpells = new List<SpellData>();
    private bool isWheelOpen = false;

    public delegate void SpellConfirmedHandler(List<SpellData> spells);
    public event SpellConfirmedHandler OnSpellsConfirmed;

    private FPSShooter fpsShooter;

    void Start()
    {
        spellWheel.SetActive(false);
        CreateSpellWheel();
        fpsShooter = FindObjectOfType<FPSShooter>(); // Find FPSShooter script in the scene
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleSpellWheel();
        }
    }

    void CreateSpellWheel()
    {
        foreach (var icon in spellIcons)
        {
            Destroy(icon);
        }
        spellIcons.Clear();

        RectTransform spellWheelRect = spellWheel.GetComponent<RectTransform>();
        float canvasScaleFactor = spellWheelRect.lossyScale.x;

        for (int i = 0; i < spells.Count; i++)
        {
            float angle = i * (360f / spells.Count);
            float angleRad = angle * Mathf.Deg2Rad;

            Vector2 localPosition = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * (radius / canvasScaleFactor);

            GameObject spellIcon = Instantiate(spellIconPrefab, spellWheel.transform);
            spellIcon.name = spells[i].spellName;

            RectTransform iconRect = spellIcon.GetComponent<RectTransform>();
            iconRect.anchoredPosition = localPosition;
            iconRect.localScale = Vector3.one;

            spellIcon.GetComponent<Image>().sprite = spells[i].spellIcon;
            int index = i;
            spellIcon.GetComponent<Button>().onClick.AddListener(() => SelectSpell(spells[index], spellIcon));

            spellIcons.Add(spellIcon);
        }
    }

    void ToggleSpellWheel()
    {
        isWheelOpen = !isWheelOpen;
        spellWheel.SetActive(isWheelOpen);

        if (isWheelOpen)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ConfirmSelection();
        }
    }

    void SelectSpell(SpellData spell, GameObject spellIcon)
    {
        if (selectedSpells.Contains(spell))
        {
            selectedSpells.Remove(spell);
            spellIcon.GetComponent<Image>().color = Color.white;
        }
        else if (selectedSpells.Count < 4)
        {
            selectedSpells.Add(spell);
            spellIcon.GetComponent<Image>().color = Color.green;
        }

        // Immediately update FPSShooter when selection changes
        if (fpsShooter != null)
        {
            fpsShooter.SetSelectedSpells(selectedSpells);
        }
    }


    void ConfirmSelection()
    {
        if (selectedSpells.Count >= 1) // Ensure at least 1 spell is selected
        {
            Debug.Log("Spells confirmed: " + string.Join(", ", selectedSpells.ConvertAll(s => s.spellName)));

            OnSpellsConfirmed?.Invoke(selectedSpells);
            if (fpsShooter != null)
            {
                fpsShooter.SetSelectedSpells(selectedSpells);
            }
        }
    }
}
