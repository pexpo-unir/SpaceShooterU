using Enemies;
using UI.HUD;
using UnityEngine;
using UI.MainMenu;
using Players;
using System;
using Audio;
using UI.WinMenu;
using UI.LostMenu;
using ObjectsPool;
using Ships;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameController
{
    public enum GameState { MAINMENU, GAMEPLAY }

    public class GameController : MonoBehaviour
    {
        #region Player Prefs Names

        private readonly string musicVolumeNormalizedPref = "MusicVolumeNormalizedPlayerPrefSS";
        private readonly string sfxVolumeNormalizedPref = "SfxVolumeNormalizedPlayerPrefSS";
        private readonly string playerAutoShootPref = "PlayerAutoShootPlayerPrefSS";
        private readonly string playerHasImmunityPref = "PlayerHasImmunityPlayerPrefSS";

        #endregion

        public GameState GameState { get; set; } = GameState.MAINMENU;

        private int score = 0;

        [SerializeField]
        private PlayerController playerController;

        [SerializeField]
        private EnemySpawner enemySpawner;

        [SerializeField]
        private HUD hud;

        [SerializeField]
        private GameObject mainMenu;

        [SerializeField]
        private OptionsMenu optionsMenu;

        [SerializeField]
        private PauseMenu pauseMenu;

        [SerializeField]
        private WinMenu winMenu;

        [SerializeField]
        private LostMenu lostMenu;

        [SerializeField]
        private ShipPool shipPool;

        [SerializeField]
        private AudioController audioController;

        private InputSystem_Actions inputActions;

        void Awake()
        {
            Debug.Assert(playerController != null, $"Variable {nameof(playerController)} cannot be null.");
            Debug.Assert(enemySpawner != null, $"Variable {nameof(enemySpawner)} cannot be null.");
            Debug.Assert(hud != null, $"Variable {nameof(hud)} cannot be null.");
            Debug.Assert(mainMenu != null, $"Variable {nameof(mainMenu)} cannot be null.");
            Debug.Assert(optionsMenu != null, $"Variable {nameof(optionsMenu)} cannot be null.");
            Debug.Assert(audioController != null, $"Variable {nameof(audioController)} cannot be null.");
            Debug.Assert(shipPool != null, $"Variable {nameof(shipPool)} cannot be null.");

            inputActions = new();
        }

        void Start()
        {
            EnemySpawner.OnRoundChanged += (newRound) => hud.UpdateRound(newRound);
            EnemySpawner.OnPlayerWon += () => SetActiveWinMenu(true);

            PlayerController.OnPlayerDies += () => SetActiveLostMenu(true);

            EnemyController.OnEnemyDied += () => hud.UpdateScore(++score);

            LoadSettings();

            inputActions.Enable();

            SetActiveGamePlay(false);
        }

        void Update()
        {
            if (inputActions.UI.TogglePauseMenu.WasPressedThisFrame())
            {
                if (GameState == GameState.GAMEPLAY)
                {
                    SetActivePauseMenu(!pauseMenu.isActiveAndEnabled);
                }
            }
        }

        public void SetActiveMainMenu(bool value)
        {
            GameState = value ? GameState.MAINMENU : GameState;

            mainMenu.gameObject.SetActive(value);
        }

        public void SetActiveOptionsMenu(bool value)
        {
            optionsMenu.gameObject.SetActive(value);
        }

        public void ReturnFromOptionsMenu()
        {
            SetActiveOptionsMenu(false);

            switch (GameState)
            {
                case GameState.MAINMENU:
                    SetActiveMainMenu(true);
                    break;
                case GameState.GAMEPLAY:
                    SetActivePauseMenu(true);
                    break;
            }

            SaveSettings();
        }

        public void SetActiveGamePlay(bool value)
        {
            if (value)
            {
                score = 0; // Reset Score
                hud.UpdateScore(score);

                GameState = GameState.GAMEPLAY;
            }

            if (!value)
            {
                foreach (Transform child in shipPool.gameObject.transform)
                {
                    if (child.TryGetComponent<Ship>(out var ship))
                    {
                        ship.DamageComponent.GetDamage(ship.DamageComponent.MaxHealth);
                    }
                }
            }

            playerController.gameObject.SetActive(value);
            enemySpawner.gameObject.SetActive(value);
        }

        public void SetActivePauseMenu(bool value)
        {
            pauseMenu.gameObject.SetActive(value);
            SetFreezeGamePlay(value);
        }

        public void SetActiveWinMenu(bool value)
        {
            winMenu.gameObject.SetActive(value);
            SetFreezeGamePlay(value);
        }

        public void SetActiveLostMenu(bool value)
        {
            lostMenu.gameObject.SetActive(value);
            SetFreezeGamePlay(value);
        }

        /// <summary>
        /// Applies <see cref="Time.timeScale"/> to 0 and
        /// LowpassFilter on <see cref="audioController"/>.
        /// </summary>
        /// <param name="value"></param>
        private void SetFreezeGamePlay(bool value)
        {
            if (value)
            {
                Time.timeScale = 0;
                audioController.ApplyLowpassFilter();
            }
            else
            {
                Time.timeScale = 1;
                audioController.RemoveLowpassFilter();
            }
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void LoadSettings()
        {
            if (PlayerPrefs.HasKey(musicVolumeNormalizedPref))
            {
                optionsMenu.SetMusicVolumeValueSlider(PlayerPrefs.GetFloat(musicVolumeNormalizedPref));
            }

            if (PlayerPrefs.HasKey(sfxVolumeNormalizedPref))
            {
                optionsMenu.SetSFXVolumeValueSlider(PlayerPrefs.GetFloat(sfxVolumeNormalizedPref));
            }

            if (PlayerPrefs.HasKey(playerAutoShootPref))
            {
                optionsMenu.SetAutoShootToggle(Convert.ToBoolean(PlayerPrefs.GetInt(playerAutoShootPref)));
            }

            if (PlayerPrefs.HasKey(playerHasImmunityPref))
            {
                optionsMenu.SetHasImmunityToggle(Convert.ToBoolean(PlayerPrefs.GetInt(playerHasImmunityPref)));
            }
        }

        public void SaveSettings()
        {
            PlayerPrefs.Save();
        }

        public void SetMusicVolume(float musicVolume)
        {
            audioController.SetMusicVolume(musicVolume);
            PlayerPrefs.SetFloat(musicVolumeNormalizedPref, musicVolume);
        }

        public void SetSFXVolume(float sfxVolume)
        {
            audioController.SetSFXVolume(sfxVolume);
            PlayerPrefs.SetFloat(sfxVolumeNormalizedPref, sfxVolume);
        }

        public void SetAutoShoot(bool autoShoot)
        {
            playerController.AutoShoot = autoShoot;
            PlayerPrefs.SetInt(playerAutoShootPref, Convert.ToInt32(autoShoot));
        }

        public void SetPlayerImmunity(bool hasImmunity)
        {
            playerController.HasImmunity = hasImmunity;
            PlayerPrefs.SetInt(playerHasImmunityPref, Convert.ToInt32(hasImmunity));
        }
    }
}
