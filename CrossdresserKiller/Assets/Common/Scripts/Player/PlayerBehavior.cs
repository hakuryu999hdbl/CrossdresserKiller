﻿using OctoberStudio.Easing;
using OctoberStudio.Extensions;
using OctoberStudio.UI;
using OctoberStudio.Upgrades;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;

namespace OctoberStudio
{
    public class PlayerBehavior : MonoBehaviour
    {
        private static readonly int DEATH_HASH = "Death".GetHashCode();
        private static readonly int REVIVE_HASH = "Revive".GetHashCode();
        private static readonly int RECEIVING_DAMAGE_HASH = "Receiving Damage".GetHashCode();

        private static PlayerBehavior instance;
        public static PlayerBehavior Player => instance;

        [SerializeField] CharactersDatabase charactersDatabase;

        [Header("Stats")]
        [SerializeField, Min(0.01f)] float speed = 2;
        [SerializeField, Min(0.1f)] float defaultMagnetRadius = 0.75f;
        [SerializeField, Min(1f)] float xpMultiplier = 1;
        [SerializeField, Range(0.1f, 1f)] float cooldownMultiplier = 1;
        [SerializeField, Range(0, 100)] int initialDamageReductionPercent = 0;
        [SerializeField, Min(1f)] float initialProjectileSpeedMultiplier = 1;
        [SerializeField, Min(1f)] float initialSizeMultiplier = 1f;
        [SerializeField, Min(1f)] float initialDurationMultiplier = 1f;
        [SerializeField, Min(1f)] float initialGoldMultiplier = 1;

        [Header("References")]
        [SerializeField] HealthbarBehavior healthbar;
        [SerializeField] Transform centerPoint;
        [SerializeField] PlayerEnemyCollisionHelper collisionHelper;

        public static Transform CenterTransform => instance.centerPoint;
        public static Vector2 CenterPosition => instance.centerPoint.position;

        [Header("Death and Revive")]
        [SerializeField] ParticleSystem reviveParticle;

        [Space]
        [SerializeField] SpriteRenderer reviveBackgroundSpriteRenderer;
        [SerializeField, Range(0, 1)] float reviveBackgroundAlpha;
        [SerializeField, Range(0, 1)] float reviveBackgroundSpawnDelay;
        [SerializeField, Range(0, 1)] float reviveBackgroundHideDelay;

        [Space]
        [SerializeField] SpriteRenderer reviveBottomSpriteRenderer;
        [SerializeField, Range(0, 1)] float reviveBottomAlpha;
        [SerializeField, Range(0, 1)] float reviveBottomSpawnDelay;
        [SerializeField, Range(0, 1)] float reviveBottomHideDelay;

        [Header("Other")]
        [SerializeField] Vector2 fenceOffset;
        [SerializeField] Color hitColor;
        [SerializeField] float enemyInsideDamageInterval = 2f;

        public event UnityAction onPlayerDied;

        public float Damage { get; private set; }
        public float MagnetRadiusSqr { get; private set; }
        public float Speed { get; private set; }

        public float XPMultiplier { get; private set; }
        public float CooldownMultiplier { get; private set; }
        public float DamageReductionMultiplier { get; private set; }
        public float ProjectileSpeedMultiplier { get; private set; }
        public float SizeMultiplier { get; private set; }
        public float DurationMultiplier { get; private set; }
        public float GoldMultiplier { get; private set; }

        public Vector2 LookDirection { get; private set; }
        public bool IsMovingAlowed { get; set; }

        private bool invincible = false;

        private List<EnemyBehavior> enemiesInside = new List<EnemyBehavior>();

        private CharactersSave charactersSave;
        public CharacterData Data { get; set; }
        private CharacterBehavior Character { get; set; }



        [Header("手动修改")]
        public Animator anim;//接入Spine动画机
        private float inputX, inputY;
        private float StopX, StopY;
        private bool isRunning = false;


        private void Awake()
        {
            charactersSave = GameController.SaveManager.GetSave<CharactersSave>("Characters");
            Data = charactersDatabase.GetCharacterData(charactersSave.SelectedCharacterId);

            Character = Instantiate(Data.Prefab).GetComponent<CharacterBehavior>();
            Character.transform.SetParent(transform);
            Character.transform.ResetLocal();

            instance = this;
            healthbar.Init(Data.BaseHP);
            healthbar.SetAutoHideWhenMax(true);
            healthbar.SetAutoShowOnChanged(true);

            RecalculateMagnetRadius(1);
            RecalculateMoveSpeed(1);
            RecalculateDamage(1);
            RecalculateMaxHP(1);
            RecalculateXPMuliplier(1);
            RecalculateCooldownMuliplier(1);
            RecalculateDamageReduction(0);
            RecalculateProjectileSpeedMultiplier(1f);
            RecalculateSizeMultiplier(1f);
            RecalculateDurationMultiplier(1);
            RecalculateGoldMultiplier(1);

            LookDirection = Vector2.right;

            IsMovingAlowed = true;




        }

       //public void OnRun(InputValue value)
       //{
       //    isRunning = value.isPressed;
       //}


        private void Update()
        {
            if (healthbar.IsZero) return;

            foreach(var enemy in enemiesInside)
            {
                if (Time.time - enemy.LastTimeDamagedPlayer > enemyInsideDamageInterval)
                {
                    TakeDamage(enemy.GetDamage());
                    enemy.LastTimeDamagedPlayer = Time.time;
                }
            }

            if (!IsMovingAlowed) return;

            var input = GameController.InputManager.MovementValue;



            // 记录原始输入值（四向判断用）
            inputX = input.x;
            inputY = input.y;



            if (inputX > 0.5f) { inputX = 1; inputY = 0; }
            else if (inputX < -0.5f) { inputX = -1; inputY = 0; }
            else if (inputY > 0.5f && inputX > -0.5f && inputX < 0.5f) { inputX = 0; inputY = 1; }
            else if (inputY < -0.5f && inputX > -0.5f && inputX < 0.5f) { inputX = 0; inputY = -1; }
            else { inputX = 0; inputY = 0; } // 静止时也归零

            // 保存上一次方向（用于静止状态播放对应Idle动画）
            if (inputX != 0 || inputY != 0)
            {
                StopX = inputX;
                StopY = inputY;
        
             
            }

            // 传给 Spine 动画机
            anim.SetFloat("InputX", StopX);
            anim.SetFloat("InputY", StopY);




            // 💥 判断移动强度 → 设定跑 / 走 / 停止状态
            float magnitude = input.magnitude;

            // 默认是走路
            int moveSpeed = 1;


            var gamepad = Gamepad.current;
            var keyboard = Keyboard.current;

            isRunning = false;

            if (gamepad != null && gamepad.leftShoulder.isPressed)
                isRunning = true;

            if (keyboard != null && keyboard.spaceKey.isPressed)
                isRunning = true;


            if (isRunning)
            {
                moveSpeed = 2;
            }

            // 其他平台（如安卓）根据输入强度判断是否为跑（这一段在PC端会被注释掉）
            //if (magnitude > 0.8)
            //{
            //    moveSpeed = 2;
            //}


            // 如果没输入，设置为静止
            if (magnitude < 0.1f)
            {
                moveSpeed = 0;
            }

            anim.SetInteger("Speed", moveSpeed);











            //原版逻辑
            float joysticPower = input.magnitude;
            Character.SetSpeed(joysticPower);

            if (!Mathf.Approximately(joysticPower, 0) && Time.timeScale > 0)
            {
                var frameMovement = input * Time.deltaTime * Speed;

                if (StageController.FieldManager.ValidatePosition(transform.position + Vector3.right * frameMovement.x, fenceOffset))
                {
                    transform.position += Vector3.right * frameMovement.x;
                }

                if (StageController.FieldManager.ValidatePosition(transform.position + Vector3.up * frameMovement.y, fenceOffset))
                {
                    transform.position += Vector3.up * frameMovement.y;
                }

                collisionHelper.transform.localPosition = Vector3.zero;

                Character.SetLocalScale(new Vector3(input.x > 0 ? 1 : -1, 1, 1));

                LookDirection = input.normalized;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsInsideMagnetRadius(Transform target)
        {
            return (transform.position - target.position).sqrMagnitude <= MagnetRadiusSqr;
        }

        public void RecalculateMagnetRadius(float magnetRadiusMultiplier)
        {
            MagnetRadiusSqr = Mathf.Pow(defaultMagnetRadius * magnetRadiusMultiplier, 2);
        }

        public void RecalculateMoveSpeed(float moveSpeedMultiplier)
        {
            Speed = speed * moveSpeedMultiplier;
        }

        public void RecalculateDamage(float damageMultiplier)
        {
            Damage = Data.BaseDamage * damageMultiplier;
            if (GameController.UpgradesManager.IsUpgradeAquired(UpgradeType.Damage))
            {
                Damage *= GameController.UpgradesManager.GetUpgadeValue(UpgradeType.Damage);
            }
        }

        public void RecalculateMaxHP(float maxHPMultiplier)
        {
            var upgradeValue = GameController.UpgradesManager.GetUpgadeValue(UpgradeType.Health);
            healthbar.ChangeMaxHP((Data.BaseHP + upgradeValue) * maxHPMultiplier);
        }

        public void RecalculateXPMuliplier(float xpMultiplier)
        {
            XPMultiplier = this.xpMultiplier * xpMultiplier;
        }

        public void RecalculateCooldownMuliplier(float cooldownMultiplier)
        {
            CooldownMultiplier = this.cooldownMultiplier * cooldownMultiplier;
        }

        public void RecalculateDamageReduction(float damageReductionPercent)
        {
            DamageReductionMultiplier = (100f - initialDamageReductionPercent - damageReductionPercent) / 100f;

            if (GameController.UpgradesManager.IsUpgradeAquired(UpgradeType.Armor))
            {
                DamageReductionMultiplier *= GameController.UpgradesManager.GetUpgadeValue(UpgradeType.Armor);
            } 
        }

        public void RecalculateProjectileSpeedMultiplier(float projectileSpeedMultiplier)
        {
            ProjectileSpeedMultiplier = initialProjectileSpeedMultiplier * projectileSpeedMultiplier;
        }

        public void RecalculateSizeMultiplier(float sizeMultiplier)
        {
            SizeMultiplier = initialSizeMultiplier * sizeMultiplier;
        }

        public void RecalculateDurationMultiplier(float durationMultiplier)
        {
            DurationMultiplier = initialDurationMultiplier * durationMultiplier;
        }

        public void RecalculateGoldMultiplier(float goldMultiplier)
        {
            GoldMultiplier = initialGoldMultiplier * goldMultiplier;
        }

        public void RestoreHP(float hpPercent)
        {
            healthbar.AddPercentage(hpPercent);
        }

        public void Heal(float hp)
        {
            healthbar.AddHP(hp + GameController.UpgradesManager.GetUpgadeValue(UpgradeType.Healing));
        }

        public void Revive()
        {
            Character.PlayReviveAnimation();
            reviveParticle.Play();

            invincible = true;
            IsMovingAlowed = false;
            healthbar.ResetHP(1f);

            Character.SetSortingOrder(102);

            reviveBackgroundSpriteRenderer.DoAlpha(0f, 0.3f, reviveBottomHideDelay).SetUnscaledTime(true).SetOnFinish(() => reviveBackgroundSpriteRenderer.gameObject.SetActive(false));
            reviveBottomSpriteRenderer.DoAlpha(0f, 0.3f, reviveBottomHideDelay).SetUnscaledTime(true).SetOnFinish(() => reviveBottomSpriteRenderer.gameObject.SetActive(false));

            GameController.AudioManager.PlaySound(REVIVE_HASH);
            EasingManager.DoAfter(1f, () =>
            {
                IsMovingAlowed = true;
                Character.SetSortingOrder(0);
            });

            EasingManager.DoAfter(3, () => invincible = false);
        }

        public void CheckTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 7)
            {
                if (invincible) return;

                var enemy = collision.GetComponent<EnemyBehavior>();

                if (enemy != null)
                {
                    enemiesInside.Add(enemy);
                    enemy.LastTimeDamagedPlayer = Time.time;

                    enemy.onEnemyDied += OnEnemyDied;
                    TakeDamage(enemy.GetDamage());
                }
            }
            else
            {
                if (invincible) return;

                var projectile = collision.GetComponent<SimpleEnemyProjectileBehavior>();
                if (projectile != null)
                {
                    TakeDamage(projectile.Damage);
                }
            }
        }

        public void CheckTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 7)
            {
                if (invincible) return;

                var enemy = collision.GetComponent<EnemyBehavior>();

                if (enemy != null)
                {
                    enemiesInside.Remove(enemy);
                    enemy.onEnemyDied -= OnEnemyDied;
                }
            }
        }

        private void OnEnemyDied(EnemyBehavior enemy)
        {
            enemy.onEnemyDied -= OnEnemyDied;
            enemiesInside.Remove(enemy);
        }

        private float lastTimeVibrated = 0f;

        public void TakeDamage(float damage)
        {
            if (invincible || healthbar.IsZero) return;

            healthbar.Subtract(damage * DamageReductionMultiplier);

            Character.FlashHit();

            if (healthbar.IsZero)
            {
                Character.PlayDefeatAnimation();
                Character.SetSortingOrder(102);

                reviveBackgroundSpriteRenderer.gameObject.SetActive(true);
                reviveBackgroundSpriteRenderer.DoAlpha(reviveBackgroundAlpha, 0.3f, reviveBackgroundSpawnDelay).SetUnscaledTime(true);
                reviveBackgroundSpriteRenderer.transform.position = transform.position.SetZ(reviveBackgroundSpriteRenderer.transform.position.z);

                reviveBottomSpriteRenderer.gameObject.SetActive(true);
                reviveBottomSpriteRenderer.DoAlpha(reviveBottomAlpha, 0.3f, reviveBottomSpawnDelay).SetUnscaledTime(true);

                GameController.AudioManager.PlaySound(DEATH_HASH);

                EasingManager.DoAfter(0.5f, () =>
                {
                    onPlayerDied?.Invoke();
                }).SetUnscaledTime(true);

                GameController.VibrationManager.StrongVibration();
            } else
            {
                if(Time.time - lastTimeVibrated > 0.05f)
                {
                    GameController.VibrationManager.LightVibration();
                    lastTimeVibrated = Time.time;
                }
                
                GameController.AudioManager.PlaySound(RECEIVING_DAMAGE_HASH);
            }
        }
    }
}