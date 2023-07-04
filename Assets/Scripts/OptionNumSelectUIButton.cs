using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionNumSelectUIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject previewCard;


    /// <summary>
    /// ��ư ���� ���콺�� �ö���� ��, preview ī�带 Enable��.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        previewCard.SetActive(true);
    }



    /// <summary>
    /// ��ư ������ ���콺�� �������� ��, preview ī�带 Disable��.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        previewCard.SetActive(false);
    }

}
