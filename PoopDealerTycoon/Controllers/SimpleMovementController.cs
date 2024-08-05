using UnityEngine;
using Chameleon.Game.ArcadeIdle.Abstract;
using System.Collections;
using Chameleon.Game.ArcadeIdle.Upgrade;

namespace Chameleon.Game.ArcadeIdle.Movement
{
    public class SimpleMovementController : BaseMovementController
    {    
        [SerializeField] private float _baseSpeed = 5f;
        private float _movementSpeed;
        [SerializeField] private Transform _model;

        private float _rotationSpeed = 6f;
        private bool _isMoveAvailable = true;
        private bool _isParticleRoutineActive = false;

        private FloatingJoystick joystick;
        private CharacterController _characterController;

        private void Start()
        {
            Helpers.UpgradeSkillDict.upgradableSkillsDict[UpgradeSkillName.PlayerSpeed].UpgradeEvent += UpdateMovementSpeed;
        }

        private void OnDisable()
        {
            Helpers.UpgradeSkillDict.upgradableSkillsDict[UpgradeSkillName.PlayerSpeed].UpgradeEvent -= UpdateMovementSpeed;
        }

        protected override void Initialize() {
            base.Initialize();

            if (TryGetComponent<CharacterController>(out CharacterController characterController))
                _characterController = characterController;
            else
                Debug.LogError("No CharacterController Found!!");
            UpdateMovementSpeed();
            joystick = InputController.instance.GetFloatingJoystick();
            
        }

        private void UpdateMovementSpeed()
        {
            _movementSpeed = _baseSpeed * Helpers.UpgradeSkillDict.upgradableSkillsDict[UpgradeSkillName.PlayerSpeed].GetCurrentLevelCoef();
        }
        
        private void Update(){
            MoveWithRotation();
        }

        private void MoveWithRotation(){
            float inputX = joystick.Horizontal;
            float inputZ = joystick.Vertical;
            Vector3 moveDir = new Vector3(inputX, 0, inputZ);

            if (Mathf.Abs(inputX) > 0 || Mathf.Abs(inputZ) > 0){
                _isMoving = true;
                if(!_isParticleRoutineActive)
                {
                    StartCoroutine(PlayParticlesCoroutine());
                    _isParticleRoutineActive = true;
                }
            }
                
            else{
                _isMoving = false;
                StopAllCoroutines();
                _isParticleRoutineActive = false;
            }                    
            
            if(CanMove(moveDir)) {
                _characterController.SimpleMove(moveDir * _movementSpeed /*+ new Vector3(0, transform.position.y, 0)*/);
                Rotate(moveDir);
                SetMoveAnimationPlaying(true);
            }
            else
                SetMoveAnimationPlaying(false);
        }

        private void Rotate(Vector3 targetDir) {
            if (targetDir == Vector3.zero)
                return;

            Quaternion rotation = Quaternion.Lerp(_model.rotation, Quaternion.LookRotation(targetDir), _rotationSpeed * targetDir.magnitude * Time.deltaTime);
            _model.rotation = rotation;
        }
        
        private bool CanMove(Vector3 moveDir) {
            return _isMoveAvailable && moveDir.magnitude > 0;
        }

        public void SetCanMove(bool canMove){
            _isMoveAvailable = canMove;
        }

        private IEnumerator PlayParticlesCoroutine()
        {
            while(true)
            {
                yield return new WaitForSeconds(.3f);
                GameObject footStepObject = ObjectPool.instance.SpawnFromPool("Footstep", transform.position, Quaternion.identity);
                FootstepBehaviour footstepBehaviour = footStepObject.GetComponent<FootstepBehaviour>();
                footstepBehaviour.StartLifetime();
            }
        }
    }
}