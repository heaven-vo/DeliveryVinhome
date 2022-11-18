using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryVHGP.Core.Interfaces.IRepositories
{
    public interface ISegmentRepository : IRepositoryBase<Segment>
    {
        Task CreatSegment(OrderDto order);
    }
}
