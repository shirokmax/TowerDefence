using UnityEngine;
using UnityEngine.UI;
using SpaceShooter;

namespace TowerDefence
{
    public class UIHeroPanel : MonoSingleton<UIHeroPanel>
    {
        [SerializeField] private Image m_HeroWindowImage;
        [SerializeField] private Image m_HeroIconImage;
        [SerializeField] private Image m_HeroHitPointsBarImage;
        [SerializeField] private Text m_HeroRespawnCooldownText;

        [SerializeField] private Color m_CooldownIconColor;

        [Space]
        [SerializeField] private Image[] m_SkillIconImages;
        [SerializeField] private Text[] m_SkillCooldownTexts;

        private Button[] m_SkillButtons;

        private void Start()
        {
            if (LevelSequenceController.PlayerHero == null)
            {
                gameObject.SetActive(false);
                enabled = false;
                return;
            }

            m_SkillButtons = new Button[m_SkillIconImages.Length];
            for (int i = 0; i < m_SkillButtons.Length; i++)
                m_SkillButtons[i] = m_SkillIconImages[i].GetComponent<Button>();

            m_HeroIconImage.sprite = LevelSequenceController.PlayerHero.HeroIconSprite;

            m_HeroRespawnCooldownText.gameObject.SetActive(false);

            foreach (var skillCooldownText in m_SkillCooldownTexts)
                skillCooldownText.gameObject.SetActive(false);

            for (int i = 0; i < m_SkillIconImages.Length; i++)
            {
                if (i < Player.Instance.ActiveHero.HeroSkills.Length)
                {
                    m_SkillIconImages[i].gameObject.SetActive(true);
                    m_SkillIconImages[i].sprite = Player.Instance.ActiveHero.HeroSkills[i].SkillIconSprite;
                }
                else
                    m_SkillIconImages[i].gameObject.SetActive(false);
            }

            //Подписка на все ивенты после возрождения игрока
            Player.Instance.EventOnRespawnHero.AddListener(AssignListeners);
            Player.Instance.EventOnRespawnHero.AddListener(OnHeroRespawn);

            //Подписка на все ивенты на старте игры
            AssignListeners();
        }

        private void Update()
        {
            if (m_HeroRespawnCooldownText.text != ((int)Player.Instance.HeroRespawnTimer.CurrentTime).ToString())
                m_HeroRespawnCooldownText.text = ((int)Player.Instance.HeroRespawnTimer.CurrentTime).ToString();

            if (Player.Instance.ActiveHero != null)
            {
                for (int i = 0; i < Player.Instance.ActiveHero.HeroSkills.Length; i++)
                {
                    if (Player.Instance.ActiveHero.HeroSkills[i].CooldownTimer.IsFinished)
                    {
                        m_SkillIconImages[i].color = Color.white;
                        m_SkillCooldownTexts[i].gameObject.SetActive(false);
                        m_SkillButtons[i].interactable = true;
                    }

                    if (m_SkillCooldownTexts[i].gameObject.activeSelf == true)
                    {
                        if (m_SkillCooldownTexts[i].text != ((int)Player.Instance.ActiveHero.HeroSkills[i].CooldownTimer.CurrentTime).ToString())
                            m_SkillCooldownTexts[i].text = ((int)Player.Instance.ActiveHero.HeroSkills[i].CooldownTimer.CurrentTime).ToString();
                    }
                }
            }
        }

        private void AssignListeners()
        {
            if (Player.Instance.ActiveHero == null) return;

            Player.Instance.ActiveHero.EventChangeHitPoints.AddListener(OnHeroHitPointsChange);
            Player.Instance.ActiveHero.EventOnDeath.AddListener(OnHeroDeath);

            InitStats();
        }

        private void InitStats()
        {
            OnHeroHitPointsChange();
        }

        private void OnHeroHitPointsChange()
        {
            m_HeroHitPointsBarImage.fillAmount = (float)Player.Instance.ActiveHero.CurrentHitPoints / Player.Instance.ActiveHero.MaxHitPoints;
        }

        private void OnHeroDeath()
        {
            m_HeroRespawnCooldownText.gameObject.SetActive(true);
            m_HeroWindowImage.color = m_CooldownIconColor;
            m_HeroIconImage.color = m_CooldownIconColor;

            for (int i = 0; i < m_SkillIconImages.Length; i++)
            {
                if (m_SkillIconImages[i].gameObject.activeSelf == true)
                {
                    m_SkillButtons[i].interactable = false;
                    m_SkillIconImages[i].color = m_CooldownIconColor;
                    m_SkillCooldownTexts[i].gameObject.SetActive(false);
                }
            }
        }

        private void OnHeroRespawn()
        {
            m_HeroRespawnCooldownText.gameObject.SetActive(false);
            m_HeroWindowImage.color = Color.white;
            m_HeroIconImage.color = Color.white;

            for (int i = 0; i < Player.Instance.ActiveHero.HeroSkills.Length; i++)
                m_SkillIconImages[i].color = Color.white;

            foreach (var skillIcon in m_SkillIconImages)
            {
                if (skillIcon.gameObject.activeSelf == true)
                    skillIcon.color = Color.white;
            }
        }

        public void OnActivationSkillButtonClick(int skillIndex)
        {
            if (Player.Instance.ActiveHero == null) return;

            Player.Instance.ActiveHero.HeroSkills[skillIndex].OnSkillActivation();

            m_SkillButtons[skillIndex].interactable = false;
            m_SkillIconImages[skillIndex].color = m_CooldownIconColor;
            m_SkillCooldownTexts[skillIndex].gameObject.SetActive(true);

            ClickSpot.EventOnSpotClick.Invoke(null);
        }
    }
}
