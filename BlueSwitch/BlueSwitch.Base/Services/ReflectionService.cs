using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using NLog;

namespace BlueSwitch
{
    public class ReflectionService
    {
        public ReflectionService()
        {
            
        }

        private Logger _log = LogManager.GetCurrentClassLogger();

        public void LoadFiles(String fileName)
        {
            //var path = AppDomain.CurrentDomain.BaseDirectory;
            //var files = Directory.GetFiles(path,"*.dll", SearchOption.TopDirectoryOnly);

            //foreach (var file in files)
            //{
                
            //}
        }
        
        public List<SwitchBase> LoadAddons(Engine engine)
        {
            var list = new List<SwitchBase>();

            var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "BlueSwitch*.dll", SearchOption.AllDirectories);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.ManifestModule.Name.Contains("BlueSwitch."));

            foreach (var file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (assemblies.All(x => x.ManifestModule.Name != fi.Name))
                {
                    try
                    {
                        Assembly.LoadFile(file);
                    }
                    catch(Exception ex)
                    {
                        _log.Error(ex);
                    }
                }
            }

            assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x=>x.ManifestModule.Name.Contains("BlueSwitch."));
            
            foreach (var asm in assemblies)
            {
                var types = asm.GetTypes();

                foreach (var type in types)
                {
                    if (type.IsSubclassOf(typeof (SwitchBase)))
                    {
                        if (!type.IsAbstract)
                        {
                            var instance = Activator.CreateInstance(type) as SwitchBase;
                            if (instance != null)
                            {
                                instance.Initialize(engine);
                                engine.AddAvailableSwitch(instance); // Important for Search and Help Services
                                instance.InitializeMetaInformation(engine); // Initializes
                                list.Add(instance);
                            }
                        }
                    }
                }
            }
            return list;
        }
    }
}
