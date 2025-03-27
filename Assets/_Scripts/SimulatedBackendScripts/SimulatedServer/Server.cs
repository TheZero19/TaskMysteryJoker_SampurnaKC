using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class Server
{
    private static List<List<int>> _winScenariosMatchingIndices = new List<List<int>>
    {
        new List<int> {0, 0, 0},
        new List<int> {1, 1, 1},
        new List<int> {2, 2, 2},
        new List<int> {0, 1, 2},
        new List<int> {2, 1, 0},
    };
    
    private static List<int> _indices = new List<int>{0, 1, 2};
    
    private static Random rnd = new Random();
    
    public static List<List<int>>RespondWin(List<int> itemIDList)
    {
        var randomItemIDSelectedForWin = itemIDList[rnd.Next(itemIDList.Count)];
        var randomWinScenario = _winScenariosMatchingIndices[rnd.Next(_winScenariosMatchingIndices.Count)];
        Debug.Log($"Selected Win Scenario: {randomWinScenario[0]}, {randomWinScenario[1]}, {randomWinScenario[2]}");
        List<List<int>> reelsWithItemIDs = new List<List<int>>()
        {
            new List<int>{-1, -1, -1},
            new List<int>{-1, -1, -1},
            new List<int>{-1, -1, -1}
        };

        //map the item id to the indices
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i == randomWinScenario[j])
                {
                    (reelsWithItemIDs[j])[i] = randomItemIDSelectedForWin;
                }
                else
                {
                    var randomItemID = itemIDList[rnd.Next(itemIDList.Count)];
                    (reelsWithItemIDs[j])[i] = randomItemID;
                }
            }
        }

        //return the mapping containing the position of the itemIDs .
        return reelsWithItemIDs;
    }

    public static List<List<int>> RespondLoss(List<int> itemIDList)
    {
        List<List<int>> randomScenario; 

        bool isScenarioWinning;
        do
        {
            randomScenario = GetRandomScenario();
            isScenarioWinning = !(IsScenarioLosing(randomScenario));
            
        } while (isScenarioWinning);

        var uniqueItemIDList = GetRandomUniqueItemIDForUniqueIndices(itemIDList, 3);
        if (uniqueItemIDList == null) return null;
        
        //map the random index with random item id
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                randomScenario[j][i] = uniqueItemIDList[randomScenario[j][i]];
            }
        } 

        //return the mapping containing the position of the itemIDs .
        return randomScenario;
    }

    private static List<List<int>> GetRandomScenario()
    {
        List<List<int>> randomScenario = new List<List<int>>()
        {
            new List<int>{-1, -1, -1},
            new List<int>{-1, -1, -1},
            new List<int>{-1, -1, -1}
        };

        rnd.Next(randomScenario.Count);
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                var randomIndex = rnd.Next(randomScenario.Count);
                (randomScenario[j])[i] = randomIndex;
            }
        }

        return randomScenario;
    }

    private static List<int> GetRandomUniqueItemIDForUniqueIndices(List<int> itemIDList, int numberOfUniqueItems)
    {
        //if supplied pool of ids are insufficient, not possible to generate unique items for each index.
        if (itemIDList.Count < numberOfUniqueItems) return null;
        List<int> uniqueItemIDs = new List<int>();
        
        //First will be unique on empty list no matter what
        var randomItemID1 = itemIDList[rnd.Next(itemIDList.Count)];
        uniqueItemIDs.Add(randomItemID1);
        do
        {
            var randomItemID = itemIDList[rnd.Next(itemIDList.Count)];
            bool isGeneratedRandomItemIDUnique = true;
            foreach (var itemID in uniqueItemIDs)
            {
                if (itemID == randomItemID)
                {
                    isGeneratedRandomItemIDUnique = false;
                    break;
                }
            }

            if (isGeneratedRandomItemIDUnique && uniqueItemIDs.Count < numberOfUniqueItems)
            {
                uniqueItemIDs.Add(randomItemID);
            }
        } while (uniqueItemIDs.Count < numberOfUniqueItems);

        return uniqueItemIDs;
    }

    private static bool IsScenarioLosing(List<List<int>> scenario)
    {
        bool isLosing = true;
        List<List<int>> winConditionsIndexPool = new List<List<int>>()
        {
            new List<int> {scenario[0][0], scenario[1][0], scenario[2][0]},
            new List<int> {scenario[0][1], scenario[1][1], scenario[2][1]},
            new List<int> {scenario[0][2], scenario[1][2], scenario[2][2]},
            new List<int> {scenario[0][0], scenario[1][1], scenario[2][2]},
            new List<int> {scenario[0][2], scenario[1][1], scenario[2][0]},
        };

        foreach (var indexList in winConditionsIndexPool)
        {
            foreach (var winScenario in _winScenariosMatchingIndices)
            {
                if (winScenario.SequenceEqual(indexList))
                {
                    isLosing= false;
                    break;
                }
            }
        }

        return isLosing;
    }
}
