using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content;
using DotNetNuke.Entities.Content.Common;

namespace Christoc.Modules.SGGameDistribution.Components.Taxonomy
{
    public class Content
    {
        private const string ContentTypeName = "SGGameDistribtionGame";

        /// <summary>
        /// This should only run after the Game exists in the data store.
        /// </summary>
        /// <param name="objGame"></param>
        /// <param name="tabId"></param>
        /// <returns>The newly created ContentItemID from the data store.</returns>
        public ContentItem CreateContentItem(Game objGame, int tabId)
        {
            // Get ContentTypeController from DNN
            var typeController = new ContentTypeController();
            // Get List of ContentTypes and find any that match our contentType == "SGGameDistribtionGame"
            var colContentTypes = (from t in typeController.GetContentTypes() where t.ContentType == ContentTypeName select t) ;
            int contentTypeId;

            // If our content type not found i.e. first time usage of SGGameDistribution - Create ContentType
            // Otherwsie just grab Id from any found contentTypes.
            if (colContentTypes.Any())
            {
                var contentType = colContentTypes.Single();
                // TODO: Before resharper contentTypeId = contentType == null ? CreateContentType() : contentType.ContentTypeId; figure changes out
                contentTypeId = contentType?.ContentTypeId ?? CreateContentType();
            }
            else
            {
                contentTypeId = CreateContentType();
            }

            var objContent = new ContentItem
            {
                Content = objGame.GameName + " " + objGame.GameDescription,
                ContentTypeId = contentTypeId,
                Indexed = false,
                ContentKey = "gid=" + objGame.GameId,
                ModuleID = objGame.ModuleId,
                TabID = tabId
            };

            objContent.ContentItemId = Util.GetContentController().AddContentItem(objContent);

            //Add Terms
            var cntTerm = new Terms();
            cntTerm.ManageGameTerms(objGame, objContent);

            return objContent;
        }

        /// <summary>
        /// Used to update the content in the contenItems table.
        /// Should be called whenever a Game is updated.
        /// </summary>
        /// <param name="objGame"></param>
        /// <param name="tabId"></param>
        public void UpdateContentItem(Game objGame, int tabId)
        {
            var objContent = Util.GetContentController().GetContentItem(objGame.ContentItemId);

            if (objContent == null) return;
            objContent.Content = objGame.GameName + " " + objGame.GameDescription;
            objContent.TabID = tabId;
            Util.GetContentController().UpdateContentItem(objContent);

            //Update Terms
            var cntTerm = new Terms();
            cntTerm.ManageGameTerms(objGame, objContent);
        }

        public void DeleteContentItem(Game objGame)
        {
            if (objGame.ContentItemId <= Null.NullInteger) return;
            var objContent = Util.GetContentController().GetContentItem(objGame.ContentItemId);
            if (objContent == null) return;

            // remove any metadata/terms associated first
            // TODO: Swap to ContentItem cascade delete instead.
            var cntTerms = new Terms();
            cntTerms.RemoveGameTerms(objGame);

            Util.GetContentController().DeleteContentItem(objContent);
        }

        #region Private Methods

        /// <summary>
        /// Creates a Content Type (for taxononmy) in the data store.
        /// </summary>
        /// <returns>The primary key value of the new ContentType.</returns>
        private static int CreateContentType()
        {
            var typeController = new ContentTypeController();
            var objContentType = new ContentType { ContentType = ContentTypeName };

            return typeController.AddContentType(objContentType);
        }
        #endregion
    }
}