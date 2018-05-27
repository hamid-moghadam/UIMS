using System;
using System.Collections.Generic;
using System.Text;

namespace UIMS.Web.Models.Interfaces
{
    public interface IUser
    {
        AppUser User { get; set; }

        int UserId { get; set; }
    }
}
