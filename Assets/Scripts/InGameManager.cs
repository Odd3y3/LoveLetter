using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private InGameNetworkManager inGameNetworkManager;

    public SelectCardUI selectCardUI;
    public TextMeshProUGUI remainCardCount;
    
    public GameObject hand;

    private GameObject handCardInstance;

    const int MAX_CARD_COUNT = 8;

    GameObject[] cardSources;     //카드 리소스 ( 0번은 뒷면 )
    Vector3 cardDeckPos = new Vector3(-1f, 0.5f, 0f);            //카드 덱 위치
    Vector3 myHandCardPos = new Vector3(0f, -3.5f, 0f);         //내손에 있는 카드 위치

    int[] myHandCards;              //내손에 있는 카드, 없다면 0

    void Awake()
    {
        inGameNetworkManager = FindFirstObjectByType<InGameNetworkManager>();
        if (inGameNetworkManager == null)
            Debug.LogWarning("inGameNetworkManager is null.");

        myHandCards = new int[2];
        cardSources = new GameObject[MAX_CARD_COUNT + 1];

        //리소스에서 카드 로딩
        cardSources[0] = Resources.Load<GameObject>("Cards/BackCard");
        for(int i = 1; i <= MAX_CARD_COUNT; i++)
            cardSources[i] = Resources.Load<GameObject>("Cards/Card" + i);

    }

    private void FixedUpdate()
    {
        remainCardCount.text = inGameNetworkManager.GetRemainCardsCountInDeck().ToString();
    }


    public void DrawCard(int cardNum)
    {
        if (myHandCards[0] == 0)
        {
            myHandCards[0] = cardNum;
            handCardInstance = Instantiate(cardSources[cardNum], Vector3.zero, Quaternion.identity);
            handCardInstance.transform.SetParent(hand.transform, false);
        }
        else
        {
            myHandCards[1] = cardNum;
        }
    }

    public void MyTurnStart()
    {
        hand.SetActive(false);
        selectCardUI.gameObject.SetActive(true);
        selectCardUI.SetCard(myHandCards[0], myHandCards[1]);
    }

    /// <summary>
    /// 실제 카드를 사용하는 함수
    /// </summary>
    /// <param name="isLeft">왼쪽 카드를 사용 할 때 true</param>
    public void UseCard(bool isLeft)
    {
        int remainCardNum;
        int useCardNum;
        if (isLeft)
        {
            remainCardNum = myHandCards[1];
            useCardNum = myHandCards[0];
        }
        else
        {
            remainCardNum = myHandCards[0];
            useCardNum = myHandCards[1];
        }
        myHandCards[0] = 0;
        myHandCards[1] = 0;
        Destroy(handCardInstance); 
        DrawCard(remainCardNum);
        hand.SetActive(true);

        //카드를 사용하고 ( Card class 만들기 )***

        //CardOptionOrUseCard(useCardNum);

        //NetworkManager에게 전달하고, 턴 넘어감
        //(선택지가 있는 카드의 경우, 바로 넘어가지 않고, 선택할 옵션이 나오고 그 다음에 턴을 넘겨야함.)

        //inGameNetworkManager.SendMove(useCardNum);        SelectCardUI에서..??????
    }

}
