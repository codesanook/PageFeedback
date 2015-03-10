using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using PageFeedback.Service.Models;

namespace PageFeedback.Service.Mappings
{
    public class CommentMap : ClassMap<Comment>
    {
        public CommentMap()
        {
            Table("Comments");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Url).Length(40000).Not.Nullable();
            Map(x => x.Message).Length(40000).Not.Nullable();
        }
    }
}
