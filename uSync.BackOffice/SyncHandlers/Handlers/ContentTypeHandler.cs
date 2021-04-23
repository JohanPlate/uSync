﻿using System;

using Microsoft.Extensions.Logging;

using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Entities;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Extensions;

using uSync.BackOffice.Configuration;
using uSync.BackOffice.Services;
using uSync.Core;
using uSync.Core.Serialization;

using static Umbraco.Cms.Core.Constants;

namespace uSync.BackOffice.SyncHandlers.Handlers
{
    [SyncHandler("contentTypeHandler", "DocTypes", "ContentTypes", uSyncBackOfficeConstants.Priorites.ContentTypes,
            IsTwoPass = true, Icon = "icon-item-arrangement", EntityType = UdiEntityType.DocumentType)]
    public class ContentTypeHandler : SyncHandlerContainerBase<IContentType, IContentTypeService>, ISyncHandler, ISyncPostImportHandler,
        INotificationHandler<SavedNotification<IContentType>>,
        INotificationHandler<DeletedNotification<IContentType>>,
        INotificationHandler<MovedNotification<IContentType>>,
        INotificationHandler<EntityContainerSavedNotification>
    {
        private readonly IContentTypeService contentTypeService;

        public ContentTypeHandler(
            IShortStringHelper shortStringHelper,
            ILogger<ContentTypeHandler> logger,
            uSyncConfigService uSyncConfig,
            IContentTypeService contentTypeService,
            IEntityService entityService,
            AppCaches appCaches,
            ISyncSerializer<IContentType> serializer,
            ISyncItemFactory syncItemFactory,
            SyncFileService syncFileService)
            : base(shortStringHelper, logger, uSyncConfig, appCaches, serializer, syncItemFactory, syncFileService, entityService)
        {
            this.contentTypeService = contentTypeService;
        }

        protected override string GetItemFileName(IUmbracoEntity item, bool useGuid)
        {
            if (useGuid) return item.Key.ToString();

            if (item is IContentTypeBase baseItem)
            {
                return baseItem.Alias.ToSafeFileName(shortStringHelper);
            }

            return item.Name.ToSafeFileName(shortStringHelper);
        }


        protected override IEntity GetContainer(int id)
            => contentTypeService.GetContainer(id);

        protected override IEntity GetContainer(Guid key)
            => contentTypeService.GetContainer(key);

        protected override void DeleteFolder(int id)
            => contentTypeService.DeleteContainer(id);
    }
}
