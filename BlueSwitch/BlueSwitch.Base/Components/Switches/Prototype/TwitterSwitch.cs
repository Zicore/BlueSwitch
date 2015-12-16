using System;
using System.Collections.Generic;
using System.Linq;
using BlueSwitch.Base.Components.Base;
using BlueSwitch.Base.Components.Switches.Base;
using BlueSwitch.Base.Processing;
using TweetSharp;

namespace BlueSwitch.Base.Components.Switches.Prototype
{
    public class TwitterSwitch : SwitchBase
    {
        private string accessToken = "174596486-k00EUEBU6u4CEChZh25TNOe9FOP00lXXxzWx7TS0";
        private string accessTokenSecret = "UYQoNylJv7YWwxyeMgA5Mb67XOy4dKue5ynRl6a09qXAS";

        private string token = "9oa5mcx48Fdj7lSROb0CZOV7k";
        private string tokenSecret = "7j9Qzavg6AB3gaiYbxX1sBvj41LzqTDcsVbXQ2knMJn32sN4wW";



        protected override void OnInitialize(RenderingEngine engine)
        {
            AddOutput(typeof (string));
            AddInput(typeof (string));

            SetData(0,new DataContainer(""));
            Name = "Twitter";
            Description = "Twitter";
        }

        public override GroupBase OnSetGroup()
        {
            return GroupBase.SocialMedia;
        }

        protected override void OnProcessData<T>(Processor p, ProcessingNode<T> node)
        {
            var data = GetData(0);
            var q = data.Value.ToString();

            if (!String.IsNullOrEmpty(q))
            {
                TwitterService service = new TwitterService();
                service.AuthenticateWith(token, tokenSecret, accessToken, accessTokenSecret);

                TwitterSearchResult res = service.Search(new SearchOptions {Q = q});

                IEnumerable<TwitterStatus> status = res.Statuses;

                var first = res.Statuses.LastOrDefault();
                if (first != null)
                {
                    SetData(0, new DataContainer(first.Text));
                }
            }
        }
    }
}
