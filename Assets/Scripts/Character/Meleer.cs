using System;
using UnityEngine;

public class Meleer : MonoBehaviour
{
    [SerializeField] MeleeHit[] hits;

    Timer timer;

    int currentHit;

    public bool hitting { get; private set; }
    public bool cding { get; private set; }

    PlayerAnimator PlayerAnimator => FindAnyObjectByType<PlayerAnimator>();

    private void Awake()
    {
        timer = new Timer(0);
    }

    private void Start()
    {
        foreach (var meleeHit in hits)
        {
            meleeHit.hurtBox.Apply = false;
        }
    }

    private void Update()
    {
        if (hitting)
        {
            if (timer.UpdateTimer())
            {
                hits[currentHit].hurtBox.Apply = false;
                if (cding)
                {
                    hitting = false;
                    cding = false;
                }
                else
                {
                    cding = true;
                    timer.ResetTimer(hits[currentHit].cooldown);
                }

            }
        }
    }

    public void Hit(bool forceHit = false)
    {
        if (hitting && !forceHit) return;

		PlayerAnimator.Hit();
		timer.ResetTimer(hits[currentHit].duration);
        hits[currentHit].hurtBox.Apply = true;

        hitting = true;

        currentHit++;
        if (currentHit >= hits.Length) currentHit = 0;
    }

    [Serializable]
    class MeleeHit
    {
        public HurtBox hurtBox;
        public float duration;
        public float cooldown;
    }
}
