using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReelElementSOAsset", menuName = "ReelElementSO")]
public class ReelElementSO : ScriptableObject
{
    public List<SlotElement> allSlotElements;
}
