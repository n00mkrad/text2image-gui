using System.Collections.Generic;

namespace StableDiffusionGui.Implementations
{
    public class OperationOrder
    {
        public enum LoopAction { Prompt, Iteration, Scale, Guidance, Step, InitImg, InitStrength, LoraWeight, RefineStrength }
        public List<LoopAction> LoopOrder = new List<LoopAction> { LoopAction.Prompt, LoopAction.InitImg, LoopAction.InitStrength, LoopAction.Iteration, LoopAction.LoraWeight, LoopAction.Scale, LoopAction.Guidance, LoopAction.Step };
        public List<LoopAction> SeedIncrementActions = new List<LoopAction> { LoopAction.Iteration };
        public List<LoopAction> SeedResetActions = new List<LoopAction> { LoopAction.Prompt };

        public OperationOrder () { }
        public OperationOrder (List<LoopAction> order, List<LoopAction> incrementActions = null, List<LoopAction> seedResetActions = null)
        {
            LoopOrder = order;

            if(incrementActions != null)
                SeedIncrementActions = incrementActions;

            if (seedResetActions != null)
                SeedResetActions = seedResetActions;
        }
    }
}
