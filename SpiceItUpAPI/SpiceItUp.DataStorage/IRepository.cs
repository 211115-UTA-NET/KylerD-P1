using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiceItUpDataStorage
{
    public interface IRepository
    {
        List<Store> GetStoreList();
    }
}
