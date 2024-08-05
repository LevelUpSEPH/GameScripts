using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chameleon.Game.ArcadeIdle.Helpers;
using PlayerData = Game.Scripts.Models.PlayerData;

namespace Chameleon.Game.ArcadeIdle
{
    public class CarGalleryBehavior : MonoBehaviour // 3 types of cars, three spots for different cars. cars can be bought from the cashier?
    {
        public event Action<int> SpawnedCar; // int stands for index
        public event Action<int> CarLeftPoint;
        public event Action<int> CarSpawnTimerChanged;

        [SerializeField] private List<CarSpawnPointBehavior> _spawnPoints = new List<CarSpawnPointBehavior>();
        [SerializeField] private List<string> _spawnableCarTags = new List<string>();
        [SerializeField] private float[] _spawnCarCooldowns = {10, 20, 30};
        [SerializeField] private ParticleSystem _spawnCarParticle;

        private CarSpawningTimer[] _carTimers = new CarSpawningTimer[3];

        private List<CarController> _spawnedCars = new List<CarController>();

        private void Start()
        {
            InitializeCarSpawnTimers();

            foreach(CarSpawnPointBehavior carSpawnPoint in _spawnPoints)
            {
                carSpawnPoint.CarLeftPoint += OnCarLeftPoint;
            }

            SpawnInitialCars();
        }

        private void OnDisable()
        {
            foreach(CarSpawnPointBehavior carSpawnPoint in _spawnPoints)
            {
                carSpawnPoint.CarLeftPoint -= OnCarLeftPoint;
            }
        }
        
        private void SpawnInitialCars()
        {
            SpawnCar(0);
            StartCoroutine(SpawnRoutineForIndex(1));
            StartCoroutine(SpawnRoutineForIndex(2));
        }

        private void InitializeCarSpawnTimers()
        {
            _carTimers[0] = new CarSpawningTimer(_spawnCarCooldowns[0]); 
            _carTimers[1] = new CarSpawningTimer(_spawnCarCooldowns[1]);
            _carTimers[2] = new CarSpawningTimer(_spawnCarCooldowns[2]);
        }

        private void OnCarLeftPoint(CarSpawnPointBehavior carSpawnPoint)
        {
            int leftCarIndex = GetIndexOfPoint(carSpawnPoint);

            CarLeftPoint?.Invoke(leftCarIndex);

            StartCoroutine(SpawnRoutineForIndex(leftCarIndex));
        }

        private IEnumerator SpawnRoutineForIndex(int carIndex)
        {
            _carTimers[carIndex].ResetTimer();
            
            while(_carTimers[carIndex].timeLeftToSpawn > 0)
            {
                yield return new WaitForSeconds(1);
                _carTimers[carIndex].timeLeftToSpawn--;

                CarSpawnTimerChanged?.Invoke(carIndex);
            }

            SpawnCar(carIndex);
        }

        private void SpawnCar(int carIndex)
        {
            CarSpawnPointBehavior targetCarSpawnPoint = _spawnPoints[carIndex];
            Transform carTargetTransform = targetCarSpawnPoint.GetCarTargetTransform();
            string carToSpawn = GetCarTagToSpawnFromIndex(carIndex);
            GameObject spawnedCar = CarPool.instance.SpawnFromPool(carToSpawn, carTargetTransform.position, carTargetTransform.rotation);
            CarController spawnedCarController = spawnedCar.GetComponentInChildren<CarController>();

            targetCarSpawnPoint.SetCarInsidePoint(spawnedCarController);

            SpawnedCar?.Invoke(carIndex);
        }

        private void PlaySpawnCarParticle(Vector3 targetPos)
        {
            _spawnCarParticle.transform.position = targetPos;

            _spawnCarParticle.Play();
        }

        private string GetCarTagToSpawnFromIndex(int carIndex)
        {
            return _spawnableCarTags[carIndex];
        }

        public bool TryGetCarByIndex(int carIndex, out CarController carInSlot)
        {
            carInSlot = null;
            if(!GetHasCarInSlot(carIndex))
                return false;
            
            carInSlot = _spawnPoints[carIndex].GetCarInsidePoint();
            return true;
        }

        private int GetIndexOfPoint(CarSpawnPointBehavior spawnPoint)
        {
            for(int i = 0; i < _spawnPoints.Count; i++)
            {
                if(_spawnPoints[i] == spawnPoint)
                {
                    return i;
                }
            }

            Debug.LogError("Such point has no index");
            return -1;
        }

        public float GetRemainingTimeToSpawnByIndex(int targetIndex)
        {
            return _carTimers[targetIndex].timeLeftToSpawn;
        }

        public bool GetIsCarActiveByIndex(int carIndex)
        {
            CarController targetCar = null;
            
            TryGetCarByIndex(carIndex, out targetCar);
            if(targetCar == null)
                return false;
            
            return targetCar.GetIsCarActive();
        }

        public bool GetHasCarInSlot(int index)
        {
            return _spawnPoints[index].HasCarInsidePoint;
        }

        public void UnlockCar(int indexToUnlock)
        {
            if(_spawnPoints[indexToUnlock].GetCarInsidePoint() == null)
                return;
            _spawnPoints[indexToUnlock].UnlockCar();
        }
    }

    [Serializable]
    public class CarSpawningTimer
    {
        public float carSpawnCooldown;
        public float timeLeftToSpawn;

        public void ResetTimer()
        {
            timeLeftToSpawn = carSpawnCooldown * UpgradeController.instance.restockSpeed[PlayerData.Instance.RestockSpeedLevel - 1].coef;
        }

        public CarSpawningTimer(float carSpawnCooldown)
        {
            this.carSpawnCooldown = carSpawnCooldown;

            ResetTimer();
        }
    }
}