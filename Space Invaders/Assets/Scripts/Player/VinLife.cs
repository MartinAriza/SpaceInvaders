using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VinLife : MonoBehaviour
{
    [SerializeField] int maxLife = 100;
    [SerializeField] int lifeLostWhenHit = 5;
    [HideInInspector] public int currentLife;
    [SerializeField] [Tooltip("Time to regenerate 1 life point in seconds")] [Range(0f,5f)]float regenerateTime = 1f;
    [SerializeField] bool regenerate = true;
    [SerializeField] RectTransform lifeBar;
    float maxScale;
    private const float minScale = 0f;
    VinMov VM;
    [SerializeField] float idleBonus = 2f;


    // Start is called before the first frame update
    void Start()
    {
        VM = GetComponent<VinMov>();
        currentLife = maxLife;
        maxScale = lifeBar.localScale.x;
        StartCoroutine("regenerateOverTime");

    }

    private void Update()
    {
        print(currentLife);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "AlienLaser")
        {
            currentLife -= lifeLostWhenHit;
            if (currentLife < 0)
            {
                currentLife = 0;
                die();
            }
            adjustLifeBar();
        }
    }

    IEnumerator regenerateOverTime()
    {
        float time = regenerateTime;
        if (VM.notInput) time /= idleBonus;
        yield return new WaitForSeconds(time);
        if (regenerate)
        {
            currentLife += 1;
            if (currentLife > maxLife)
                currentLife = maxLife;
            adjustLifeBar();
            StartCoroutine("regenerateOverTime");
        }
    }

    private void adjustLifeBar()
    {
        float scale = (float)currentLife / (float)maxLife * maxScale;
        lifeBar.localScale = new Vector3(scale, lifeBar.localScale.y, lifeBar.localScale.z);
    }

    private void die()
    {
        print("game over");
    }
}
