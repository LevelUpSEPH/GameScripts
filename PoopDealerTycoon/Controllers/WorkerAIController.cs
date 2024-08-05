using Chameleon.Game.ArcadeIdle.Helpers;
using System.Collections.Generic;
using Chameleon.Game.ArcadeIdle.Abstract;
using Chameleon.Game.ArcadeIdle.Commands;
using Chameleon.Game.ArcadeIdle.Units;

namespace Chameleon.Game.ArcadeIdle.Movement
{
    public class WorkerAIController : BaseAIController
    {
        private PoopSet _targetPoopSet;
        private List<PoopType> _activePoopTypeList = new List<PoopType>();
        private int _workCount = 0;
        private int _targetPoopTypeIndex = 0;
        private WorkerUnit _workerUnit;
        private int _usedSlotCount = 0;
        private AICommandType _lastPlaceCommandType;
        private PoopType _lastPlacedPoopType;

        private void Awake()
        {
            _targetPoopSet = GetComponent<AIWorkerUnit>().GetTargetSet();
            _activePoopTypeList = PoopTypeActivityController.instance.GetActiveTypesBySet(_targetPoopSet);
            OrganizeActiveTypesList();
            _workerUnit = GetComponent<WorkerUnit>();
            PoopTypeActivityController.NewPoopTypeActivated += OnNewPoopTypeActivated;
        }

        private void OnDestroy()
        {
            PoopTypeActivityController.NewPoopTypeActivated -= OnNewPoopTypeActivated;
        }

        private void OrganizeActiveTypesList() // mostly obsolete, only here for guarantee purposes which is not needed
        {
            for(int i = 0; i + 1 < _activePoopTypeList.Count; i++)
            {
                if((int)_activePoopTypeList[i] > (int)_activePoopTypeList[i + 1])
                {
                    PoopType temp = _activePoopTypeList[i];
                    _activePoopTypeList[i] = _activePoopTypeList[i + 1];
                    _activePoopTypeList[i + 1] = temp;
                    i = 0;
                }
            }
        }

        protected override void HandleCommanding()
        {
            if(!_isAvailableForCommands)
                return;
            _isAvailableForCommands = false;
            if(_workCount % 2 == 0)
            {
                HandleTakeCommand();
            }
            else
            {
                // place command
                HandlePlaceCommand();
            }
        }

        private void HandleTakeCommand()
        {
            PoopType targetPoopType = _activePoopTypeList[_targetPoopTypeIndex % _activePoopTypeList.Count];
            AICommand aiCommand = AICommandsByType.GetAICommandByType(AICommandType.TakePoop);
            GiveCommandWithPoopTarget(aiCommand, targetPoopType, ReleaseAvailibity, _usedSlotCount > 0);
            _usedSlotCount++;
            if(_workerUnit.GetActiveSlotCount() <= _usedSlotCount)
            {
                _usedSlotCount = 0;
                _workCount++;
            }
        }

        private void HandlePlaceCommand()
        {
            AICommand aiCommand;
            if(_workerUnit.GetEveryPoop().Count <= 0)
            {
                _usedSlotCount = 0;
                _workCount++;
                _targetPoopTypeIndex++;
                ReleaseAvailibity();
                return;
            }
            if(_usedSlotCount == 0)
            {
                PoopBase lastPlacedPoop = _workerUnit.GetEveryPoop()[0];
                _lastPlacedPoopType = lastPlacedPoop.GetPoopType(); // type of any poop, cant place if didnt take
                if(_lastPlacedPoopType == PoopType.None || lastPlacedPoop == null)
                {
                    _usedSlotCount = 0;
                    _workCount++;
                    ReleaseAvailibity();
                    return;
                }
                bool isBaseOfSet = lastPlacedPoop.GetIsBaseOfSet();
                if(isBaseOfSet)
                {
                    if(UnityEngine.Random.Range(0, 2) == 0 && _activePoopTypeList.Count > 1)
                    {
                        aiCommand = AICommandsByType.GetAICommandByType(AICommandType.PlacePoop);
                        _lastPlacedPoopType = _activePoopTypeList[UnityEngine.Random.Range(1, _activePoopTypeList.Count)];
                        _lastPlaceCommandType = AICommandType.PlacePoop;
                    }
                    else
                    {
                        aiCommand = AICommandsByType.GetAICommandByType(AICommandType.PlacePoopOnShow);
                        _lastPlaceCommandType = AICommandType.PlacePoopOnShow;
                    }
                }
                else
                {
                    aiCommand = AICommandsByType.GetAICommandByType(AICommandType.PlacePoopOnShow);
                    _lastPlaceCommandType = AICommandType.PlacePoopOnShow;
                }
                
            }
            else
            {
                aiCommand = AICommandsByType.GetAICommandByType(_lastPlaceCommandType);
            }
            GiveCommandWithPoopTarget(aiCommand, _lastPlacedPoopType, ReleaseAvailibity, _usedSlotCount > 0);
            _usedSlotCount++;
            if(_workerUnit.GetActiveSlotCount() <= _usedSlotCount)
            {
                _usedSlotCount = 0;
                _workCount++;
                _targetPoopTypeIndex++;
            }
        }

        public void SkipCycle()
        {
            _workCount++;
            _targetPoopTypeIndex++;
            _usedSlotCount = 0;
        }

        private void OnNewPoopTypeActivated(PoopType poopType)
        {
            if(PoopSetByType.GetPoopSetByType(poopType) != _targetPoopSet)
                return;
            _activePoopTypeList.Add(poopType);
            OrganizeActiveTypesList();
        }

        public PoopSet GetPoopSet()
        {
            return _targetPoopSet;
        }
    }
}
