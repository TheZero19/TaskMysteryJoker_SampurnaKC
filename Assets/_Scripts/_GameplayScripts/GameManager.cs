using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        List<int> itemIDList = new List<int>() { 1, 2, 3, 4, 5 };
        for (int i = 0; i < 100; i++)
        {
            var responseListOfLists = Server.RespondWin(itemIDList);
            foreach (var list in responseListOfLists)
            {
                Debug.Log($"{i}th win response: {list[0]}, {list[1]}, {list[2]}");
            }
        }
        
        for (int i = 0; i < 100; i++)
        {
            var response = Server.RespondLoss(itemIDList);
            foreach (var list in response)
            {
                Debug.Log($"{i}th loss response: {list[0]}, {list[1]}, {list[2]}");
            }
        }
    }
}
