using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionNumSelectUIButton : MonoBehaviour
    , IPointerEnterHandler, IPointerExitHandler
{

    public int optionCardNumber;
    [SerializeField]
    private GameObject previewCard;

    private InGameManager inGameManager;
    private TargetPlayerUI targetPlayerUI;

    private void Awake()
    {
        inGameManager = FindFirstObjectByType<InGameManager>();
        if (inGameManager == null)
            Debug.LogWarning("inGameManager is null.");

        targetPlayerUI = FindFirstObjectByType<TargetPlayerUI>();
        if (targetPlayerUI == null)
            Debug.LogWarning("targetPlayerUI is null.");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        previewCard.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        previewCard.SetActive(false);
    }

    public void SelectOption()
    {
        inGameManager.EndTurn(1, targetPlayerUI.GetTargetDir(), optionCardNumber);
        transform.parent.gameObject.SetActive(false);
    }
}
