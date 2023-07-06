using System;

namespace Datas.DataModels.Generators
{
    public class UserDataForGeneratorDataModel
    {
        public string IdentifierID;
        public bool IsUnlocked;
        public ulong CurrentLevel;

        public UserDataForGeneratorDataModel(string identifierID, bool isUnlocked, ulong currentLevel)
        {
            if (string.IsNullOrEmpty(identifierID))
            {
                throw new Exception("Identifier id can't be null or empty.");
            }

            if (currentLevel <= 0)
            {
                currentLevel = 1;
            }

            IdentifierID = identifierID;
            IsUnlocked = isUnlocked;
            CurrentLevel = currentLevel;
        }
    }
}