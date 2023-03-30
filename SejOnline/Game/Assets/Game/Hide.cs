using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Hide : MonoBehaviour
{
    [SerializeField] private CanvasGroup CanvasGroup;

    public void Update()
    {
        if (IsMouseOverGameLog())
        {
            StopAllCoroutines();
            ShowGameLog();
        }
        else
        {
            StartCoroutine(HideGameLog());
        }
        
    }
    
    private bool IsMouseOverGameLog()
    {
        PointerEventData PointerEventData = new PointerEventData(EventSystem.current);
        PointerEventData.position = Input.mousePosition;

        List<RaycastResult> RaycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(PointerEventData, RaycastResults);
        for (int i = 0; i < RaycastResults.Count; i++)
        {
            if (RaycastResults[i].gameObject.tag != "GameLog")
            {
                RaycastResults.RemoveAt(i);
                i--;
            }
        }

        return RaycastResults.Count > 0;
    }

    IEnumerator HideGameLog()
    {
        yield return new WaitForSeconds(10);
        CanvasGroup.alpha = 0;
    }

    public void ShowGameLog()
    {
        CanvasGroup.alpha = 1;
    }
}
