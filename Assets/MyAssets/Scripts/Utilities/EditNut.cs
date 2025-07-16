using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Drawing;
//using GogoGaga.OptimizedRopesAndCables;




#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

[ExecuteInEditMode]
public class EditNut : MonoBehaviour
{
    public List<Material> nutMaterials = new List<Material>();
    public Material hiddenMaterial;
    public List<MeshRenderer> renderers = new List<MeshRenderer>();
    public List<GameObject> roots = new List<GameObject>();
    public MeshRenderer curRenderer;
    public ColorType Color;
    public NutType Type;
    public bool isHidden;
    public int bombCount;
    public bool isIce;
  //  public Rope rope;

    public void SetToDefault()
    {
        Color = ColorType.Orange;
        Type = NutType.Normal;
        bombCount = 0;
        isIce = false;
        curRenderer = renderers[0];
        foreach (var item in roots)
            item.SetActive(false);

        roots[0].SetActive(true);
        curRenderer.material = nutMaterials[1];
        bombCount = 0;
        isIce = false;
        SetBomb();
        SetIce();
   //     rope.gameObject.SetActive(false);
    }
    public void SetData(string data)
    {
        string[] datas = data.Split('/');
        SetType((NutType) int.Parse(datas[0]));
        int colorIndex = int.Parse(datas[1]);
        isHidden = colorIndex > 11;
        SetColor(isHidden ? (ColorType) (colorIndex - 12) : (ColorType) colorIndex);
        bombCount = int.Parse(datas[2]);
        SetBomb();
        isIce = int.Parse(datas[3]) > 0;
        SetIce();
        SetHidden();
    }
    public void SetColor(ColorType color)
    {
        if (color == ColorType.None) return;
        Color = color;
        curRenderer.material = nutMaterials[(color.GetHashCode())];
    }
    public void SetType(NutType type)
    {
        foreach (var item in roots)
            item.SetActive(false);

        Type = type;
        roots[type.GetHashCode()].SetActive(true);
        curRenderer = renderers[type.GetHashCode()];
    }
    public void SetIce()
    {
        if (isIce)
        {
            roots[Type.GetHashCode()].transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            roots[Type.GetHashCode()].transform.GetChild(2).gameObject.SetActive(false);
        }
    }
    public void SetBomb()
    {
        if (bombCount <= 0)
        {
            roots[Type.GetHashCode()].transform.GetChild(3).gameObject.SetActive(false);
        }
        else
        {
            roots[Type.GetHashCode()].transform.GetChild(3).gameObject.SetActive(true);
            roots[Type.GetHashCode()].transform.GetChild(3).GetChild(0).GetComponent<TextMeshPro>().text = bombCount.ToString();
        }
    }
    public void SetHidden()
    {
        if (isHidden)
        {
            curRenderer.material = hiddenMaterial;
        }
        else
        {
            curRenderer.material = nutMaterials[(Color.GetHashCode())];
        }
    }
    public void SetRope(EditNut nut)
    {
        //rope.gameObject.SetActive(true);
        //rope.SetStartPoint(rope.transform);
        //nut.rope.gameObject.SetActive(true);
        //rope.SetEndPoint(nut.rope.transform);
        //rope.ropeLength = Vector3.Distance(rope.transform.position, nut.rope.transform.position) * 1.5f;
    }
}


#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(EditNut))]
public class NutEditor : Editor
{
    private EditNut instance;
    private bool isHidden;
    private bool isIce;
    private int bombCount; 
    private bool show;


    private void OnEnable()
    {
        instance = target as EditNut;
    }
    private readonly string[] _colorTypes =
        {
            ColorType.None.ToString(),
            ColorType.Orange.ToString(),
            ColorType.Green.ToString(),
            ColorType.Blue.ToString(),
            ColorType.Violet.ToString(),
            ColorType.Red.ToString(),
            ColorType.Grey.ToString(),
            ColorType.Pink.ToString(),
            ColorType.Cyan.ToString(),
            ColorType.Yellow.ToString(),
            ColorType.Pink_2.ToString(),
            ColorType.Blue_2.ToString(),
        };
    private readonly string[] _nutTypes =
        {
            NutType.Normal.ToString(),
            NutType.Star.ToString(),
            NutType.Square.ToString(),
        };
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //
        var color = (int)instance.Color;
        var type = (int)instance.Type;
        color = EditorGUILayout.Popup("Color Type", color, _colorTypes);
        if ((int)instance.Color != color)
        {
            instance.SetColor((ColorType) color);
            //instance.nutMeshRenderer.material = instance.nutMaterials[color];
        }
        //instance.Color = (ColorType)color;
        type = EditorGUILayout.Popup("Nut Type", type, _nutTypes);
        if ((int)instance.Type != type)
        {
            instance.SetType((NutType) type);
            instance.SetColor(instance.Color);
        }
        isHidden = EditorGUILayout.Toggle("Is Hidden Nut", isHidden);
        if (instance.isHidden != isHidden)
        {
            instance.isHidden = isHidden;
            instance.SetHidden();
        }
        isIce = EditorGUILayout.Toggle("Is Ice Nut", isIce);
        if (instance.isIce != isIce)
        {
            instance.isIce = isIce;
            instance.SetIce();
        }

        GUILayout.Label("Bomb Count", EditorStyles.boldLabel);
        bombCount = EditorGUILayout.IntField(instance.bombCount, GUILayout.Height(20));
        if (instance.bombCount != bombCount)
        {
            instance.bombCount = bombCount;
            instance.SetBomb();
        }

        serializedObject.ApplyModifiedProperties();
        //
        show = EditorGUILayout.Toggle("Show Reference", show);
        if (show)
            DrawDefaultInspector();
    }
}

#endif
