namespace Leaf.Model
{
    /// <summary>
    /// 单选题和试卷级联类
    /// </summary>
    internal class SingleTest
    {
        //单选题id
        public int SingleId { get; set; }

        //试卷id
        public int TestId { get; set; }

        //单选题
        public virtual SingleChoice single { get; set; }

        //试卷
        public virtual TestPaper test { get; set; }
    }
}