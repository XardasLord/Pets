using System;

namespace Pets.Core.Domain
{
    public class AnimalToCare
    {
        public Guid Id { get; protected set; }
        public Animal Animal { get; protected set; }
        public DateTime DateFrom { get; protected set; }
        public DateTime DateTo { get; protected set; }
        public bool IsTaken { get; protected set; }

        protected AnimalToCare()
        {
        }

        public AnimalToCare(Animal animal, DateTime dateFrom, DateTime dateTo)
        {
            Animal = animal;
            DateFrom = dateFrom;
            DateTo = dateTo;
            IsTaken = false;
        }

        public void SetIsTaken(bool isTaken)
        {
            IsTaken = isTaken;
        }
    }
}
