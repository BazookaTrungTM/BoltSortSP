using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using TMPro;


#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

[ExecuteInEditMode]
public class EditScrew : MonoBehaviour
{
    public GameObject editNutprefab;
    public TextMeshPro index;
    public Transform itemHolder;
    public Transform towel;
    ScrewData data;
    public float delta = 0.7f;
    public int size;
    public int towelColor;

    public void SetToDefault()
    {
        foreach (Transform nut in itemHolder)
        {
            DestroyImmediate(nut);
        }
        size = 0;
        SetSize(5);
        towelColor = 0;
    }

    public void SetData(ScrewData data)
    {
        this.data = data;
        for (int i = 0; i < data.nutDatas.Count; i++)
        {
            EditNut nut = Instantiate(editNutprefab, itemHolder).GetComponent<EditNut>();
            nut.transform.localPosition = new Vector3(0, i * delta, 0);
            nut.SetData(data.nutDatas[i]);
        }
        size = 0;
        SetSize(data.size);
        SetTowel(data.towelColor);
    }
    public void SetSize(int sizeNum)
    {
        if (size != sizeNum)
        {
            size = sizeNum;
            transform.GetChild(1).gameObject.SetActive(sizeNum == 5);
            transform.GetChild(2).gameObject.SetActive(sizeNum == 10);
        }
    }
    public void SetTowel(int colorIndex)
    {
        if (towelColor != colorIndex)
        {
            towelColor = colorIndex;
            //towel.gameObject.SetActive(towelColor != 0);
        }
    }
    public void SetIndex(int num)
    {
        index.text = num.ToString();
    }
}
#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(EditScrew))]
public class ScrewEditor : Editor
{
    private EditScrew instance;
    private bool show;
    private bool is10X;
    private int towelColor;

    private readonly string[] screwSize =
    {
            5.ToString(),
            10.ToString(),
    };
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
    private void OnEnable()
    {
        instance = target as EditScrew;
        if (instance == null)
            return;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear All", GUILayout.Height(50)))
        {
            if (instance.itemHolder.childCount == 0) return;
            for (int i = instance.itemHolder.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(instance.itemHolder.GetChild(i).gameObject);
            }
        }
        if (GUILayout.Button("Create Nut", GUILayout.Height(50)))
        {
            if (instance.itemHolder.childCount >= instance.size) return;
            var nut = Instantiate(instance.editNutprefab, instance.itemHolder).GetComponent<EditNut>();
            nut.name = "Nut";
            nut.SetToDefault();
            nut.transform.localPosition = new Vector3(0, (instance.itemHolder.childCount - 1) * 0.7f, 0);
            nut.gameObject.SetActive(true);
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Delete Nut", GUILayout.Height(50)))
        {
            if (instance.itemHolder.childCount == 0) return;
            DestroyImmediate(instance.itemHolder.GetChild(instance.itemHolder.childCount - 1).gameObject);
        }

        //
        is10X = EditorGUILayout.Toggle("Is 10X Screw", is10X);
        instance.SetSize(is10X ? 10 : 5);

        var color = instance.towelColor;
        color = EditorGUILayout.Popup("Towel", color, _colorTypes);
        if (instance.towelColor != color)
        {
            instance.SetTowel(color);
        }

        serializedObject.ApplyModifiedProperties();
        show = EditorGUILayout.Toggle("Show Reference", show);
        if (show)
            DrawDefaultInspector();
    }

}

#endif
