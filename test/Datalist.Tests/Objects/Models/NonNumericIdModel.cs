using System;

namespace Datalist.Tests.Objects.Models
{
    public class NonNumericIdModel
    {
        [DatalistColumn]
        public Guid Id { get; set; }
    }
}
