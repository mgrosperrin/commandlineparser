using System.Threading;

namespace MGR.CommandLineParser.Extensibility
{
    internal class ParserOptionsAccessor : IParserOptionsAccessor
    {
        private static readonly AsyncLocal<ParserOptionsHolder> ParserOptionsCurrent = new AsyncLocal<ParserOptionsHolder>();

        public IParserOptions Current
        {
            get { return ParserOptionsCurrent.Value?.Current; }
            set
            {
                var holder = ParserOptionsCurrent.Value;
                if (holder != null)
                {
                    holder.Current = null;
                }

                if (value != null)
                {
                    ParserOptionsCurrent.Value = new ParserOptionsHolder {Current = value};
                }
            }
        }

        private class ParserOptionsHolder
        {
            public IParserOptions Current;
        }
    }
}