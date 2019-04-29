using MediatR;
using SurfLevel.Contracts.Models.DatabaseObjects;

namespace SurfLevel.Domain.Events
{
    public class CreatedOrder : INotification
    {
        public Order Order { get; set; }
    }
}
