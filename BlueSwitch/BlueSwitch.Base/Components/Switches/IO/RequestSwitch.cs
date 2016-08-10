using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Processing;

//using IntTrader.API.Base.Exchange;
//using IntTrader.API.Currency;
//using IntTrader.API.Exchange.Kraken;
//using IntTrader.API.ExchangeLoader;

namespace BlueSwitch.Base.Components.Switches.IO
{
    public class RequestSwitch : SwitchBase
    {
        protected override void OnInitialize(Engine renderingEngine)
        {
            AddOutput(typeof (decimal));
            UniqueName = "BlueSwitch.Base.Components.Switches.IO.Request";
            DisplayName = "Request";
            Description = "Request";
        }

        public override GroupBase OnSetGroup()
        {
            return Groups.IO;
        }

        protected override void OnProcess<T>(Processor p, ProcessingNode<T> node)
        {
            //KrakenExchange kraken = new KrakenExchange(new ExchangeManager());

            //var price = kraken.RequestTicker(kraken.PairManager.GetPair(PairBase.BTCEUR));

            //SetData(0,new DataContainer((double)price.LastPrice));
        }
    }
}
