using System;
using System.Collections.Generic;
using Nop.Data.Mapping;
using Wombit.Plugin.Widgets.BetterDocs.Domain;

namespace Wombit.Plugin.Widgets.BetterDocs.Data
{
    public class BaseNameCompability : INameCompatibility
    {
        public Dictionary<Type, string> TableNames => new Dictionary<Type, string>
        {
            { typeof(Document), "w_Document" }
        };

        public Dictionary<(Type, string), string> ColumnName => new Dictionary<(Type, string), string>
        {

        };
    }
}
