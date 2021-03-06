﻿using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct ItemMovement : IComponentData {
    // the target we want to move towards (offset included)
    public float3 targetWithOffset;
}

[Serializable]
public struct ItemID : IComponentData {
    public ushort myItemId;
}
