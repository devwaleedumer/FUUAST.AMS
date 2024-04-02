using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUUAST.AMS.DOMAIN.Base
{
    public interface IBaseEntity<TId> : IBaseEntity, IEntity<TId>
    {
    }

    // for normal entities
    public interface IBaseEntity : IEntity
    {
        int? InsertedBy { get; set; }
        DateTime? InsertedDate { get; set; }
        int? UpdatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        bool? IsDeleted { get; set; }
    }
}
