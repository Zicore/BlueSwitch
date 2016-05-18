using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;

namespace BlueSwitch.Base.Services
{
    public class NamespaceResolverService
    {
        public Engine Engine { get; set; }
        public List<SwitchBase> Items { get; } = new List<SwitchBase>();

        public NamespaceResolverService(Engine engine)
        {
            Engine = engine;
        }

        public SwitchBase ResolveByUniqueName(String uniqueName)
        {
            var result = Items.Where(x => x.UniqueName == uniqueName).ToList();
            if (result.Count == 1)
            {
                return result[0];
            }
            return null;
        }

        public SwitchBase ResolveByTypeName(String typeName)
        {
            var result = Items.Where(x => x.TypeName == typeName).ToList();
            if (result.Count == 1)
            {
                return result[0];
            }
            return null;
        }

        public SwitchBase Resolve(UnknownSwitch sw)
        {
            var result =
                Items.FirstOrDefault(
                    x =>
                        x.UniqueName == sw.UniqueNameJson || x.TypeName == sw.TypeNameJson ||
                        x.FullTypeName == sw.FullTypeNameJson);

            if (result != null)
            {
                return Activator.CreateInstance(result.GetType()) as SwitchBase;
            }

            return null;
        }
    }
}
