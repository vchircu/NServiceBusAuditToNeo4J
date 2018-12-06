namespace ModelBuilder
{
    using System;

    public class InterimMessage
    {
        public string Type { get; set; }

        public string Intent { get; set; }

        public Guid RelatedTo { get; set; }
    }
}