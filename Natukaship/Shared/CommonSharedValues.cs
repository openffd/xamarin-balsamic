using System.Collections.Generic;

namespace Natukaship
{
    public class CommonSharedValues
    {
        public bool isEditable { get; set; } = true;
        public bool isRequired { get; set; } = false;
        public List<string> errorKeys { get; set; }
    }
}
