using Photon.Pun.Demo.PunBasics;
using UnityEngine;

public class SelectCardUI : MonoBehaviour
{
    private InGameNetworkManager inGameNetworkManager;
    private InGameManager inGameManager;

    private GameObject leftCard;
    private GameObject rightCard;
    private GameObject selectButton;

    private GameObject[] cardSources;
    private const int MAX_CARD_COUNT = 8;

    private GameObject leftCardInstance;
    private GameObject rightCardInstance;

    private int leftCardNum;
    private int rightCardNum;

    private int selectedCard = 0;       // 0이면 선택 안됨, 1이면 왼쪽, 2이면 오른쪽

    private Vector3 selectedCardScale = new Vector3(1.2f, 1.2f, 1.0f);
    private Vector3 nonSelectedCardScale = new Vector3(0.8f, 0.8f, 1.0f);
    private Vector3 originalCardScale = new Vector3(1.0f, 1.0f, 1.0f);

    private void Awake()
    {
        inGameNetworkManager = FindFirstObjectByType<InGameNetworkManager>();
        if (inGameNetworkManager == null)
            Debug.LogWarning("inGameNetworkManager is null.");

        inGameManager = FindFirstObjectByType<InGameManager>();
        if (inGameManager == null)
            Debug.LogWarning("inGameManager is null.");


        leftCard = transform.Find("LeftCard").gameObject;
        rightCard = transform.Find("RightCard").gameObject;
        selectButton = transform.Find("SelectButton").gameObject;

        cardSources = new GameObject[MAX_CARD_COUNT + 1];
        cardSources[0] = Resources.Load<GameObject>("UICards/BackCard");
        for (int i = 1; i <= MAX_CARD_COUNT; i++)
            cardSources[i] = Resources.Load<GameObject>("UICards/Card" + i);
    }

    public void SetCard(int leftCardNum, int rightCardNum)
    {
        this.leftCardNum = leftCardNum;
        leftCardInstance = Instantiate(cardSources[leftCardNum], Vector3.zero, Quaternion.identity);
        leftCardInstance.transform.SetParent(leftCard.transform, false);

        this.rightCardNum = rightCardNum;
        rightCardInstance = Instantiate(cardSources[rightCardNum], Vector3.zero, Quaternion.identity);
        rightCardInstance.transform.SetParent(rightCard.transform, false);
    }

    public void ClickLeftCard()
    {
        leftCard.transform.localScale = selectedCardScale;
        rightCard.transform.localScale = nonSelectedCardScale;
        selectedCard = 1;
        selectButton.SetActive(true);
    }

    public void ClickRightCard()
    {
        leftCard.transform.localScale = nonSelectedCardScale;
        rightCard.transform.localScale = selectedCardScale;
        selectedCard = 2;
        selectButton.SetActive(true);
    }

    /// <summary>
    /// 카드 선택 버튼을 눌렀을 때 호출되는 함수.
    /// </summary>
    public void SelectCard()
    {
        int useCardNum = 0;
        if (selectedCard == 1)
        {
            inGameManager.UseCard(true);
            useCardNum = leftCardNum;
        }
        else if (selectedCard == 2)
        {
            inGameManager.UseCard(false);
            useCardNum = rightCardNum;
        }
        else
        {
            Debug.LogWarning("카드가 선택되지 않음.");
            return;
        }

        DisableSelectCardUI();

        //여기에 UI설정하는 거?/???????????
        //카드 번호에 따라서 상대 선택하는 UI
        //그리고 1번은 몇번인지 정하는거 까지 해야댐

        //inGameNetworkManager.SendMove(useCardNum);
    }


    /// <summary>
    /// SelectCardUI 없애는 함수.
    /// </summary>
    private void DisableSelectCardUI()
    {
        SetSelectedCardToZero();
        Destroy(leftCardInstance);
        Destroy(rightCardInstance);
        gameObject.SetActive(false);
    }


    /// <summary>
    /// 자신이 Select한 카드 초기화 하는 함수. ( 처음에 선택하지 않은 상태로 바꾸는 함수 )
    /// </summary>
    private void SetSelectedCardToZero()
    {
        leftCard.transform.localScale = originalCardScale;
        rightCard.transform.localScale = originalCardScale;
        selectedCard = 0;
        selectButton.SetActive(false);
    }


    // 카드 추가 옵션 UI


}
