using System.Timers;
using UnityEditor;
using UnityEngine;

namespace _1.Tetris.Scripts
{
    public class CubeFloat : MonoBehaviour
    {
        public float speed, tilt;
        public Vector3 target = new Vector3 (-5, -13, 0);

        void Update(){
        transform.position = Vector3.MoveTowards (transform.position, target, Time.deltaTime * speed);
        if (transform.position == target && target.y != -13f)
            target.y = -13f;
        else if (transform.position == target && target.y == -13f)
            target.y = -11f;
        transform.Rotate(Vector3.up * Time.deltaTime * tilt);
        }
    }
}
