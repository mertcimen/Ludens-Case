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

		private Transform target;
		[SerializeField] private float sensitivity = 100f;
		[SerializeField] private Vector3 offset = Vector3.up;

		public void Initialize(Health health, Transform target)
		{
			this.health = health;
			health.OnHealthChanged += OnHealthChanged;
			this.target = target;
			canvasTransform.SetParent(null);
			UpdateHealthUI();
		}

		private void Update()
		{
			if (target == null) return;

			Vector3 desiredPosition = target.position + offset;
			Vector3 smoothedPosition =
				Vector3.Lerp(canvasTransform.position, desiredPosition, sensitivity * Time.deltaTime);

			canvasTransform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, canvasTransform.position.z);
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
	}
}