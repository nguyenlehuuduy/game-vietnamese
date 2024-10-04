using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotScript : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private bool isOccupied = false;
    private bool isRight = true;
    public ManagerGameMulWithImage manager;
    private DragAndDropCript dragAndDrop;
    void Start()
    {
        manager = FindAnyObjectByType<ManagerGameMulWithImage>();   
        dragAndDrop = FindAnyObjectByType<DragAndDropCript>();
        
    }
    public void OnDrop(PointerEventData eventData)
    {  
        if (eventData.pointerDrag != null)
        {
            Debug.Log("Drop");
            if (this.transform.childCount == 4)
            {
                isOccupied = true;
                Debug.Log("DropZone is occupied. Cannot drop component here.");
            }
            if (!isOccupied)
            {
                eventData.pointerDrag.GetComponent<Button>().transform.SetParent(this.transform);
                eventData.pointerDrag.GetComponent<Button>().transform.SetAsLastSibling();
            }
            else
            {
                string temp = eventData.pointerDrag.GetComponentInChildren<TextMeshProUGUI>().text;
                eventData.pointerDrag.GetComponentInChildren<TextMeshProUGUI>().text = this.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text;
                this.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text = temp;
            }
             
        }
    }
    
}
    
