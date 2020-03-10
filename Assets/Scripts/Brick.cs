using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour
{
    public int hitPoints = 1;
    public ParticleSystem destroyEffect;
    private SpriteRenderer sr;

    public static event Action<Brick> OnBrickDestruction;

    private void Awake()
    {
        this.sr = this.GetComponent<SpriteRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        CollisionLogic(ball);
    }

    private void CollisionLogic(Ball ball)
    {
        this.hitPoints--;
        if (this.hitPoints <= 0)
        {
            BrickManager.Instance.RemainingBricks.Remove(this);
            OnBrickDestruction?.Invoke(this);
            SpawnDestroyEffect();
            Destroy(this.gameObject);
        }
        else
        {
            sr.sprite = BrickManager.Instance.Sprites[this.hitPoints - 1];
        }
    }

    private void SpawnDestroyEffect()
    {
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnEffectPos = new Vector3(brickPos.x, brickPos.y, brickPos.z - 0.2f);
        GameObject effect = Instantiate(destroyEffect.gameObject, spawnEffectPos, Quaternion.identity);

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = this.sr.color;
        Destroy(effect, destroyEffect.main.startLifetime.constant);
    }

    public void Init(Transform containerTransform, Sprite sprite, Color color, int _hitPoints)

    {
        transform.SetParent(containerTransform);
        sr.sprite = sprite;
        sr.color = color;
        hitPoints = _hitPoints;
    }
}
