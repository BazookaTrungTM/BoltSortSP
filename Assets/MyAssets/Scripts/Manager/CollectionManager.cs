using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Do;
using DG.Tweening;
using UnityEngine.Events;
using System.Threading.Tasks;
using UnityEngine.UIElements;

public class CollectionManager : Singleton<CollectionManager>
{
    [Range(0.5f, 5f)][SerializeField] private float range = 2f;
    [Range(0.5f, 90f)][SerializeField] private float rotate = 45f;
    public GameObject coinEffectPrefab;
    private readonly List<GameObject> coinEffectList = new List<GameObject>();
    public int maxCoin = 20;
    public int delay = 300;
    public async void PlayCoinEffect(Vector3 spawnPosition, Vector3 targetPosition)
    {
        for (int i = 0; i < maxCoin; i++)
        {
            var nutAnim = GetCoinEffectPrefab();
            nutAnim.gameObject.SetActive(true);
            nutAnim.transform.localScale = Vector3.zero;
            nutAnim.transform.position = spawnPosition;
            nutAnim.transform.DOMove(targetPosition, 1f).SetEase(Ease.InOutQuad);
            nutAnim.transform.DOScale(1, 0.3f).OnComplete(() =>
            {
                DOVirtual.DelayedCall(0.4f, () =>
                {
                    nutAnim.transform.DOScale(0, 0.3f).OnComplete(() =>
                    {
                        nutAnim.SetActive(false);
                    });
                });
            });

            await Task.Delay(delay);
        }

    }
    private GameObject GetCoinEffectPrefab()
    {
        foreach (var obj in coinEffectList)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        var coin = Instantiate(coinEffectPrefab, transform);
        coinEffectList.Add(coin);
        return coin;
    }
    private List<Vector3> GetBetween(Vector2 origin, Vector2 target, int count)
    {
        var between = new List<Vector3>();
        var direction = origin - target;
        direction.Normalize();
        direction *= range;
        for (var i = 0; i < count; i++)
        {
            between.Add(origin + RotateVector2(direction, Random.Range(-rotate, rotate)));
        }
        return between;
    }
    public static Vector2 RotateVector2(Vector2 v, float degrees)
    {
        return Quaternion.Euler(0, 0, degrees) * v;
    }
}
