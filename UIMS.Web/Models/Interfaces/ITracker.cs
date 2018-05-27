using System;
using System.Collections.Generic;
using System.Text;

namespace UIMS.Web.Models.Interfaces
{
    public interface ITracker
    {
        DateTime Created { get; set; }

        DateTime Modified { get; set; }

    }
}
