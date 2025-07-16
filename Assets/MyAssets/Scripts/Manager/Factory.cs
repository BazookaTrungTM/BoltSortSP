using Do;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Singleton<Factory>
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    #region Ultilities
    public Material GetNutMaterial(ColorType color)
    {
        return nutMats[(int)color];
    }
    public Color GetColorNut(ColorType color)
    {
        return matColors[(int) color];
    }
    public float GetSmoothnessNut(ColorType color)
    {
        return matSmoothness[(int) color];
    }
    public float GetMetalicNut(ColorType color)
    {
        return matMetalic[(int)color];
    }
    public Color GetGradientColorByType(ColorType type)
    {
        Color color = Color.white;
        switch (type)
        {
            case ColorType.None:
                color = Color.white;
                color.a = 120 / 255f;
                break;
            case ColorType.Orange:
                color = new Color(255 / 255f, 187 / 255f, 52 / 255f, 120/ 255f);
                break;
            case ColorType.Green:
                color = Color.green;
                color.a = 120 / 255f;
                break;
            case ColorType.Blue:
                color = new Color(47 / 255f, 159 / 255f, 243 / 255f, 120 / 255f);
                break;
            case ColorType.Violet:
                color = Color.magenta;
                color.a = 120 / 255f;
                break;
            case ColorType.Red:
                color = Color.red;
                color.a = 120 / 255f;
                break;
            case ColorType.Grey:
                color = Color.grey;
                color.a = 120 / 255f;
                break;
            case ColorType.Pink:
                color = new Color(253 / 255f, 125 / 255f, 235 / 255f, 120/ 255f);
                break;
            case ColorType.Cyan:
                color = Color.cyan;
                color.a = 120 / 255f;
                break;
            case ColorType.Yellow:
                color = Color.yellow;
                color.a = 120/ 255f;
                break;
            case ColorType.Pink_2:
                color = new Color(248 / 255f, 195 / 255f, 234 / 255f, 120 / 255f);
                break;
            case ColorType.Blue_2:
                color = new Color(34 / 255f, 97 / 255f, 195 / 255f, 255 / 255f);
                break;
        }
        return color;
    }


    #endregion

    #region Nut
    [Space(10)]
    [Header("Nut")]
    [SerializeField] List<GameObject> nutPrefabs;
    [SerializeField] List<Material> nutMats;
    [SerializeField] List<Color> matColors;
    [SerializeField] List<float> matSmoothness;
    [SerializeField] List<float> matMetalic;
    private Dictionary<NutType, Queue<GameObject>> nutPoolList = new Dictionary<NutType, Queue<GameObject>>();
    public Transform nutPoolParent;
    public Nut GetNutByType(NutData data, Transform parent = null)
    {

        if (!nutPoolList.ContainsKey(data.nutType))
        {
            nutPoolList.Add(data.nutType, new Queue<GameObject>());
        }
        if (nutPoolList[data.nutType].Count == 0)    
        {
            var newObject = Instantiate(nutPrefabs[(int) data.nutType], parent);
            newObject.transform.localScale = Vector3.one;
            newObject.SetActive(true);
            newObject.GetComponent<Nut>().Init(data);
            return newObject.GetComponent<Nut>();
        }

        var deQueuedPoolObject = nutPoolList[data.nutType].Dequeue();
        if (deQueuedPoolObject.activeSelf) return GetNutByType(data, parent);
        deQueuedPoolObject.transform.parent = parent;
        deQueuedPoolObject.transform.localScale = Vector3.one;
        deQueuedPoolObject.SetActive(true);
        deQueuedPoolObject.GetComponent<Nut>().Init(data);

        return deQueuedPoolObject.GetComponent<Nut>();    
    }
    public void ReturnNutToPool(GameObject poolObject, NutType type)
    {
        poolObject.transform.parent = nutPoolParent;
        poolObject.gameObject.SetActive(false);
        nutPoolList[type].Enqueue(poolObject);
    }

    #endregion

    #region Screw
    [Space(10)]
    [Header("Screw")]
    [SerializeField] GameObject screw3xPrefab;
    [SerializeField] GameObject screw5xPrefab;
    [SerializeField] GameObject screw10xPrefab;
    private Dictionary<int, Queue<GameObject>> _poolScrewList = new Dictionary<int, Queue<GameObject>>();
    public Transform _screwPoolParent;
    public Screw GetScrewBySize(int size, Transform parent = null)
    {
        if (!_poolScrewList.ContainsKey(size))
        {
            _poolScrewList.Add(size, new Queue<GameObject>());
        }
        if (_poolScrewList[size].Count == 0) // Expand pool if necessary
        {
            GameObject newObject;
            if (size == 3)
                newObject = Instantiate(screw3xPrefab, parent);
            else if (size == 5)
                newObject = Instantiate(screw5xPrefab, parent);
            else
                newObject = Instantiate(screw10xPrefab, parent);
            newObject.transform.localScale = Vector3.one;
            newObject.SetActive(true);
            return newObject.GetComponent<Screw>();
        }

        var deQueuedPoolObject = _poolScrewList[size].Dequeue();
        if (deQueuedPoolObject.activeSelf) return GetScrewBySize(size, parent);
        deQueuedPoolObject.transform.parent = parent;
        deQueuedPoolObject.transform.localScale = Vector3.one;
        deQueuedPoolObject.SetActive(true);
        return deQueuedPoolObject.GetComponent<Screw>();
    }
    public void ReturnScrewToPool(int size, GameObject poolObject)
    {
        poolObject.transform.parent = _screwPoolParent;
        poolObject.gameObject.SetActive(false);
        _poolScrewList[size].Enqueue(poolObject);
    }

    #endregion

    #region Goal Screw
    [Space(10)]
    [Header("Goal Screw")]
    [SerializeField] List<GameObject> goalScrewPrefab = new List<GameObject>();
    private Dictionary<NutType, Queue<GameObject>> _poolGoalScrewList = new Dictionary<NutType, Queue<GameObject>>();
    public Transform _goalScrewPoolParent;
    public GoalScrew GetGoalScrewBySize(NutType type, Transform parent = null)
    {
        if (!_poolGoalScrewList.ContainsKey(type))
        {
            _poolGoalScrewList.Add(type, new Queue<GameObject>());
        }
        if (_poolGoalScrewList[type].Count == 0) // Expand pool if necessary
        {
            GameObject newObject = Instantiate(goalScrewPrefab[(int) type], parent);

            newObject.transform.localScale = Vector3.one;
            newObject.SetActive(true);
            return newObject.GetComponent<GoalScrew>();
        }

        var deQueuedPoolObject = _poolGoalScrewList[type].Dequeue();
        if (deQueuedPoolObject.activeSelf) return GetGoalScrewBySize(type, parent);
        deQueuedPoolObject.transform.parent = parent;
        deQueuedPoolObject.transform.localScale = Vector3.one;
        deQueuedPoolObject.SetActive(true);
        return deQueuedPoolObject.GetComponent<GoalScrew>();
    }
    public void ReturnGoalScrewToPool(NutType type, GameObject poolObject)
    {
        poolObject.transform.parent = _goalScrewPoolParent;
        poolObject.gameObject.SetActive(false);
        _poolGoalScrewList[type].Enqueue(poolObject);
    }

    #endregion

    #region Glass
    [Space(10)]
    [Header("Glass")]

    [SerializeField] GameObject glass2xPrefab;
    [SerializeField] GameObject glass3xPrefab;
    private Dictionary<int, Queue<GameObject>> _poolGlassList = new Dictionary<int, Queue<GameObject>>();
    public Transform _glassPoolParent;
    public Glass GetGlassBySize(int size, Transform parent = null)
    {
        if (!_poolGlassList.ContainsKey(size))
        {
            _poolGlassList.Add(size, new Queue<GameObject>());
        }
        if (_poolGlassList[size].Count == 0) // Expand pool if necessary
        {
            GameObject newObject;
            if (size == 2)
                newObject = Instantiate(glass2xPrefab, parent);
            else
                newObject = Instantiate(glass3xPrefab, parent);
            newObject.SetActive(true);
            return newObject.GetComponent<Glass>();
        }

        var deQueuedPoolObject = _poolGlassList[size].Dequeue();
        if (deQueuedPoolObject.activeSelf) return GetGlassBySize(size, parent);
        deQueuedPoolObject.transform.parent = parent;
        deQueuedPoolObject.SetActive(true);
        return deQueuedPoolObject.GetComponent<Glass>();
    }
    public void ReturnGlassToPool(int size, GameObject poolObject)
    {
        poolObject.transform.parent = _glassPoolParent;
        poolObject.gameObject.SetActive(false);
        _poolGlassList[size].Enqueue(poolObject);
    }

    #endregion
}
