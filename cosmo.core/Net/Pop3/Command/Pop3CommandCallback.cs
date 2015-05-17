using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmo.Net.Pop3
{
    /// <summary>A Callback method which is called by Pop3Client class when pop3 request gets response.
    /// </summary>
    /// <param name="inResult"></param>
    public delegate void Pop3CommandCallback(Pop3CommandResult inResult);
}
