using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Dir
{
    None = 0,
    Left,
    Top,
    Right
}
public class TargetPlayerUI : MonoBehaviour
{

    private InGameManager inGameManager;
    [SerializeField]
    private OptionNumSelectUI optionNumSelectUI;

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

    public int GetTargetDir()
    {
        return (int)targetDir;
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
            //IsRequireOptionNum 이 true 일때, (현재는 카드 1번 일 때만)
            //그 다음 UI 띄우기

            optionNumSelectUI.gameObject.SetActive(true);
            optionNumSelectUI.SetTargetInfo((int)targetDir);
        }
        else
        {
            inGameManager.EndTurn(useCard.Num, (int)targetDir);
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
