using DG.Tweening;
using UnityEngine;

namespace CCore.Senary.Gameplay.Tiles
{
    public class TileView : MonoBehaviour
    {
        [SerializeField] private TextMesh hqText;

        [SerializeField] private TextMesh unitText;

        private Vector3 originalUnitTextScale;

        private void Awake()
        {
            originalUnitTextScale = unitText.transform.localScale;
        }

        public void SetHQTextVisible(bool visible)
        {
            hqText.gameObject.SetActive(visible);
        }

        public void SetUnitTextVisible(bool visible)
        {
            unitText.gameObject.SetActive(visible);
        }

        public void AnimateStartHQ(float delay)
        {
            SetHQTextVisible(false);

            GameObject hqGameObject = hqText.gameObject;

            Vector3 targetScale = hqGameObject.transform.localScale;

            hqGameObject.transform.localScale *= 10f;

            Tween scaleTween = hqGameObject.transform.DOScale(targetScale, 0.5f);
            scaleTween.SetEase(Ease.OutBack);
            scaleTween.SetDelay(delay);
            scaleTween.OnStart(() =>
            {
                SetHQTextVisible(true);
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

        public void AnimateAddUnits(int totalAmount)
        {
            SetUnitTextVisible(totalAmount > 0);

            unitText.text = totalAmount.ToString();

            Tween scaleTweenDown = unitText.transform.DOScale(originalUnitTextScale * 0.3f, 0.1f);

            Tween scaleTweenUp = unitText.transform.DOScale(originalUnitTextScale, 0.25f);
            scaleTweenUp.SetEase(Ease.OutBack);

            scaleTweenDown.OnComplete(() =>
            {
                scaleTweenDown.Kill();

                scaleTweenUp.Play();
            });

            scaleTweenDown.Play();
        }
    }
}