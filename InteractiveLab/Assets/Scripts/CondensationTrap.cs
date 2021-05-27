using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CondensationTrap : MonoBehaviour
{
    public string id;
    public int liquidLevel;

    public void ClearLiquidLevel() {
        liquidLevel = 0;
    }
}
