﻿using System;

namespace Pets.Core.Domain
{
    public class AnimalToCare
    {
        public Guid Id { get; protected set; }
        public Guid AnimalId { get; protected set; }
        public Animal Animal { get; protected set; }
        public DateTime DateFrom { get; protected set; }
        public DateTime DateTo { get; protected set; }
        public bool IsTaken { get; protected set; }

        protected AnimalToCare()
        {
        }

        public AnimalToCare(Guid animalId, DateTime dateFrom, DateTime dateTo)
        {
            Id = Guid.NewGuid();
            AnimalId = animalId;
            DateFrom = dateFrom;
            DateTo = dateTo;
            IsTaken = false;
        }

        public void SetIsTaken(bool isTaken)
        {
            IsTaken = isTaken;
        }

        public void SetDateFrom(DateTime dateFrom)
        {
            DateFrom = dateFrom;
        }

        public void SetDateTo(DateTime dateTo)
        {
            DateTo = dateTo;
        }
    }
}
