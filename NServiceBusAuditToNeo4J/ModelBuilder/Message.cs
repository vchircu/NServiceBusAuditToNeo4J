namespace ModelBuilder
{
    using System.Collections.Concurrent;

    public class Message
    {
        public string Id { get; set; }

        public string Intent { get; set; }

        public ConcurrentBag<string> RelatedToMessages { get; } = new ConcurrentBag<string>();

        public void AddRelatedTo(string relatedTo)
        {
            RelatedToMessages.Add(relatedTo);
        }
    }
}