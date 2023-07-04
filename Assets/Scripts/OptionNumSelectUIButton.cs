using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionNumSelectUIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject previewCard;


    /// <summary>
    /// 버튼 위로 마우스가 올라왔을 때, preview 카드를 Enable함.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        previewCard.SetActive(true);
    }



    /// <summary>
    /// 버튼 위에서 마우스가 내려왔을 때, preview 카드를 Disable함.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        previewCard.SetActive(false);
    }

}
