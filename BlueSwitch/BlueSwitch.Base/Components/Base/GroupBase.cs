using System;

namespace BlueSwitch.Base.Components.Base
{
    public class GroupBase
    {
        public String Name { get; set; }
        public String DisplayName { get; set; }
        public String Description { get; set; }

        public readonly static GroupBase Logic = new GroupBase { Name = "Logic", DisplayName = "Logic"};

        public readonly static GroupBase CodeFlow = new GroupBase { Name = "CodeFlow", DisplayName = "CodeFlow" };

        public readonly static GroupBase LogicInt32 = new GroupBase { Name = "Logic.Int32", DisplayName = "Logic.Int32" };
        public readonly static GroupBase LogicDouble = new GroupBase { Name = "Logic.Double", DisplayName = "Logic.Double" };


        public readonly static GroupBase Base = new GroupBase { Name = "Base", DisplayName = "Base" };

        public readonly static GroupBase Math = new GroupBase { Name = "Math", DisplayName = "Math" };

        public readonly static GroupBase IO = new GroupBase { Name = "IO", DisplayName = "IO" };

        public readonly static GroupBase OpcUa = new GroupBase { Name = "OpcUa", DisplayName = "OPC UA" };

        public readonly static GroupBase SocialMedia = new GroupBase { Name = "SocialMedia", DisplayName = "Social Media" };

        public readonly static GroupBase Variable = new GroupBase { Name = "Variable", DisplayName = "Variable" };

        public readonly static GroupBase Converter = new GroupBase { Name = "Converter", DisplayName = "Converter" };

        public readonly static GroupBase Trigger = new GroupBase { Name = "Trigger", DisplayName = "Trigger" };
    }
}
