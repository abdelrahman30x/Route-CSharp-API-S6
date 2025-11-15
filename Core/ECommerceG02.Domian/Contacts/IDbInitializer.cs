using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Domian.Contacts
{
    public interface IDbInitializer
    {
        Task IdentityInitializeAsync();

    }
}
