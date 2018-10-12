using System;
using System.Collections.Generic;
using System.Text;

namespace demo_db.Common.Wrappers
{
    public class StringBuilderWrapper : IStringBuilderWrapper
    {
        private readonly StringBuilder builder;

        public StringBuilderWrapper()
        {
            this.builder = new StringBuilder();
        }

        public void AppendLine(string line)
        {
            this.builder.AppendLine(line);
        }

        public override string ToString()
        {
            return this.builder.ToString();
        }
    }
}
