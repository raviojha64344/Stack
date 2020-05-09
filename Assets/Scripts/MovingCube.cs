using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stack
{
    public class MovingCube : MonoBehaviour
    {
        public static MovingCube CurrentCube { get; private set; }
        public static MovingCube LastCube { get; private set; }
        public MovingDirection MovingDirection { get; private set; }

        [SerializeField]
        private float moveSpeed = 1f;

        private void OnEnable()
        {
            if (LastCube == null)
                LastCube = GameObject.Find("Start").GetComponent<MovingCube>();

            CurrentCube = this;
            CurrentCube.transform.localScale = new Vector3(LastCube.transform.localScale.x, transform.localScale.y, LastCube.transform.localScale.z);
            GetComponent<Renderer>().material.color = GetRandomColor();
        }

        private Color GetRandomColor()
        {
            return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        }

        public void SetDirection(MovingDirection movingDirection)
        {
            MovingDirection = movingDirection;
        }

        private void Update()
        {
            Vector3 direction = MovingDirection == MovingDirection.X ? transform.right : transform.forward;
            transform.position += -direction * Time.deltaTime * moveSpeed;
        }

        public void Stop()
        {
            moveSpeed = 0;
            float hangover = GetHangOver();
            float max = MovingDirection == MovingDirection.X ? LastCube.transform.localScale.x : LastCube.transform.localScale.z;

            if (max <= Mathf.Abs(hangover))
            {
                GameManager.Instance.Score = 0;
                CurrentCube = null;
                LastCube = null;
                SceneManager.LoadScene("Game");
                return;
            }

            float direction = hangover > 0f ? 1f : -1f;
            if(MovingDirection == MovingDirection.X)
            {
                SplitCubeOnX(hangover, direction);
            }
            else
            {
                SplitCubeOnZ(hangover, direction);
            }
            GameManager.Instance.Score++;
            LastCube = this;
        }

        private float GetHangOver()
        {
            if(MovingDirection == MovingDirection.X)
                return transform.position.x - LastCube.transform.position.x;

            return transform.position.z - LastCube.transform.position.z;
        }

        private void SplitCubeOnX(float hangover, float direction)
        {
            float newXSize = LastCube.transform.localScale.x - Mathf.Abs(hangover);
            float fallingBlockSize = transform.localScale.x - newXSize;

            float newXPosition = LastCube.transform.position.x + hangover / 2;

            transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
            transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);

            float cubeEdge = transform.position.x + (newXSize / 2 * direction);
            float fallingBlockXPosition = cubeEdge + fallingBlockSize / 2 * direction;

            SpawnFallingBlock(fallingBlockXPosition, fallingBlockSize);
        }

        private void SplitCubeOnZ(float hangover, float direction)
        {
            float newZSize = LastCube.transform.localScale.z - Mathf.Abs(hangover);
            float fallingBlockSize = transform.localScale.z - newZSize;

            float newZPosition = LastCube.transform.position.z + hangover / 2;

            transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);

            float cubeEdge = transform.position.z + (newZSize / 2 * direction);
            float fallingBlockZPosition = cubeEdge + fallingBlockSize / 2 * direction;

            SpawnFallingBlock(fallingBlockZPosition, fallingBlockSize);
        }

        private void SpawnFallingBlock(float fallingBlockPosition, float fallingBlockSize)
        {
            GameObject fallingBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
            if(MovingDirection == MovingDirection.X)
            {
                fallingBlock.transform.position = new Vector3(fallingBlockPosition, transform.position.y, transform.position.z);
                fallingBlock.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                fallingBlock.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockPosition);
                fallingBlock.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
            }

            fallingBlock.AddComponent<Rigidbody>();
            fallingBlock.GetComponent<Rigidbody>().AddForce(Vector3.up * 100f);
            fallingBlock.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;

            Destroy(fallingBlock.gameObject, 1f);
        }
    }
}
