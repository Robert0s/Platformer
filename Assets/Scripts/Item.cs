using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    
    
    
    public string itemName;
    public Sprite icon;         
    public int maxStack = 5;    
    public GameObject projectilePrefab; 
}