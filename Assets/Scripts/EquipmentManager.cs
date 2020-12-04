using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton

    public static EquipmentManager instance;

    void Awake(){
        instance = this;
    }

    #endregion

    public Equipment[] defaultItems;
    public SkinnedMeshRenderer targetMesh;
    Equipment[] currentEquipment;
    SkinnedMeshRenderer[] currentMeshes;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    Inventory inventory;

    void Start(){
        inventory = Inventory.instance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        currentMeshes = new SkinnedMeshRenderer[numSlots];

        EquipDefaultItems();
    }

    public void Equip(Equipment newItem){

        // Find out what slot the item fits in
        int slotIndex = (int) newItem.equipSlot;
        Equipment oldItem = Unequip(slotIndex);


        // An item has been equipped so we trigger the callback
        if(onEquipmentChanged != null){
            onEquipmentChanged.Invoke(newItem, oldItem);
        }

        // Insert the item into the slot
        currentEquipment[slotIndex] = newItem;
        SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.mesh);
        newMesh.transform.parent = targetMesh.transform;

        newMesh.bones = targetMesh.bones;
        newMesh.rootBone = targetMesh.rootBone;
        currentMeshes[slotIndex] = newMesh;
    }

    public Equipment Unequip(int slotIndex){
        
        // Only do this if an item is there 
        if(currentEquipment[slotIndex] != null){

            if(currentMeshes[slotIndex] != null){
                Destroy(currentMeshes[slotIndex].gameObject);
            }

            // Add the item to the inventory
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            // Remove the item from the inventory
            currentEquipment[slotIndex] = null;

            // Equipment has been removed so we trigger the callback
            if(onEquipmentChanged != null){
                onEquipmentChanged.Invoke(null, oldItem);
            }
            return oldItem;
        }
        return null;
    }

    public void UnequipAll(){
        for(int i = 0; i < currentEquipment.Length; i++){
            Unequip(i);
        }

        EquipDefaultItems();
    }

    void EquipDefaultItems(){
        foreach(Equipment item in defaultItems){
            Equip(item);
        }
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.U)){
            UnequipAll();
        }
    }

}
