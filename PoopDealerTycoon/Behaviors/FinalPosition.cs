using UnityEngine;

namespace Chameleon.Game.ArcadeIdle
{
    public class FinalPosition : MonoBehaviour
    {
        [SerializeField] private UnorganizedPoopStockPlace _targetPoopStockPlace;
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Poop"))
            {
                PoopBase poop = other.GetComponent<PoopBase>();
                if(poop.GetPassedFinalPosition())
                    return;
                poop.ApplyForce(GetRandomForceDirectionToApply());
                poop.SetPassedFinalPosition(true);
                _targetPoopStockPlace.AddToPoopsInStock(poop);
                _targetPoopStockPlace.DecrementWaitingPoopAmount();
            }
        }

        private Vector3 GetRandomForceDirectionToApply()
        {
            Vector3 forceDirection;
            float randomRotationAngle = Random.Range(-45, 45);
            Vector3 forwardDir = transform.forward;
            forceDirection = Quaternion.AngleAxis(randomRotationAngle, Vector3.up) * forwardDir + Vector3.down;
            return forceDirection;
        }

    }
}
