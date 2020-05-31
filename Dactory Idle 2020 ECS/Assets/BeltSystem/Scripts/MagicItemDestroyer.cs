﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicItemDestroyer : BeltObject
{
    public void DestroyItemsOnSlots () {
        for (int x = 1; x < 3; x++) {
            for (int y = 1; y < 3; y++) {
                BeltMaster.s.DestroyItemAtSlot(myBeltItemSlots[x, y]);
            }
        }
    }
}
