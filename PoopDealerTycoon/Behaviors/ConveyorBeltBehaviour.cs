using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chameleon.Game.ArcadeIdle
{
    public class ConveyorBelt : MonoBehaviour
    {
        [SerializeField]
        private float speed, conveyorSpeed;
        [SerializeField]
        private Vector3 direction;
        private List<Rigidbody> onBelt = new List<Rigidbody>();
        [SerializeField] private MeshRenderer _conveyorMeshRenderer;
        private Material material;

        // Start is called before the first frame update
        void Start()
        {
            /* Create an instance of this texture
            * This should only be necessary if the belts are using the same material and are moving different speeds
            */
            if(conveyorSpeed > 0)
                material = _conveyorMeshRenderer.material;
        }

        // Update is called once per frame
        private void Update()
        {
            // Move the conveyor belt texture to make it look like it's moving
            if(material != null)
                material.mainTextureOffset += new Vector2(0, -1) * conveyorSpeed * Time.deltaTime;
        }

        // Fixed update for physics
        void FixedUpdate()
        {
            // For every item on the belt, add force to it in the direction given
            for (int i = 0; i <= onBelt.Count - 1; i++)
            {
                onBelt[i].AddForce(speed * direction);
            }
        }

        // When something collides with the belt
        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.TryGetComponent<Rigidbody>(out Rigidbody collisionRb))
            {
                if(!onBelt.Contains(collisionRb))
                    onBelt.Add(collisionRb);
            }
            
        }

        // When something leaves the belt
        private void OnCollisionExit(Collision collision)
        {
            if(collision.gameObject.TryGetComponent<Rigidbody>(out Rigidbody collisionRb))
            {
                onBelt.Remove(collisionRb);
            }
        }
    }
}