using MemoAtlas_Backend_ASP.Models.Entities;

namespace MemoAtlas_Backend_ASP.Models.DTOs
{
    public class TagGroupData(TagGroup tagGroup)
    {
        public int Id { get; set; } = tagGroup.Id;
        public string Name { get; set; } = tagGroup.Name;
        public string Color { get; set; } = tagGroup.Color;
        public List<TagDetails> Tags { get; set; } = tagGroup.Tags.Select(t => new TagDetails(t)).ToList() ?? [];

        public class TagDetails(Tag tag)
        {
            public int Id { get; set; } = tag.Id;
            public string Name { get; set; } = tag.Name;
            public string Description { get; set; } = tag.Description;
        }
    }
}