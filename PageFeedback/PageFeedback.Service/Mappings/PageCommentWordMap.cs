using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using PageFeedback.Service.Models;

namespace PageFeedback.Service.Mappings
{
    public class PageCommentWordMap : ClassMap<PageCommentWord>
    {
        public PageCommentWordMap()
        {
            Table("PageCommentWords");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Word).Length(255).Not.Nullable();
        }
    }
}
