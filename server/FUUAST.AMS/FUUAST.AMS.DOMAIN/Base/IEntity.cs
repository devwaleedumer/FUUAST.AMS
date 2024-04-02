using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUUAST.AMS.DOMAIN.Base
{
    // For Generic Id based entities
    public interface IEntity<TId> : IEntity
    {
        public TId Id { get; set; }
    }
    // For Normal Entities
    public interface IEntity
    {
    }
}
