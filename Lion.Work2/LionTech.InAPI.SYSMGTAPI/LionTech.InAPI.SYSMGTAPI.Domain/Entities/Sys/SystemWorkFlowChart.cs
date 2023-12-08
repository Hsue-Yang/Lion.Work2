namespace LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys
{
    public class SystemWorkFlowNodePosition
    {
        public string WFNodeID { get; set; }
        public int NodePOSBeginX { get; set; }
        public int NodePOSBeginY { get; set; }
        public int NodePOSEndX { get; set; }
        public int NodePOSEndY { get; set; }
    }

    public class SystemWorkFlowArrowPosition
    {
        public string WFNodeID { get; set; }
        public string NextWFNodeID { get; set; }
        public int ArrowPOSBeginX { get; set; }
        public int ArrowPOSBeginY { get; set; }
        public int ArrowPOSEndX { get; set; }
        public int ArrowPOSEndY { get; set; }
    }
}