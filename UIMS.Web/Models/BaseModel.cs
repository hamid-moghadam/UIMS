using System;
using System.Collections.Generic;
using System.Text;

namespace UIMS.Web.Models
{
    public abstract class BaseModel:IKey<int>
    {
        public int Id { get; set; }
    }

    public interface IKey<T>
    {
        T Id { get; set; }
    }
}
