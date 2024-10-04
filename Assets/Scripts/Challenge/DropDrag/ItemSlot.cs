using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour , IDropHandler
{
    // Start is called before the first frame update

    public GameObject slot;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(slot.transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            DragAndDrop item = dropped.GetComponent<DragAndDrop>();
            item.parentAfterDrag = transform;
        }
    }
}
