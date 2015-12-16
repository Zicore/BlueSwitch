using System;
using System.Collections.Generic;
using System.Reflection;
using BlueSwitch.Base.Attributes;

namespace BlueSwitch.Base.DataHandling
{
    public class DataAttributeHelper
    {
        public SortedDictionary<int, DataEntry> GetObjectDataMapping(Object dataObject)
        {
            SortedDictionary<int, DataEntry> dataMapping = new SortedDictionary<int, DataEntry>();

            PropertyInfo[] modelProperties = dataObject.GetType().GetProperties();
            for (int i = 0; i < modelProperties.Length; i++)
            {
                DataProperty[] attributes = (DataProperty[])modelProperties[i].GetCustomAttributes(typeof(DataProperty), true);
                if (attributes.Length >= 1)
                {
                    DataEntry entry = new DataEntry
                    {
                        DataType = modelProperties[i].PropertyType,
                        Index = attributes[0].Index,
                        Name = attributes[0].Name,
                        PropertyInfo = modelProperties[0],
                    };
                    dataMapping[entry.Index] = entry;
                }
            }
            return dataMapping;
        }
    }
}
