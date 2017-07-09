using System;
using System.Collections.Generic;

namespace Pets.Core.Domain
{
    public class Owner
    {
        public Guid UserId { get; protected set; }

        public IEnumerable<Animal> Animals { get; protected set; }

        protected Owner()
        {
        }

        public Owner(User user)
        {
            UserId = user.Id;
        }
    }
}
