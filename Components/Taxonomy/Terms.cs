using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Entities.Content;
using DotNetNuke.Entities.Content.Common;

namespace Christoc.Modules.SGGameDistribution.Components.Taxonomy
{
    public class Terms
    {
        public void ManageGameTerms(Game objGame, ContentItem objContent)
        {
            //Remove all terms from Game before managing
            //This prevents having any left over games if they've been remove and not managed during update process.
            RemoveGameTerms(objContent);

            foreach (var term in objGame.Terms)
            {
                Util.GetTermController().AddTermToContent(term, objContent);
            }
        }

        public void RemoveGameTerms(ContentItem objContent)
        {
            Util.GetTermController().RemoveTermsFromContent(objContent);
        }
    }
}