using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using GogoGaga.OptimizedRopesAndCables;


#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ToolEditLevel : MonoBehaviour
{
    public List<int> colorList;
    public EditScrew editScrew;
    public GameObject glassPrefab;
    public Transform screwParent;
    public Transform glassParent;
    public List<string> ropeMap;
    public List<string> glassMap;
    public int goalNum;
    [HideInInspector] public int level;

}

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(ToolEditLevel))]
public class ToolEditor : Editor
{
    private ToolEditLevel instance;
    private bool show;
    private string ropeData;
    private string glassData;
    private Dictionary<ColorType, int> levelColorMap = new Dictionary<ColorType, int>();
    private Dictionary<NutType, int> nutTypeMap = new Dictionary<NutType, int>();
  //  private List<Rope> ropeInLevel = new List<Rope>();  

    private const string path = "Assets/MyAssets/Resources/";
    private Dictionary<int, List<Vector3>> screwPosDic1 = new Dictionary<int, List<Vector3>>()
    {
        {2, new List<Vector3>() { new Vector3(-2.5f, 0, 0), new Vector3(2.5f, 0, 0) } },
        {3, new List<Vector3>() { new Vector3(-4, 0, 0), new Vector3(0, 0, 0), new Vector3(4, 0, 0) } },
        {4, new List<Vector3>() { new Vector3(-3, 0, 3), new Vector3(3, 0, 3), new Vector3(-3, 0, -3), new Vector3(3, 0, -3) } },
        {5, new List<Vector3>() { new Vector3(-3, 0, 3), new Vector3(3, 0, 3), new Vector3(0, 0, 0), new Vector3(-3, 0, -3), new Vector3(3, 0, -3) } },
        {6, new List<Vector3>() { new Vector3(-4, 0, 3), new Vector3(0, 0, 3), new Vector3(4, 0, 3), new Vector3(-4, 0, -4), new Vector3(0, 0, -4), new Vector3(4, 0, -4) } },
        {7, new List<Vector3>() { new Vector3(-2.5f, 0, 5), new Vector3(2.5f, 0, 5), new Vector3(-5, 0, 1), new Vector3(0, 0, 1), new Vector3(5, 0, 1), new Vector3(-2.5f, 0, -3), new Vector3(2.5f, 0, -3) } },
        {8, new List<Vector3>() { new Vector3(-4.5f, 0, 4), new Vector3(4.5f, 0, 4), new Vector3(-2, 0, 2), new Vector3(2, 0, 2), new Vector3(-2, 0, -4), new Vector3(2, 0, -4), new Vector3(-4.5f, 0, -6), new Vector3(4.5f, 0, -6) } },
        {9, new List<Vector3>() { new Vector3(-4, 0, 5), new Vector3(0, 0, 5), new Vector3(4, 0, 5), new Vector3(-4, 0, 0), new Vector3(0, 0, 0), new Vector3(4, 0, 0), new Vector3(-4, 0, -5), new Vector3(0, 0, -5), new Vector3(4, 0, -5) } },
        {10, new List<Vector3>() { new Vector3(-4, 0, 5), new Vector3(4f, 0, 5), new Vector3(-2, 0, 2), new Vector3(2, 0, 2), new Vector3(-5, 0, -0.5f), new Vector3(5, 0, -0.5f), new Vector3(-2, 0, -3), new Vector3(2, 0, -3), new Vector3(-4f, 0, -6), new Vector3(4f, 0, -6) } },
        {11, new List<Vector3>() { new Vector3(-5, 0, 6), new Vector3(5, 0, 6), new Vector3(-2.5f, 0, 3), new Vector3(2.5f, 0, 3), new Vector3(-5, 0, 0), new Vector3(0, 0, 0), new Vector3(5, 0, 0), new Vector3(-2.5f, 0, -3), new Vector3(2.5f, 0, -3f), new Vector3(-5, 0, -6f), new Vector3(5, 0, -6f) } },
        {12, new List<Vector3>() { new Vector3(-4.5f, 0, 5), new Vector3(-1.5f, 0, 5), new Vector3(1.5f, 0, 5), new Vector3(4.5f, 0, 5), new Vector3(-4.5f, 0, 0), new Vector3(-1.5f, 0, 0), new Vector3(1.5f, 0, 0), new Vector3(4.5f, 0, 0), new Vector3(-4.5f, 0, -5), new Vector3(-1.5f, 0, -5), new Vector3(1.5f, 0, -5), new Vector3(4.5f, 0, -5) } },
        {13, new List<Vector3>() { new Vector3(-4f, 0, 6), new Vector3(0f, 0, 6), new Vector3(4f, 0, 6), new Vector3(-2f, 0, 3), new Vector3(2f, 0, 3), new Vector3(-4, 0, 0), new Vector3(0, 0, 0), new Vector3(4f, 0, 0), new Vector3(-2f, 0, -3), new Vector3(2f, 0, -3), new Vector3(-4f, 0, -6), new Vector3(0f, 0, -6), new Vector3(4f, 0, -6) } }

    };


    private void OnEnable()
    {
        instance = target as ToolEditLevel;
        if (instance == null)
            return;
        //FindLevel();
    }

    private void FindLevel()
    {
        var info = new DirectoryInfo(path);
        var files = info.GetFiles("*.txt");
        instance.level = files.Length + 1;
    }
    
    private void SortScrewPositions()
    {
        if (instance.screwParent.childCount <= 1) return;
        for(int i = 0; i < instance.screwParent.childCount; i++)
        {
            instance.screwParent.GetChild(i).localPosition = screwPosDic1[instance.screwParent.childCount][i];
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear All", GUILayout.Height(50)))
        {
            Clear();
        }
        if (GUILayout.Button("Create Screw", GUILayout.Height(50)))
        {
            var screw = Instantiate(instance.editScrew, instance.screwParent);
            screw.name = "Screw";
            screw.SetToDefault();
            screw.gameObject.SetActive(true);
            screw.SetIndex(instance.screwParent.childCount - 1);
            SortScrewPositions();
        }
        if (GUILayout.Button("New Holder Color List", GUILayout.Height(50)))
        {
            CreateNewHolderColorList();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Level", EditorStyles.boldLabel);
        instance.level = EditorGUILayout.IntField(instance.level, GUILayout.Height(20));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("GoalNum", EditorStyles.boldLabel);
        instance.goalNum = EditorGUILayout.IntField(instance.goalNum, GUILayout.Height(20));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Rope", EditorStyles.boldLabel);
        ropeData = EditorGUILayout.TextField(ropeData, GUILayout.Height(20), GUILayout.Width(200));
        if (GUILayout.Button("Update Ropes", GUILayout.Height(20)))
        {
            DecodeData(ropeData);
            InitAllRopes();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Glass", EditorStyles.boldLabel);
        glassData = EditorGUILayout.TextField(glassData, GUILayout.Height(20), GUILayout.Width(200));
        if (GUILayout.Button("Update Glasses", GUILayout.Height(20)))
        {
            DecodeData(glassData, false);
            InitAllGlasses(); 
        }
        EditorGUILayout.EndHorizontal();


        show = EditorGUILayout.Toggle("Show Reference", show);
        if (show)
            DrawDefaultInspector();
        EditorGUILayout.Space(20);
        if (GUILayout.Button("Open Level", GUILayout.Height(50)))
        {
            Clear();
            var info = new DirectoryInfo(path);
            var files = info.GetFiles("*.txt");
            if (instance.level <= 0 || instance.level > files.Length)
                return;
            OpenLevel(instance.level);
        }
        if (GUILayout.Button("Save As Replace Old Level", GUILayout.Height(50)))
        {
            if (instance.screwParent.childCount == 0) return;

            //if (!IsLevelLegit())
            //{
            //    ColorType type = levelColorMap.Keys.ToList().Find(key => levelColorMap[key] % 5 != 0);
            //    // Debug.LogError("Level Design Is Not Correct At Color: <" + type + ">");
            //    return;
            //}

            var info = new DirectoryInfo(path);
            var files = info.GetFiles("*.txt");
            if (instance.level > files.Length)
                return;
            var levelData = new LevelData
            {
                screwList = new List<ScrewData>(),
                targetColorList = new List<int>(),
                goalNum = 0,
                ropeMap = new List<RopeData>(),
                glassMap = new List<GlassData>()
            };

            levelData.goalNum = instance.goalNum;
            foreach (Transform screw in instance.screwParent)
            {
                if (screw.GetComponent<EditScrew>().itemHolder.childCount == 0) continue;
                ScrewData newData = new ScrewData()
                {
                    nutDatas = new List<string>(),
                    size = screw.GetComponent<EditScrew>().size,
                    towelColor = screw.GetComponent<EditScrew>().towelColor
                };
                foreach (Transform nut in screw.GetComponent<EditScrew>().itemHolder)
                {
                    EditNut editNut = nut.GetComponent<EditNut>();
                    string nutData = editNut.Type.GetHashCode().ToString() + "/";
                    int colorIndex = editNut.Color.GetHashCode() + (editNut.isHidden ? 12 : 0);
                    nutData += colorIndex.ToString() + "/" + editNut.bombCount.ToString() + "/";
                    nutData += editNut.isIce ? 1 : 0;

                    newData.nutDatas.Add(nutData);
                }
                levelData.screwList.Add(newData);
            }

            foreach (var item in instance.colorList)
            {
                levelData.targetColorList.Add((int) item);
            }
            foreach (var item in instance.ropeMap)
            {
                string[] datas = item.Split("/");
                RopeData ropeData = new RopeData()
                {
                    datas = new List<int>()
                };
                foreach (var data in datas)
                {
                    ropeData.datas.Add(int.Parse(data));
                }
                levelData.ropeMap.Add(ropeData);
            }
            foreach (var item in instance.glassMap)
            {
                string[] datas = item.Split("/");
                GlassData glassData = new GlassData()
                {
                    datas = new List<int>()
                };
                foreach (var data in datas)
                {
                    glassData.datas.Add(int.Parse(data)); 
                }
                levelData.glassMap.Add(glassData);
            }

            var json = JsonUtility.ToJson(levelData);
            Debug.Log("Edit Level " + instance.level + " Complete :   " + json);
            File.WriteAllText(path + instance.level + ".txt", json);
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
            FindLevel();

        }
        if (GUILayout.Button("Save As New Level", GUILayout.Height(50)))
        {
            if (instance.screwParent.childCount == 0) return;

            if (!IsLevelLegit())
            {
                ColorType type = levelColorMap.Keys.ToList().Find(key => levelColorMap[key] % 5 != 0);
                // Debug.LogError("Level Design Is Not Correct At Color: <" + type + ">");
                return;
            }

            var levelData = new LevelData
            {
                    screwList = new List<ScrewData>(),
                    targetColorList = new List<int>(),
                    goalNum = 0,
                    ropeMap = new List<RopeData>(),
                    glassMap = new List<GlassData>()
            };

            levelData.goalNum = instance.goalNum;
            foreach (Transform screw in instance.screwParent)
            {
                if (screw.GetComponent<EditScrew>().itemHolder.childCount == 0) continue;
                ScrewData newData = new ScrewData()
                {
                    nutDatas = new List<string>(),
                    size = screw.GetComponent<EditScrew>().size,
                    towelColor = screw.GetComponent<EditScrew>().towelColor
                };
                foreach (Transform nut in screw.GetComponent<EditScrew>().itemHolder)
                {
                    EditNut editNut = nut.GetComponent<EditNut>();
                    string nutData = editNut.Type.GetHashCode().ToString() + "/";
                    int colorIndex = editNut.Color.GetHashCode() + editNut.Type.GetHashCode() * 12;
                    nutData += colorIndex.ToString() + "/" + editNut.bombCount.ToString() + "/";
                    nutData += editNut.isIce ? 1 : 0;

                    newData.nutDatas.Add(nutData);
                }
                levelData.screwList.Add(newData);
            }
            foreach (var item in instance.colorList)
            {
                levelData.targetColorList.Add((int)item);
            }

            foreach (var item in instance.ropeMap)
            {
                Debug.Log(item);
                string[] datas = item.Split("/");
                RopeData ropeData = new RopeData()
                {
                    datas = new List<int>()
                };
                foreach (var data in datas)
                {
                    ropeData.datas.Add(int.Parse(data));
                }
                levelData.ropeMap.Add(ropeData);
            }
            foreach (var item in instance.glassMap)
            {
                Debug.Log(item);
                string[] datas = item.Split("/");
                GlassData glassData = new GlassData()
                {
                    datas = new List<int>()
                };
                foreach (var data in datas)
                {
                    Debug.Log(int.Parse(data));
                    glassData.datas.Add(int.Parse(data));
                }
                levelData.glassMap.Add(glassData);
            }

            var json = JsonUtility.ToJson(levelData);
            FindLevel();
            Debug.Log("New Level " + instance.level + " Complete :   " + json);
            File.WriteAllText(path + instance.level + ".txt", json);
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
            FindLevel();
        }
        //
        EditorGUILayout.Space(20);
        GUILayout.Label("LIST LEVEL", EditorStyles.boldLabel);
        var max = new DirectoryInfo(path).GetFiles("*.txt").Length;
        if (max < 1)
            return;
        _scroll = EditorGUILayout.BeginScrollView(_scroll, GUILayout.Height(150));
        for (var i = 1; i < max + 1; i++)
        {
            if (GUILayout.Button("Level " + i))
            {
                Clear();
                OpenLevel(i);
            }
        }
        EditorGUILayout.EndScrollView();
        serializedObject.ApplyModifiedProperties();
        // DrawDefaultInspector();
    }

    private Vector2 _scroll;

    private void Clear()
    {
        if (instance.screwParent.childCount > 0)
        {
            for (int i = instance.screwParent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(instance.screwParent.GetChild(i).gameObject);
            }
        }
        if (instance.glassParent.childCount > 0)
        {
            for (int i = instance.glassParent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(instance.glassParent.GetChild(i).gameObject);
            }
        }
      //  ropeInLevel.Clear();
        instance.ropeMap.Clear();
        instance.glassMap.Clear();
    }

    private void OpenLevel(int level)
    {
        instance.level = level;
        var text = AssetDatabase.LoadAssetAtPath<TextAsset>(path + level + ".txt");
        if (text == null)
            return;
        //Debug.Log(text.text);
        var data = JsonUtility.FromJson<LevelData>(text.text);
        instance.goalNum = data.goalNum;
        int index = 0;
        foreach (var screw in data.screwList)
        {
            var newScrew = Instantiate(instance.editScrew, instance.screwParent);
            newScrew.name = "Screw";
            newScrew.SetData(screw);
            newScrew.gameObject.SetActive(true);
            newScrew.SetIndex(index++);
            SortScrewPositions();
        }
        instance.colorList.Clear();
        foreach (var color in data.targetColorList)
        {
            instance.colorList.Add(color);
        }
        foreach (var ropeData in data.ropeMap)
        {
            string newData = "";
            foreach (var data1 in ropeData.datas)
            {
                newData += data1.ToString() + "/";
            }
            newData = newData.TrimEnd('/');
            instance.ropeMap.Add(newData);
        }
        InitAllRopes();
        foreach (var glassData in data.glassMap)
        {
            string newData = "";
            foreach (var data2 in glassData.datas)
            {
                newData += data2.ToString() + "/";
            }
            newData = newData.TrimEnd('/');
            instance.glassMap.Add(newData);
        }
        InitAllGlasses();
    }
    private void CreateNewHolderColorList()
    {
        UpdateDataColorMap();
        instance.colorList.Clear();
        int count = 0;
        foreach (var item in levelColorMap)
        {
            for (int i = 0; i < item.Value / 5; i++)
            {
                if (instance.colorList.Count < count + 1)
                    instance.colorList.Add(0);
                else
                    instance.colorList[count] = 0;
                count++;
            }
        }
    }
    private void UpdateHolderColorList()
    {
        UpdateDataColorMap();
        instance.colorList.Clear();
        int count = 0;
        foreach (var item in levelColorMap)
        {
            for (int i = 0; i < item.Value / 5; i++)
            {
                if (instance.colorList.Count < count + 1)
                    instance.colorList.Add(0);
                count++;
            }
        }
    }
    private bool IsLevelLegit()
    {
        UpdateHolderColorList();
        foreach (var value in levelColorMap.Values)
        {
            if(value % 5 != 0)
                return false;
        }

        return true;
    }
    private void UpdateDataColorMap()
    {
        levelColorMap.Clear();
        foreach (Transform screw in instance.screwParent)
        {
            foreach (Transform nut in screw.GetComponent<EditScrew>().itemHolder)
            {
                ColorType type = nut.GetComponent<EditNut>().Color;
                if (levelColorMap.ContainsKey(type))
                {
                    levelColorMap[type]++;
                }
                else
                {
                    levelColorMap.Add(type, 1);
                }
            }
        }
    }

    private void DecodeData(string data,  bool isRope = true)
    {
        if (data == null) return;
        string[] datas = data.Split("/");
        if (datas.Length != 4)
        {
            // Debug.LogError("Wrong Data");
        }    
        else
        {
            List<int> parseList = new List<int>();
            foreach (string d in datas)
            {
                if (!int.TryParse(d, out int index))
                {
                    // Debug.LogError("Wrong Data");
                    break;
                }
                else
                {
                    parseList.Add(index);
                }
            }
            if (parseList.Count == 4)
            {
                if (isRope && !instance.ropeMap.Contains(data))
                    instance.ropeMap.Add(data);
                else if (!isRope && !instance.glassMap.Contains(data))
                    instance.glassMap.Add(data);
            }
        }
    }

    private void InitAllRopes()
    {
        //foreach (var item in ropeInLevel)
        //{
        //    if (item.gameObject.activeInHierarchy)
        //        item.gameObject.SetActive(false);
        //}
        //ropeInLevel.Clear();
        //foreach (var ropedata in instance.ropeMap)
        //{
        //    string[] datas = ropedata.Split("/");
        //    EditNut nut1 = instance.screwParent.GetChild(int.Parse(datas[0])).GetComponent<EditScrew>().itemHolder.GetChild(int.Parse(datas[1])).GetComponent<EditNut>();
        //    EditNut nut2 = instance.screwParent.GetChild(int.Parse(datas[2])).GetComponent<EditScrew>().itemHolder.GetChild(int.Parse(datas[3])).GetComponent<EditNut>();
        //    nut1.SetRope(nut2);
        //    ropeInLevel.Add(nut1.rope);           
        //    ropeInLevel.Add(nut2.rope);           
        //}
    }
    private void InitAllGlasses()
    {
        if (instance.glassParent.childCount > 0)
        {
            for (int i = instance.glassParent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(instance.glassParent.GetChild(i).gameObject);
            }
        }

        foreach (var glassData  in instance.glassMap)
        {
            string[] datas = glassData.Split("/");
            var glass = Instantiate(instance.glassPrefab, instance.glassParent);
            glass.name = "Glass";
            glass.gameObject.SetActive(true);
            int screwNum = int.Parse(datas[2]) < 100 ? 3 : 2;
            Transform screw1 = instance.screwParent.GetChild(int.Parse(datas[0]));
            Transform screw2 = instance.screwParent.GetChild(int.Parse(datas[1]));

            float tanAngle = Mathf.Abs(screw1.localPosition.z - screw2.localPosition.z) / Mathf.Abs(screw1.localPosition.x -  screw2.localPosition.x);
            float angle = Mathf.Atan(tanAngle) * Mathf.Rad2Deg;
            if (screw1.transform.position.x > screw2.transform.position.x)
                angle *= -1;

            if (screwNum == 2)
            {
                float posX = (screw1.localPosition.x + screw2.localPosition.x) / 2f;
                float posZ = (screw1.localPosition.z + screw2.localPosition.z) / 2f;
                glass.GetComponent<Glass>().InitGlass(posX, posZ, int.Parse(datas[3]), screwNum, angle);
            }
            else if (screwNum == 3)
            {
                glass.GetComponent<Glass>().InitGlass(screw2.localPosition.x, screw2.localPosition.z, int.Parse(datas[3]), screwNum, angle);
            }
        }
    }
}

#endif
