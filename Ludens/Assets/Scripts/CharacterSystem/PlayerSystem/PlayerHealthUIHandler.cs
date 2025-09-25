using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterSystem.PlayerSystem
{
	public class PlayerHealthUIHandler : MonoBehaviour
	{
		[SerializeField] private Transform canvasTransform;
		[SerializeField] private Image healthBar;
		[SerializeField] private TextMeshProUGUI healthText;

		private Health health;
		private Tween healthTween;
		private bool isPlayerDead;

		private Transform target;
		[SerializeField] private float sensitivity = 100f;
		[SerializeField] private Vector3 offset = Vector3.up;

		public void Initialize(Health health, Transform target)
		{
			this.health = health;
			health.OnHealthChanged += OnHealthChanged;
			health.OnDied += Dead;
			this.target = target;
			canvasTransform.SetParent(null);
			UpdateHealthUI();
			StartCoroutine(FollowPLayer());
		}

		private IEnumerator FollowPLayer()
		{
			if (target == null) yield break;
			while (health.CurrentHealth > 1 || !isPlayerDead)
			{
				Vector3 desiredPosition = target.position + offset;
				Vector3 smoothedPosition = Vector3.Lerp(canvasTransform.position, desiredPosition, sensitivity * Time.deltaTime);

				canvasTransform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, canvasTransform.position.z);
				yield return null;
			}
		}

		private void OnHealthChanged(int changeAmount)
		{
			UpdateHealthUI();
		}

		private void UpdateHealthUI()
		{
			float maxHealth = health.MaxHealth;
			float currentHealth = health.CurrentHealth;

			float fillAmount = currentHealth / maxHealth;
			string percentageText = Mathf.RoundToInt(fillAmount * 100) + "%";

			healthText.text = percentageText;

			healthTween?.Kill();
			healthTween = healthBar.DOFillAmount(fillAmount, 0.5f).SetEase(Ease.OutCubic);
		}

		private void Dead()
		{
			isPlayerDead = true;
			canvasTransform.SetParent(target);
			gameObject.SetActive(false);
		}
	}
}