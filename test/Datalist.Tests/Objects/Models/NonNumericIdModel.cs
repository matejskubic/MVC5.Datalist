using System;

namespace Datalist.Tests.Objects
{
    public class NonNumericIdModel
    {
        [DatalistColumn]
        public Guid Id { get; set; }
    }
}
