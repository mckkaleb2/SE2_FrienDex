using FrienDex.Components;
using FrienDex.Data.Entities;
namespace FrienDex.Components.Templates;

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
        TextBlockTemplate = new DataTemplate(() => new TextBlockView());
        ImageBlockTemplate = new DataTemplate(() => new ImageBlockView());
        DatePickerBlockTemplate = new DataTemplate(() => new DatePickerBlockView());
        EventBlockTemplate = new DataTemplate(() => new EventBlockView());
        RelationshipBlockTemplate = new DataTemplate(() => new RelationshipBlockView());
        ContactBlockTemplate = new DataTemplate(() => new ContactBlockView());
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
