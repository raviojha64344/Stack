using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stack
{
    public class CubeSpawner : MonoBehaviour
    {
        [SerializeField]
        private MovingCube cubePrefab;

        [SerializeField]
        private MovingDirection MovingDirection;

        public void SpawnCube()
        {
            MovingCube cube = Instantiate(cubePrefab);
            if(MovingCube.LastCube != null && MovingCube.LastCube.gameObject != GameObject.Find("Start"))
            {
                float x = MovingDirection == MovingDirection.X ? transform.position.x : MovingCube.LastCube.transform.position.x;
                float z = MovingDirection == MovingDirection.Z ? transform.position.z : MovingCube.LastCube.transform.position.z;

                cube.transform.position = new Vector3(x, MovingCube.LastCube.transform.position.y + cubePrefab.transform.localScale.y, z);
            }
            else
            {
                cube.transform.position = transform.position;
            }
            Debug.Log("Spawn");
            cube.SetDirection(MovingDirection);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireCube(transform.position, cubePrefab.transform.localScale);
        }
    }
}
