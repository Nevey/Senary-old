using DG.Tweening;
using UnityEngine;

namespace CCore.Senary.Gameplay.Tiles
{
    public class TileView : MonoBehaviour
    {
        [SerializeField] private GameObject hqGameObject;

        public void SetHQVisible(bool visible)
        {
            hqGameObject.SetActive(visible);
        }

        public void AnimateStartHQ(float delay)
        {
            SetHQVisible(false);

            Vector3 targetScale = hqGameObject.transform.localScale;

            hqGameObject.transform.localScale *= 10f;

            Tween scaleTween = hqGameObject.transform.DOScale(targetScale, 0.5f);
            scaleTween.SetEase(Ease.OutBack);
            scaleTween.SetDelay(delay);
            scaleTween.OnStart(() =>
            {
                SetHQVisible(true);
            });

            scaleTween.OnComplete(() =>
            {
                scaleTween.Kill();

                scaleTween = hqGameObject.transform.DOScale(Vector3.zero, 0.5f);
                scaleTween.SetEase(Ease.InBack);
                scaleTween.SetDelay(1f);

                scaleTween.Play();
            });

            scaleTween.Play();
        }

        public void AnimateAddUnits(int amount)
        {
            
        }
    }
}