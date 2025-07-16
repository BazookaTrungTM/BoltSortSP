using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NutData
{
    public NutType nutType;
    public ColorType color;
    public int bombCount;
    public bool isIce;
    public bool isHidden;
}
public enum NutType
{
    Normal = 0,
    Star = 1,
    Square = 2,
}
