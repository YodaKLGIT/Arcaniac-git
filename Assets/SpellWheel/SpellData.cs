using UnityEngine;

[CreateAssetMenu(fileName = "New SpellData", menuName = "SpellWheel/SpellData", order = 1)]
public class SpellData : ScriptableObject
{
    public string spellName;
    public Sprite spellIcon;
    public int damage;
    public float cooldown;
    public GameObject projectilePrefab;
}
