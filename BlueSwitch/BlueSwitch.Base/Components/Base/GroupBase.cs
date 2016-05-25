using System;

namespace BlueSwitch.Base.Components.Base
{
    public static class Groups
    {
        public static readonly GroupBase Logic = new GroupBase { Name = "Logic", DisplayName = "Logic" };
        public static readonly GroupBase CodeFlow = new GroupBase { Name = "CodeFlow", DisplayName = "CodeFlow" };
        public static readonly GroupBase LogicInt32 = new GroupBase { Name = "Logic.Int32", DisplayName = "Logic.Int32" };
        public static readonly GroupBase LogicDouble = new GroupBase { Name = "Logic.Double", DisplayName = "Logic.Double" };
        public static readonly GroupBase Base = new GroupBase { Name = "Base", DisplayName = "Base" };
        public static readonly GroupBase Math = new GroupBase { Name = "Math", DisplayName = "Math" };
        public static readonly GroupBase Debug = new GroupBase { Name = "Debug", DisplayName = "Debug" };
        public static readonly GroupBase IO = new GroupBase { Name = "IO", DisplayName = "IO" };
        public static readonly GroupBase FileSystem = new GroupBase { Name = "FileSystem", DisplayName = "FileSystem" };
        public static readonly GroupBase OpcUa = new GroupBase { Name = "OpcUa", DisplayName = "OPC UA" };
        public static readonly GroupBase SocialMedia = new GroupBase { Name = "SocialMedia", DisplayName = "Social Media" };
        public static readonly GroupBase Variable = new GroupBase { Name = "Variable", DisplayName = "Variable" };
        public static readonly GroupBase Setter = new GroupBase { Name = "Setter", DisplayName = "Setter" };
        public static readonly GroupBase Getter = new GroupBase { Name = "Getter", DisplayName = "Getter" };
        public static readonly GroupBase Converter = new GroupBase { Name = "Converter", DisplayName = "Converter" };
        public static readonly GroupBase Trigger = new GroupBase { Name = "Trigger", DisplayName = "Trigger" };
        public static readonly GroupBase Text = new GroupBase { Name = "Text", DisplayName = "Text" };
    }

    public class GroupBase
    {
        public String Name { get; set; }
        public String DisplayName { get; set; }
        public String Description { get; set; }
    }
}
