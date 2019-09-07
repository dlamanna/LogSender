using System;
using System.Collections;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace LogSender
{
    [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]
    public class GlobalVar
    {
        public static ArrayList logSizes;
    }
}
