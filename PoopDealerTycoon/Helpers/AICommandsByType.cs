using System;
using System.Collections.Generic;
using Chameleon.Game.ArcadeIdle.Commands;

namespace Chameleon.Game.ArcadeIdle.Helpers
{
    public static class AICommandsByType
    {
        private static Dictionary<AICommandType, Type> _aiCommandByType = new Dictionary<AICommandType, Type>()
        {
            {AICommandType.PlacePoop, typeof(PlacePoopCommand) },
            {AICommandType.PlacePoopOnShow, typeof(PlacePoopOnShowCommand)},
            {AICommandType.TakePoop, typeof(TakePoopCommand)},
            {AICommandType.TakePoopFromShow, typeof(TakePoopFromShowCommand)},
            {AICommandType.GotoCheckout, typeof(GotoCheckoutCommand)},
            {AICommandType.LeaveShop, typeof(LeaveShopCommand)}
        };

        public static AICommand GetAICommandByType(AICommandType commandType)
        {
            var aiCommand = Activator.CreateInstance(_aiCommandByType[commandType]);
            return (AICommand)aiCommand;
        }
    }
}
