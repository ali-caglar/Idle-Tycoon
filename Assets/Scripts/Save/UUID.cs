using System;
using Enums.ID;

namespace Save
{
    [Serializable]
    public class UUID
    {
        #region FIELDS

        public string uniqueID;
        public WorldName worldName;
        public RegionName regionName;

        #endregion

        #region CONSTRUCTOR

        public UUID(string uniqueID, WorldName worldName, RegionName regionName)
        {
            this.uniqueID = uniqueID;
            this.worldName = worldName;
            this.regionName = regionName;
        }

        #endregion

        #region PUBLIC METHODS

        public UUID Clone()
        {
            return new UUID(this.uniqueID, this.worldName, this.regionName);
        }

        public void ChangeUniqueID()
        {
            uniqueID = Guid.NewGuid().ToString();
        }

        #endregion
    }
}