using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Card Logic attribute")]
    [Tooltip("Prefabs for card and particle system")]
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject cardBrekePrefab;
    [Tooltip("The plane is for the particle system physics")]
    [SerializeField] Transform aPlane;
    [Tooltip("CardSO stores enum Name and sprite information")]
    [SerializeField] List<CardSO> cardsData;
    [Tooltip("Card component that being clicked on, debug purpose only, private")]
    [SerializeField] private Card selectedCard;
    [Tooltip("Not changing it, just positions for card holders in hand")]
    [SerializeField] List<Transform> cardHolderSlots;
    [Tooltip("How fast the animation goes")]
    [SerializeField] float cardMoveSpeed;
    private List<Transform> cardsInHand = new List<Transform>();
    int ptrInHand = 0;//so we know where to put the next card
    // Start is called before the first frame update
    void Start()
    {
        string[] dataPaths = AssetDatabase.FindAssets("t:CardSO"); // Search for all CardSO

        foreach (string path in dataPaths)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(path);
            CardSO cardSO = AssetDatabase.LoadAssetAtPath<CardSO>(assetPath);

            if (cardSO != null)
            {
                cardsData.Add(cardSO);
            }
        }
    }
    void Update()
    {
        DetectClick();
    }
    public Card SpawnCard(Vector3 pos)
    {
        Card cardSpawned= Instantiate(cardPrefab, pos, Quaternion.identity).GetComponent<Card>();
        return cardSpawned;
    }
    #region Card Function
    private void DetectClick()
    {
        // Check for touch input
        if (Input.touchCount > 0 && !IsGamePaused)
        {
            // Get the first touch (you can modify this to handle multiple touches)
            Touch touch = Input.GetTouch(0);
            MoveCardToLeft();
            // Check if the touch phase is the beginning of a touch (i.e., when the finger touches the screen)
            if (touch.phase == TouchPhase.Began)
            {
                // Convert the touch position to a world position
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                // Perform the raycast
                RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

                if (hit.collider != null)
                {
                    // A collision was detected. You can access the collided object using hit.collider.
                    //Debug.Log("Hit object: " + hit.collider.gameObject.name);
                    if (hit.collider.TryGetComponent<Card>(out selectedCard))
                    {
                        //selectedCard.SetUP(cardsData[Random.Range(0, 99)]);
                        if (ptrInHand < 7)
                        {
                            remainingCardsOnBoard--;
                            ptrInHand++;
                            hit.collider.enabled = false;
                            StartCoroutine(AddCardToHand(selectedCard.transform, cardHolderSlots[ptrInHand - 1]));
                        }

                    }
                }
                CheckEndGame();
            }
            //if (touch.phase == TouchPhase.Ended)
            //{
            //    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            //    touchPosition = new Vector3 (touchPosition.x, touchPosition.y, 0);
            //    SpawnCard(touchPosition).SetUP(cardsData[Random.Range(0,99)]);
            //}

        }
    }
    private IEnumerator AddCardToHand(Transform cardTran,Transform targetTran)
    {
        float journeyLength = Vector3.Distance(transform.position, targetTran.position);
        float startTime = Time.time;
        while (cardTran.position != targetTran.position)
        {
            if (cardTran==null)
            {
                break;
            }
            float distanceCovered = (Time.time - startTime) * cardMoveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            cardTran.position = Vector3.Lerp(cardTran.position, targetTran.position, fractionOfJourney);
            yield return null;
        }
        cardsInHand.Add(cardTran);
        StartCoroutine(CheckForTriple());
    }
    private IEnumerator CheckForTriple()
    {
        Dictionary<CardSO.Name,int> checkList = new Dictionary<CardSO.Name,int>();
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            if (checkList.Keys.Contains(cardsInHand[i].GetComponent<Card>().GetName()))
            {
                CardSO.Name currCardName = cardsInHand[i].GetComponent<Card>().GetName();
                checkList[currCardName] = checkList[currCardName] + 1;
                if (checkList[currCardName] >= 3)
                {
                    EliminateThree(currCardName);
                }
            }
            else
            {
                checkList.Add(cardsInHand[i].GetComponent<Card>().GetName(), 1);
            }
        }
    }

    private void EliminateThree(CardSO.Name cardName)
    {
        List<Transform> tranToRemove = new List<Transform>();
        for (int i=0;i< cardsInHand.Count;i++)
        {
            if (cardsInHand[i].GetComponent<Card>().GetName() == cardName)
            {
                tranToRemove.Add(cardsInHand[i]);
                if (tranToRemove.Count==3)
                {
                    break;//just 3
                }
            }
        }
        for (int i=0;i<3;i++)//remove just 3
        {
            if (tranToRemove[i] == null) continue;//just in case
            cardsInHand.Remove(tranToRemove[i]);
            Instantiate(cardBrekePrefab, tranToRemove[i].position,Quaternion.identity).GetComponent<ParticleSystem>().collision.AddPlane(aPlane);
            ptrInHand--;
            Destroy(tranToRemove[i].gameObject);
        }
        MoveCardToLeft();//remap acording to the slots
    }
    private void MoveCardToLeft()//remap acording to the slots
    {
        if (cardsInHand.Count<=0)
        {
            return;
        }
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            StartCoroutine(MoveCardLeft(cardsInHand[i], cardHolderSlots[i]));
        }
    }
    private IEnumerator MoveCardLeft(Transform cardTran, Transform targetTran)
    {
        float journeyLength = Vector3.Distance(transform.position, targetTran.position);
        float startTime = Time.time;
        while (cardTran != null&&cardTran.position != targetTran.position)
        {
            if (cardTran==null)
            {
                break;
            }
            float distanceCovered = (Time.time - startTime) * cardMoveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            cardTran.position = Vector3.Lerp(cardTran.position, targetTran.position, fractionOfJourney);
            yield return null;
        }
    }
    #endregion
    #region Game Management
    [Tooltip("should not be 0 at start,deciding if the game ends")]
    [SerializeField] int remainingCardsOnBoard;
    private void CheckEndGame()
    {
        StartCoroutine(CheckWonGame());//endGame --> Win
        if (ptrInHand>=7)
        {
            StartCoroutine(CheckLoseGame());
        }
    }
    IEnumerator CheckLoseGame()
    {
        yield return new WaitForSeconds(0.5f);
        if (ptrInHand >= 7)
        {
            //end Game-->lose
            Debug.Log("Game Over");
        }
    }
    IEnumerator CheckWonGame()
    {
        yield return new WaitForSeconds(1f);
        if (ptrInHand == 0 && remainingCardsOnBoard == 0)
        {
            //endGame --> Win
            Debug.Log("You Won");
        }
    }
    #endregion
    #region Pause Menu
    [Header("For Menu ")]
    [Tooltip("Game Object of UI: pause menu")]
    [SerializeField] GameObject PauseMenu;
    [SerializeField] bool IsGamePaused= false;
    public void ActivePauseMenu()
    {
        PauseMenu.SetActive(true);
        IsGamePaused = true;
    }
    public void Resume()
    {
        PauseMenu.SetActive(false);
        IsGamePaused = false;
    }
    public void Levels()
    {
        //go to level sence
        throw new NotImplementedException("Levels scene is not implemented");
    }
    public void Setting()
    {
        //get settings up
        throw new NotImplementedException("Setting menu is not implemented");
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    public void Exit()
    {
        Application.Quit();
    }
    #endregion
}
