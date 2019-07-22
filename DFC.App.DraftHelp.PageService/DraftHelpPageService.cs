using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using Newtonsoft.Json;

namespace DFC.App.DraftHelp.PageService
{
    public class DraftHelpPageService : IDraftHelpPageService
    {
        public HelpPageModel GetByName(string canonicalName)
        {
            string sampleData;
            switch (canonicalName)
            {
                case "terms-and-conditions":
                    sampleData =
                        "{\r\n    \"CanonicalName\": \"terms-and-conditions\",\r\n    \"BreadcrumbTitle\": \"Terms and conditions\",\r\n    \"IncludeInSitemap\": true,\r\n    \"MetaTags\": {\r\n        \"Title\": \"Terms and conditions | Explore careers\",\r\n        \"Description\": \"Terms and conditions you will work with\",\r\n        \"Keywords\": \"Terms, Conditions\"\r\n    },\r\n    \"Content\": \"<h1 class='heading-xlarge'>Terms and Conditions DRAFT</h1><p>The Chief Executive of Skills Funding (the Chief Executive) operates the National Careers Service which is the publicly funded careers service for adults and young people (aged 13 or over) in England. The service includes the website, services delivered from the website, the National Contact Centre and Careers Advisory Services.</p><p>When using the National Career Service you agree to these Terms and Conditions.</p><ul class='list list-bullet'><li>respect any patents, copyrights and trademarks</li><li>all rights are owned by the Chief Executive</li><li>use Web Chat Services appropriately &ndash; we monitor these services and will take action regarding inappropriate use</li></ul><p>The Chief Executive accepts no liability for:</p><ul class='list list-bullet'><li>loss of access to the website due to routine or emergency maintenance on the system, or due to excessive demands for the service</li><li>loss of data including both data sent and other data held by or on behalf of you</li><li>delay or failure in receipt of data provided by you or to you</li><li>damages arising from your use of the National Careers Service</li></ul><p>For further information regarding what information we collect from you, what we do with this information, who we share this information with and how we protect your Privacy, refer to the <a href='/help/cookies'>National Careers Service Privacy Policy</a>.</p><p>You can end your use of the Service at any time either via the Website or by contacting the National Contact Centre. To end your use of the service using the website, sign into your account and click on the 'Close your account' link.</p><p>If you have any further questions regarding the Service or wish to raise a complaint, please contact the National Contact Centre on 0800 100 900.</p>\",\r\n    \"LastReviewed\": \"2019-07-09T14:16:43.983Z\",\r\n    \"AlternativeNames\": [\r\n        \"tac\"\r\n    ],\r\n   }";
                    break;

                case "cookies":
                    sampleData =
                        "{\r\n \"CanonicalName\": \"privacy-and-cookies\",\r\n \"BreadcrumbTitle\": \"Privacy and Cookies\",\r\n \"IncludeInSitemap\": true,\r\n \"MetaTags\": {\r\n \"Title\": \"Privacy and Cookies | Explore careers\",\r\n \"Description\": \"Privacy and Cookies you will work with\",\r\n \"Keywords\": \"Terms, Conditions\"\r\n                },\r\n \"Content\": \"<h1 class=\\\"heading-xlarge\\\">Privacy and Cookies DRAFT</h1><p>Some privacy and cookie stuff.\\n</p>\",\r\n \"LastReviewed\": \"2019-07-09T14:16:43.983Z\",\r\n \"AlternativeNames\": []\r\n            }";
                    break;

                case "information-sources":
                    sampleData =
                        "{\r\n    \"CanonicalName\": \"information-sources\",\r\n    \"BreadcrumbTitle\": \"Information Sources\",\r\n    \"IncludeInSitemap\": true,\r\n    \"MetaTags\": {\r\n        \"Title\": \"Information Sources | Explore careers\",\r\n        \"Description\": \"Information Sources description\",\r\n        \"Keywords\": \"Information, Sources\"\r\n    },\r\n    \"Content\": \"<h1 class=\\\"heading-xlarge\\\">Information Sources DRAFT</h1><p>Some Information Sources stuff.\\n</p>\",\r\n    \"LastReviewed\": \"2019-07-09T14:16:43.983Z\",\r\n    \"AlternativeNames\": []\r\n}";
                    break;

                default:
                    sampleData =
                        "{\r\n    \"CanonicalName\": \"help\",\r\n    \"BreadcrumbTitle\": \"Help\",\r\n    \"IncludeInSitemap\": true,\r\n    \"MetaTags\": {\r\n        \"Title\": \"Help | Explore careers\",\r\n        \"Description\": \"Help main description\",\r\n        \"Keywords\": \"Help\"\r\n    },\r\n    \"Content\": \"<h1 class=\\\"heading-xlarge\\\">Help DRAFT</h1><p>Some main Help stuff.\\n</p>\",\r\n    \"LastReviewed\": \"2019-07-09T14:16:43.983Z\",\r\n    \"AlternativeNames\": []\r\n}";
                    break;
            }

            return JsonConvert.DeserializeObject<HelpPageModel>(sampleData);
        }
    }
}