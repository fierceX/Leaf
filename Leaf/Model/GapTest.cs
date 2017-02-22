namespace Leaf.Model
{
    /// <summary>
    /// 填空题和试卷级联表模型
    /// </summary>
    class GapTest
    {
        //填空题id
        public int GapId { get; set; }
        //试卷id
        public int TestId { get; set; }
        //填空题
        public virtual GapFilling gap { get; set; }
        //试卷
        public virtual TestPaper test { get; set; }
    }
}
