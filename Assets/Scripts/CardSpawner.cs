using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Vector3Int boardSize = new Vector3Int(3,1,1);
    [SerializeField] int cardComplexity=24;
    private Vector2 spawnOffset;
    // Start is called before the first frame update
    void Start()
    {
        InitialTheBoard();
    }
    
    public void InitialTheBoard()
    {
        gameManager = GameManager.instance;
        spawnOffset = new Vector2(-(boardSize.x-1) * 1.5f, -boardSize.y * 1.5f + 6.5f);//to center the board
        gameManager.SetRemainingCardsOnBoard(boardSize.x* boardSize.y* boardSize.z);//boardSize should not be 0;
        NoramlCardCombination(boardSize.x, boardSize.y, boardSize.z);
    }
    //TODO: use some pattern for the cards board, to let player see below
    //TODO: give player somthing hard to play. kind of check
    //center of screen is (0, 5), each takes 3 in x
    /// <summary>
    /// no shuffle, three as a set and filling the borad
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    public void TooEasyCardCombination(int width, int height , int depth)
    {
        int counterForThree = 0;
        int randSONum = Random.Range(0, cardComplexity);
        for (int i = 0; i < depth; i++)
        {
            for(int j = 0; j < width; j++)
            {
                for (int h = 0; h< height; h++)
                {
                    Vector3 spawnPosition = new Vector3(j * 3.0f + spawnOffset.x, h * 3.2f + spawnOffset.y, i * 0.01f);
                    spawnPosition = PatternByLayer(i, spawnPosition);
                    Card cardToSpawn = Instantiate(cardPrefab, spawnPosition, Quaternion.identity, transform).GetComponent<Card>();
                    if (counterForThree > 2)
                    {
                        randSONum = Random.Range(0, cardComplexity);
                        counterForThree = 0;
                    }
                    cardToSpawn.SetUP(gameManager.GetCardsData()[randSONum]);
                    counterForThree++;
                }
            }
        }
    }
    /// <summary>
    /// change pattern acordding to depth as odd or even number
    /// </summary>
    /// <param name="depth"></param>
    /// <param name="spawnPosition"></param>
    /// <returns></returns>
    private static Vector3 PatternByLayer(int depth, Vector3 spawnPosition)
    {
        if (depth % 2 == 0)
        {
            float xOffset = 0.2f;
            float yOffset = 0.2f;
            if (spawnPosition.x <= 0)
            {
                xOffset = -xOffset;
                if (spawnPosition.x == 0)
                {
                    xOffset = 0;
                }
            }
            if (spawnPosition.y - 5 <= 0)
            {
                yOffset = -yOffset;
                if (spawnPosition.y == 0)
                {
                    yOffset = 0;
                }
            }
            spawnPosition.x += xOffset;
            spawnPosition.y += yOffset;
        }

        return spawnPosition;
    }
    /// <summary>
    /// Each layer is shuffled
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    public void EasyCardCombination(int width, int height, int depth)
    {
        int counterForThree = 0;
        int randSONum = Random.Range(0, cardComplexity);
        int[,] aLayer = new int[width,height];
        
        for (int i = 0; i < depth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                for (int h = 0; h < height; h++)
                {
                    if (counterForThree > 2)
                    {
                        randSONum = Random.Range(0, cardComplexity);
                        counterForThree = 0;
                    }
                    aLayer[j, h] = randSONum;
                    counterForThree++;
                }
            }
            aLayer = Shuffle(aLayer);
            for (int j = 0; j < width; j++)
            {
                for (int h = 0; h < height; h++)
                {
                    Vector3 spawnPosition = new Vector3(j * 3.0f + spawnOffset.x, h * 3.2f + spawnOffset.y, i * 0.01f);
                    Card cardToSpawn = Instantiate(cardPrefab, spawnPosition, Quaternion.identity, transform).GetComponent<Card>();
                    cardToSpawn.SetUP(gameManager.GetCardsData()[aLayer[j,h]]);
                }
            }
        }
    }
    /// <summary>
    /// All the order of cards are shuffled
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    public void NoramlCardCombination(int width, int height, int depth)
    {
        int counterForThree = 0;
        int randSONum = Random.Range(0, cardComplexity);
        int[,,] wholeSet = new int[width, height,depth];

        for (int i = 0; i < depth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                for (int h = 0; h < height; h++)
                {
                    if (counterForThree > 2)
                    {
                        randSONum = Random.Range(0, cardComplexity);
                        counterForThree = 0;
                    }
                    wholeSet[j, h, i] = randSONum;
                    counterForThree++;
                }
            }
        }
        wholeSet = Shuffle(wholeSet);
        for (int i =0; i< depth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                for (int h = 0; h < height; h++)
                {
                    Vector3 spawnPosition = new Vector3(j * 3.0f + spawnOffset.x, h * 3.2f + spawnOffset.y, i * 0.01f);
                    spawnPosition = PatternByLayer(i, spawnPosition);
                    Card cardToSpawn = Instantiate(cardPrefab, spawnPosition, Quaternion.identity, transform).GetComponent<Card>();
                    cardToSpawn.SetUP(gameManager.GetCardsData()[wholeSet[j, h, i]]);
                }
            }
        }
    }
    /// <summary>
    /// All the order of cards are shuffled, and the category of the cards are maximumed
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    public void HardCardCombination(int width, int height, int depth)
    {
        int counterForThree = 0;
        int randSONum = Random.Range(0, cardComplexity);
        int[,,] wholeSet = new int[width, height, depth];

        for (int i = 0; i < depth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                for (int h = 0; h < height; h++)
                {
                    if (counterForThree > 2)
                    {
                        randSONum++;
                        if (randSONum >= 24)
                        {
                            randSONum = 0;
                        }
                        counterForThree = 0;
                    }
                    wholeSet[j, h, i] = randSONum;
                    counterForThree++;
                }
            }
        }
        wholeSet = Shuffle(wholeSet);
        for (int i = 0; i < depth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                for (int h = 0; h < height; h++)
                {
                    Vector3 spawnPosition = new Vector3(j * 3.0f + spawnOffset.x, h * 3.2f + spawnOffset.y, i * 0.01f);
                    Card cardToSpawn = Instantiate(cardPrefab, spawnPosition, Quaternion.identity, transform).GetComponent<Card>();
                    cardToSpawn.SetUP(gameManager.GetCardsData()[wholeSet[j, h, i]]);
                }
            }
        }
    }
    /// <summary>
    /// shuffles 3D array
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public int[,,] Shuffle(int[,,] list)
    {
        int x = list.GetLength(0);
        int y = list.GetLength(1);
        int z = list.GetLength(2);
        // Flatten to 1D array
        int[] flattenedArray = FlattenArray(list);
        for (int i = flattenedArray.Length - 1; i > 1; i--)
        {
            int j = Random.Range(0, i + 1);
            int value = flattenedArray[j];
            flattenedArray[j] = flattenedArray[i];
            flattenedArray[i] = value;
        }
        return ReconstructArray(flattenedArray,x,y,z);
    }
    /// <summary>
    /// shuffles 2D array
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public int[,] Shuffle(int[,] list)
    {
        int x = list.GetLength(0);
        int y = list.GetLength(1);
        // Flatten to 1D array
        int[] flattenedArray = FlattenArray(list);
        for (int i = flattenedArray.Length-1; i>1;i--)
        {
            int j = Random.Range(0, i + 1);
            int value = flattenedArray[j];
            flattenedArray[j] = flattenedArray[i];
            flattenedArray[i] = value;
        }
        // Convert 1D array back to 2D array
        return ReconstructArray(flattenedArray, x, y);
    }
    /// <summary>
    /// Got it from ChatGPT, flatten 2D array
    /// </summary>
    /// <param name="originalArray"></param>
    /// <returns></returns>
    int[] FlattenArray(int[,] originalArray)
    {
        int rows = originalArray.GetLength(0);
        int cols = originalArray.GetLength(1);

        int[] flattenedArray = new int[rows * cols];
        int index = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                flattenedArray[index++] = originalArray[i, j];
            }
        }

        return flattenedArray;
    }
    /// <summary>
    /// generated by ChatGPT, flatten 3D array
    /// </summary>
    /// <param name="originalArray"></param>
    /// <returns></returns>
    int[] FlattenArray(int[,,] originalArray)
    {
        int dim1 = originalArray.GetLength(0);
        int dim2 = originalArray.GetLength(1);
        int dim3 = originalArray.GetLength(2);

        int[] flattenedArray = new int[dim1 * dim2 * dim3];
        int index = 0;

        for (int i = 0; i < dim1; i++)
        {
            for (int j = 0; j < dim2; j++)
            {
                for (int k = 0; k < dim3; k++)
                {
                    flattenedArray[index++] = originalArray[i, j, k];
                }
            }
        }

        return flattenedArray;
    }
    /// <summary>
    /// same as above, from ChatGPT, reconstruct 1D array to 2D
    /// </summary>
    /// <param name="flattenedArray"></param>
    /// <param name="rows"></param>
    /// <param name="cols"></param>
    /// <returns></returns>
    int[,] ReconstructArray(int[] flattenedArray, int rows, int cols)
    {
        int[,] reconstructedArray = new int[rows, cols];
        int index = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                reconstructedArray[i, j] = flattenedArray[index++];
            }
        }

        return reconstructedArray;
    }
    /// <summary>
    /// still generated by ChatGPT, reconstruct 1D array to 3D
    /// </summary>
    /// <param name="flattenedArray"></param>
    /// <param name="dim1"></param>
    /// <param name="dim2"></param>
    /// <param name="dim3"></param>
    /// <returns></returns>
    int[,,] ReconstructArray(int[] flattenedArray, int dim1, int dim2, int dim3)
    {
        int[,,] reconstructedArray = new int[dim1, dim2, dim3];
        int index = 0;

        for (int i = 0; i < dim1; i++)
        {
            for (int j = 0; j < dim2; j++)
            {
                for (int k = 0; k < dim3; k++)
                {
                    reconstructedArray[i, j, k] = flattenedArray[index++];
                }
            }
        }

        return reconstructedArray;
    }
}
