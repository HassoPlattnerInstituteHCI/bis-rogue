using UnityEngine;
using TMPro;

public class HealthTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private PlayerSimple player;

    private void Start()
    {
        player.OnHealthChanged += UpdateText;
        UpdateText(player.currentHealth);
    }

    private void UpdateText(int current)
    {
        healthText.text = $"HP: {current}";
    }

    private void OnDestroy()
    {
        player.OnHealthChanged -= UpdateText;
    }
}
