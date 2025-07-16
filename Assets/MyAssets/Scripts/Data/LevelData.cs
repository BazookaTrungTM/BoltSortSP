using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public List<ScrewData> screwList = new List<ScrewData>();
    public List<int> targetColorList = new List<int>();
    public int goalNum;
    public List<RopeData> ropeMap = new List<RopeData>();
    public List<GlassData> glassMap = new List<GlassData>();
}
