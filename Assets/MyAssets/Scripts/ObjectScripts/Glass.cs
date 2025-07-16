using DG.Tweening;
using Do;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glass : MonoBehaviour
{
    [SerializeField] GameObject firstGlass;
    [SerializeField] GameObject brokenGlass;
    [SerializeField] List<Transform> explorePoses = new List<Transform>();
    [SerializeField] List<MeshRenderer> brokenGlasses = new List<MeshRenderer>();
    [SerializeField] Color color;
    public int size;
    public List<Material> glassMats = new List<Material>();
    public List<Screw> blockScrews = new List<Screw>();
    public float delta = 0.7f;
    public float magnitudeCol, radius, power, upwards;
    private float defaultScale_Size_2 = 1.2f;
    private float special_1_Scale_Size_2 = 1.35f;
    private float special_2_Scale_Size_2 = 0.95f;
    private float defaultScale_Size_3 = 1.15f;
    private float specialScale_Size_3 = 1.3f;
    private float defaultDistance = 4.6f;
    private int indexHeight;

    public void InitGlass(float posX, float posZ, int indexHeight, float distance, float angle, List<Screw> blockScrews = null, int indexColor = 0, bool isStraight = true)
    {
        firstGlass.SetActive(true);
        brokenGlass.SetActive(false);
        this.blockScrews.Clear();
        this.indexHeight = indexHeight;
        transform.localPosition = new Vector3 (posX, indexHeight * delta, posZ);
        transform.eulerAngles = new Vector3 (0, angle, 0);
        float scale = GameConfig.Instance.RatioScaleScreen > 1 ? GameConfig.Instance.RatioScaleScreen : 1;
        if (size == 2)
        {
            if (isStraight)
            {
                if (distance / defaultDistance < 0.66)
                {
                    float scaleX = scale * special_1_Scale_Size_2 * distance / defaultDistance;
                    transform.localScale = new Vector3(scaleX, 1f, 1.2f);
                }
                else if (distance / defaultDistance > 1)
                {
                    float scaleX = scale * special_2_Scale_Size_2 * distance / defaultDistance;
                    transform.localScale = new Vector3(scaleX, 1f, 1.4f);
                }
                else
                {
                    float scaleX = scale * defaultScale_Size_2 * distance / defaultDistance;
                    transform.localScale = new Vector3(scaleX, 1f, 1.35f);
                }

            }
            else
            {
                float scaleX = scale * special_1_Scale_Size_2 * distance / defaultDistance;
                transform.localScale = new Vector3(scaleX, 1f, 1.35f);
            }

        }
        else
        {
            if (isStraight)
            {
                Debug.Log(distance / defaultDistance);
                if (distance / defaultDistance < 1.35)
                {
                    float scaleX = scale * specialScale_Size_3 * distance / (defaultDistance * 2);
                    transform.localScale = new Vector3(scaleX, 1f, 1.35f);
                }
                else
                {
                    float scaleX = scale * defaultScale_Size_3 * distance / (defaultDistance * 2);
                    transform.localScale = new Vector3(scaleX, 1f, 1.35f);
                }
            }
            else
            {
                float scaleX = scale * specialScale_Size_3 * distance / (defaultDistance * 2);
                transform.localScale = new Vector3(scaleX, 1f, 1.35f);
            }

        }
        if (blockScrews != null )
        {
            foreach (var screw in blockScrews)
            {
                this.blockScrews.Add(screw);
                if (screw.glassBlockIndex.Count > 0 && indexHeight > screw.glassBlockIndex[0])
                {
                    screw.glassBlockIndex.Insert(0, indexHeight);
                }
                else
                    screw.glassBlockIndex.Add(indexHeight);
            }
        }
    }
    public void CheckBreak()
    {
        bool isBreak = true;

        foreach (var screw in blockScrews)
        {
            if (screw.amountSortPre > indexHeight)
            {
                isBreak = false;
                break;
            }
        }

        if (isBreak)
        {
            BreakGlass();
            if (LevelManager.Instance.glassInScene.Contains(this))
                LevelManager.Instance.glassInScene.Remove(this);
        }
    }
    public void BreakGlass()
    {
        foreach (var screw in blockScrews)
        {
            if (screw.glassBlockIndex.Contains(indexHeight))
                screw.glassBlockIndex.Remove(indexHeight);
        } 
        blockScrews.Clear();
        firstGlass.SetActive(false);
        brokenGlass.SetActive(true);
        LevelManager.Instance.ActiveGroundCollider();

        AudioManager.instance.PlaySFX(SoundType.Glass_Break);
        VibrationUtil.Vibrate(50);
        int layerMask = LayerMask.GetMask("Glass");
        Physics.gravity = new Vector3(0, -30, 0);
        foreach (MeshRenderer rb in brokenGlasses)
        {
            foreach (Transform pos in explorePoses)
            {
                rb.GetComponent<Rigidbody>().AddExplosionForce(power, pos.position, radius, upwards);
                rb.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
            }
        }
        DOVirtual.DelayedCall(0.8f, () =>
        {
            for (int i = 0; i < brokenGlasses.Count; i++)
            {
                //Color  color = mesh.material.GetColor("_TintColor");
                //color.a = 0;
                //Color color = new Color(0, 1, 240 / 255f, 1/255f);
                brokenGlasses[i].material.DOFade(0, "_TintColor", 0.6f).SetEase(Ease.InQuad).OnComplete(() =>
                {
                    if (LevelManager.Instance.glassInScene.Contains(this))
                        LevelManager.Instance.glassInScene.Remove(this);
                    Destroy(gameObject);
                    LevelManager.Instance.DisableGroundCollider();
                });
            }        
        });
        
    }

    public void ReturnToPool()
    {
        if (blockScrews.Count > 0)
        {
            Factory.Instance.ReturnGlassToPool(size, gameObject);
        }
        else
            Destroy(gameObject);
    }
}
