using System;

namespace Save
{
    [Serializable]
    public class UUID
    {
        #region FIELDS

        public string uniqueID;
        public uint worldNumber;
        public uint regionNumber;

        #endregion

        #region PUBLIC METHODS

        public UUID Clone()
        {
            var copy = new UUID
            {
                uniqueID = this.uniqueID,
                worldNumber = this.worldNumber,
                regionNumber = this.regionNumber
            };

            return copy;
        }

        public void ChangeUniqueID()
        {
            uniqueID = Guid.NewGuid().ToString();
        }

        #endregion
    }
}