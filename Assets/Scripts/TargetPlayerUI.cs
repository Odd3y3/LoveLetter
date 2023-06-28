using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlayerUI : MonoBehaviour
{
    enum Dir
    {
        None = 0,
        Top,
        Left,
        Right
    }

    private InGameManager inGameManager;

    private CardBase useCard;
    private Dir targetDir = Dir.None;

    private void Awake()
    {
        inGameManager = FindFirstObjectByType<InGameManager>();
        if (inGameManager == null)
            Debug.LogWarning("inGameManager is null.");
    }

    public void SetCard(CardBase useCard)
    {
        this.useCard = useCard;
    }

    public void OnClickTopPlayer()
    {
        targetDir = Dir.Top;
        UseCard();
    }

    public void OnClickLeftPlayer()
    {
        targetDir = Dir.Left;
        UseCard();
    }

    public void OnClickRightPlayer()
    {
        targetDir = Dir.Right;
        UseCard();
    }

    private void UseCard()
    {
        if (useCard.IsRequireOptionNum)
        {
            //IsRequireOptionNum �� true �϶�, (����� ī�� 1�� �� ����)
            //�� ���� UI ����

            //���� �ڵ�
            inGameManager.EndTurn(useCard.Num);
        }
        else
        {
            inGameManager.EndTurn(useCard.Num);
        }
        DisableTargetPlayerUI();
    }

    private void DisableTargetPlayerUI()
    {
        useCard = null;
        targetDir = Dir.None;
        gameObject.SetActive(false);
    }
}
