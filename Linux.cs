using System.Collections.Generic;
using MVision.Business.Mappings;
using MVision.Database;
using MVision.Database.Commands;
using MVision.Entities;
using MVision.Entities.DTO;
using MVision.Interfaces;

namespace MVision.Business.EditableTemplates
{
    public class TemplateInstanceServiceFactory : ITemplateInstanceServiceFactory
    {
        private readonly IDictionary<MediaType, IEditableTemplateInstanceService> _creationServices =
            new Dictionary<MediaType, IEditableTemplateInstanceService>();

        3
        public TemplateInstanceServiceFactory(IMediaRepository mediaRepository, 
                                            IMapping<Media, MediaDto> mapping, 
                                            IDesignRepository designRepository,
                                            IMediaCommandFactory factory, 
                                            IPlaylistRepository playlistRepository,
                                            IFeedRepository feedRepository,
                                            IFeedScheduler feedScheduler,
                                            IMframeFileHandler mframeFileHandler)
        {
            _creationServices.Add(MediaType.FlashEditable, new FlashInstanceService(mediaRepository, mapping));
            _creationServices.Add(MediaType.HtmlTemplate, new HtmlInstanceService(mediaRepository, mapping, feedRepository, feedScheduler, mframeFileHandler));
            _creationServices.Add(MediaType.DesignTemplate, new DesignLibrary(designRepository, factory, playlistRepository));
        }

        public IEditableTemplateInstanceService GetInstanceService(MediaType mediaType)
        {
            return _creationServices.TryGetValue(mediaType, out var instanceService) ? instanceService : null;
        }
    }
}
