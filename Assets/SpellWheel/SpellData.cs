using UnityEngine;

[CreateAssetMenu(fileName = "New SpellData", menuName = "SpellWheel/SpellData", order = 1)]
public class SpellData : ScriptableObject
{
    public string spellName;
    public Sprite spellIcon;
    public GameObject projectilePrefab;
    public Color backgroundColor;
    public AudioClip hitSound;

    //status effects
    public BaseStatusEffect statusEffect;
    public float statusBuildupAmount = 25f;
}
