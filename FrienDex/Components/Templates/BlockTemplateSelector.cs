using FrienDex.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrienDex.Components.Templates
{
    public class BlockTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextBlockTemplate { get; set; }
        public DataTemplate ImageBlockTemplate { get; set; }
        public DataTemplate DatePickerBlockTemplate { get; set; }
        public DataTemplate EventBlockTemplate { get; set; }
        public DataTemplate RelationshipBlockTemplate { get; set; }
        public DataTemplate ContactBlockTemplate { get; set; }

        public BlockTemplateSelector()
        {
            TextBlockTemplate = new DataTemplate(() => new TextBlock());
            ImageBlockTemplate = new DataTemplate(() => new ImageBlock());
            DatePickerBlockTemplate = new DataTemplate(() => new DatePickerBlock());
            EventBlockTemplate = new DataTemplate(() => new EventBlock());
            RelationshipBlockTemplate = new DataTemplate(() => new RelationshipBlock());
            ContactBlockTemplate = new DataTemplate(() => new ContactBlock());
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is not Block block)
                return null;

            return block.Type switch
            {
                BlockType.TextBlock => TextBlockTemplate,
                BlockType.ImageBlock => ImageBlockTemplate,
                BlockType.DatePickerBlock => DatePickerBlockTemplate,
                BlockType.EventBlock => EventBlockTemplate,
                BlockType.RelationshipBlock => RelationshipBlockTemplate,
                BlockType.ContactBlock => ContactBlockTemplate,
                _ => null
            };
        }
    }

}
