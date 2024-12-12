using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class HUD : MonoBehaviour
    {
        [SerializeField]
        private GameObject health;
        private Slider healthSlider; // TODO: Should be encapsulate to its own script. (StatBar)

        [SerializeField]
        private GameObject round;
        private TMP_Text roundText; // TODO: Should be encapsulate to its own script. (InfoBar)

        [SerializeField]
        private GameObject score;
        private TMP_Text scoreText; // TODO: Should be encapsulate to its own script. (InfoBar)

        void Awake()
        {
            healthSlider = health.GetComponent<Slider>();
            roundText = round.GetComponent<TMP_Text>();
            scoreText = score.GetComponent<TMP_Text>();
        }

        public void UpdateHealth(int oldValue, int newValue)
        {
            healthSlider.value = newValue;
        }

        public void UpdateRound(int newRound)
        {
            roundText.text = $"{newRound}";
        }

        public void UpdateScore(int score)
        {
            scoreText.text = $"{score}";
        }
    }
}
