using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joyride.Platforms
{
    public interface IMobileApplication
    {
        String BundleId { get;  }
        Screen CurrentScreen { get; }
        void Launch();
        void Close();

        void Do<T>(Func<T, Screen> func) where T : class;
        void Do<T>(Action<T> func) where T : class;
    }
}
