using System;

namespace Datas.DataModels.Workers
{
    public class UserDataForWorkerDataModel
    {
        public string IdentifierID;
        public bool IsUnlocked;
        public string BelongedGeneratorIdentifierID;

        public UserDataForWorkerDataModel(string identifierID, bool isUnlocked, string generatorID)
        {
            if (string.IsNullOrEmpty(identifierID))
            {
                throw new Exception("Identifier id can't be null or empty.");
            }

            IdentifierID = identifierID;
            IsUnlocked = isUnlocked;
            BelongedGeneratorIdentifierID = generatorID;
        }
    }
}