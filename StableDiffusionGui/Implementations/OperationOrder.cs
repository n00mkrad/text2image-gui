using System.Collections.Generic;

namespace StableDiffusionGui.Implementations
{
    public class OperationOrder
    {
        public enum LoopAction { Prompt, Iteration, Scale, Step, InitImg, InitStrengths, LoraWeight }
        public List<LoopAction> LoopOrder = new List<LoopAction> { LoopAction.Prompt, LoopAction.InitImg, LoopAction.InitStrengths, LoopAction.Iteration, LoopAction.LoraWeight, LoopAction.Scale, LoopAction.Step };
        public List<LoopAction> SeedIncrementActions = new List<LoopAction> { LoopAction.Iteration };
        public List<LoopAction> SeedResetActions = new List<LoopAction> { LoopAction.Prompt };
    }
}
