using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JB
{

    public class Target : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] float target_health = 5.0f;
        [SerializeField] bool score_from_centre = false;
        [SerializeField] float score_value = 10;

        [Header("References")]
        [SerializeField] GameObject confetti_prefab;
        [SerializeField] GameObject score_prefab;
        [SerializeField] Transform centre_point;

        private PlayerScore player_scores;

        private Camera cam;
        private DamageFlasher flasher;

        // Use this for initialization
        void Start()
        {
            cam = JHelper.main_camera;
            player_scores = GameObject.Find("ScoreManager").GetComponent<PlayerScore>();
        }


        public void TargetHit(int? _player_id, Transform _tran, int _damage)
        {
            if (flasher == null)
                flasher = GetComponent<DamageFlasher>();

            flasher.DamageFlash();
            target_health -= _damage;

            if (target_health <= 0)
            {
                if (score_from_centre)
                {
                    float dist = (1 / Vector3.Distance(centre_point.position, _tran.position));

                    if (dist > 0)
                    {
                        score_value = 10;

                        if (dist > 0.4)
                        {
                            score_value = 20;

                            if (dist > 0.8)
                            {
                                score_value = 30;
                            }
                        }
                    }
                }

                if (_player_id != null)
                    player_scores.ModifyPlayerScore((int)_player_id, (int)score_value);

                if (!centre_point)
                {
                    centre_point = gameObject.transform;
                }

                Instantiate(confetti_prefab, centre_point.position, transform.rotation);

                Vector3 screen_pos = cam.WorldToScreenPoint(centre_point.position);

                screen_pos.x += 30.0f;

                GameObject score = Instantiate(score_prefab, transform.position, transform.rotation);

                if (_player_id != null)
                    score.GetComponentInChildren<ScoreMovement>().StartValues(screen_pos, score_value, (int)_player_id);

                Destroy(this.gameObject);
            }
        }
    }

} // namespace JB