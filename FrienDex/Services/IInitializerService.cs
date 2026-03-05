using System;
using System.Collections.Generic;
using System.Text;
using FrienDex.Data;
using FrienDex.Services;

namespace FrienDex.Services
{
    public interface IInitializerService
    {

        //public IInitializerService(
        //    DexContext db,
        //    IPersonRepo personRepo);

        #region asyncMethods
        public Task SeedDataAsync();


        #endregion asyncMethods
        // -------------------------
        #region syncronousMethods

        public Task SeedPersonDataAsync();

        public void SeedDataSyncronous();


        public void SeedPersonDataSyncronous();
  

        #endregion syncronousMethods

    }// END InitializerService CLASS
}
