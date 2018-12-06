namespace ModelBuilder
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class Context
    {
        private readonly ConcurrentBag<string> messages = new ConcurrentBag<string>();

        public string Name { get; set; }

        public IEnumerable<string> ContainedMessage => messages;

        internal void Contains(InterimMessage interimMessage)
        {
            messages.Add(interimMessage.Type);
        }
    }
}