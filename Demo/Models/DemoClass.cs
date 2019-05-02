using System.Collections.Generic;

namespace Demo.Models
{
    public class DemoClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public int? ParentId { get; set; }
        public bool HasChildren { get; set; }
    }

    public class InitialData
    {
        public List<DemoClass> InitialDataList { get; set; }
    }
}
