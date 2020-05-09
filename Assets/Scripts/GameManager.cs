using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stack
{
    public class GameManager : MonoBehaviour
    {
        #region Events

        public delegate void ScoreUpdated(int score);
        public event ScoreUpdated OnScoreUpdated;

        #endregion

        #region Singleton

        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<GameManager>();
                return _instance;
            }
        }

        #endregion

        private CubeSpawner[] spawners;
        private int spawnerIndex = 0;

        private int score;
        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                SetScore(value);
            }
        }

        private void SetScore(int value)
        {
            if(score != value)
            {
                score = value;
                DispatchOnScoreUpdated(score);
            }
        }

        private void DispatchOnScoreUpdated(int score)
        {
            OnScoreUpdated?.Invoke(score);
        }

        private void Awake()
        {
            _instance = this;
            spawners = FindObjectsOfType<CubeSpawner>();
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
            {
                if(MovingCube.CurrentCube != null && MovingCube.CurrentCube.gameObject != GameObject.Find("Start"))
                    MovingCube.CurrentCube.Stop();

                spawners[spawnerIndex % 2].SpawnCube();
                spawnerIndex++;

                //FindObjectOfType<CubeSpawner>().SpawnCube();
            }
        }
    }
}
